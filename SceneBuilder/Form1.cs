using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aurora;
using System.Reflection;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using ScintillaNet;
using System.IO;
using System.Text.RegularExpressions;

namespace SceneBuilder
{
  public partial class Form1 : Form
  {
    StringBuilder output;
    Assembly assembly;
    Scene scene;
    RenderTarget target;
    formFindReplace findReplace;
    bool saved;

    string sceneFile = "";
    bool abort;
    string classes = "Scene Camera Light Material Finish Pigment " +
                     "Model Aggregate Triangle Solid Sphere Plane " +
                     "Quadric Ellipsoid CylinderX CylinderY CylinderZ ConeX ConeY ConeZ " +
                     "ParaboloidX ParaboloidY ParaboloidZ Hyperboloid HyperboloidY " +
                     "GSG CSGUnion CSGIntersection CSGDifference CSGModel Cuboid Cube " +
                     "Axis Point3 Vector3 Matrix Transform Ray Colour " +
                     "RealFunctions VectorFunctions Constant";

    string functions = "Checker Onion Perlin PerlinRidged Noise PerlinOctaves" +
                       "Add Rotate Scale Shear Translate Apply " + 
                       "Negate Normalise Norm Transpose Adjoint Inverse Origin Direction Length At";

    string initialScene = @"// Additional using declarations and user defined types

// Required scene builder function
Scene Build()
{ 
  Scene scene = new Scene();
  scene.Size = new ImageSize(1024, 768);
  
  Camera camera = new Camera(new Point3(0.0, 0.0, -15.0),   // Position
               Point3.Origin,                               // Look at
               Vector3.UnitY,                               // Up
               4.0,                                         // Focal length
               scene.Size);                                 // Aspect

  scene.Camera = camera;
  scene.Antialias = true;
  scene.Ambient = new Colour(0.2);

  Light l = new Light(new Point3(4.0, 4.0, -10.0), // Position
            new Colour(1.0),                       // Colour
            0.3);                                  // Intensity
  scene.Add(l);

  // Create Scene elements here

  // Return the complete scene
  return scene;
}

// Other functions as needed
";
    public Form1()
    {
      InitializeComponent();
      output = new StringBuilder();
      assembly = null;
      scene = null;
      target = null;

      scintilla.Lexing.SetKeywords(1, classes);
      scintilla.Lexing.SetKeywords(3, functions);
      scintilla.Margins[0].Width = 30;
      scintilla.Text = initialScene;
      System.Threading.Thread.Sleep(5);
      saved = true;
      Text = WindowTitle();    
    }

    public Assembly CompileScene(string source)
    {
      Write("Compiling...  ");

      // Match "Scene Build()" function
      Regex buildFunc = new Regex(@"^\W*Scene\W+Build\(\)", RegexOptions.Multiline);

      // Replacement text on one line to avoid messing up line numbers 
      string prefix = " using Aurora; class SceneBuilder : ISceneBuilder { public Scene Build() ";

      string suffix = @"}";

      source = buildFunc.Replace(source, prefix) + suffix;

      CodeDomProvider codeProvider = new CSharpCodeProvider();

      CompilerParameters compilerParams = new CompilerParameters();
      compilerParams.CompilerOptions = "/target:library /optimize";
      compilerParams.GenerateExecutable = false;
      compilerParams.GenerateInMemory = true;
      compilerParams.IncludeDebugInformation = false;
      compilerParams.ReferencedAssemblies.Add("mscorlib.dll");
      compilerParams.ReferencedAssemblies.Add("System.dll");
      compilerParams.ReferencedAssemblies.Add("System.Drawing.dll");
      compilerParams.ReferencedAssemblies.Add("aurora.dll");

      CompilerResults results = codeProvider.CompileAssemblyFromSource(compilerParams, source);

      if(results.Errors.Count > 0)
      {
        foreach(CompilerError error in results.Errors)
          WriteLine("{0} at {1}, {2}", error.ErrorText, error.Line, error.Column);

        return null;
      }
      else
      {
        WriteLine("Compiled OK");
        return results.CompiledAssembly;
      }
    }

    private Scene BuildScene(Assembly assembly)
    {
      Write("Building...   ");
      ISceneBuilder sceneBuilder;
      Scene scene = null;
      try
      {
        sceneBuilder = assembly.CreateInstance("SceneBuilder") as ISceneBuilder;
        scene = sceneBuilder.Build();
      }
      catch(Exception e)
      {
        WriteLine(e.Message);
        return null;
      }
      WriteLine("Built OK");
      return scene;
    }

    #region  Scan the image and render

    /// <summary>
    /// The user pressed one of the render buttons
    /// </summary>
    /// <param name="sender">The button</param>
    /// <param name="e">EventArgs</param>
    private void Render()
    {
      Write("Rendering...  ");

      System.Drawing.Size size = new System.Drawing.Size(scene.Size.Width, scene.Size.Height);
      
      // Set the default aspect ratio if none given
      scene.Camera.SetImageSize(scene.Size);

      target = new RenderTarget(size);
      target.Text = WindowTitle(); 
      target.Show();
      Application.DoEvents();

      // Use all the available space
      int w = size.Width;
      int h = size.Height;

      // Output backing store
      Bitmap bm = target.Bitmap;

      double xscale = 2.0 / w;
      double yscale = 2.0 / h;

      // A thin sliding rectangle that Invalidates the most recently drawn line of output
      Rectangle lineRect = new Rectangle(0, 0, 1, h);

      unsafe
      {
        // Lets see how quick we are...
        Stopwatch clock = new Stopwatch();
        clock.Reset();
        clock.Start();

        try
        {

          for(int x = 0; x < w; x++)
          {
            {
              BitmapData bdata = bm.LockBits(lineRect, ImageLockMode.WriteOnly, PixelFormat.Format32bppRgb);
              int* pixptr = (int*)bdata.Scan0;
              pixptr += h;
              Parallel.For(0, h, (y, state) =>
              {
                Colour pixel;

                // Simple 5-point (domino) average antialiasing
                if(scene.Antialias)
                {
                  // Distance from edge samples to centre
                  double spread = 0.4;
                  // Multiply by 1/sqrt(2) to get x, y displacement
                  spread *= 0.7071;
                  // Centre weight [0 - 1]
                  double centre = 0.25;
                  // Divide the remainder equally
                  double edge = (1.0 - centre) / 4.0;

                  pixel = centre * scene.Trace(x * xscale - 1.0, y * yscale - 1.0);

                  pixel += edge * scene.Trace((x + spread) * xscale - 1.0,
                                              (y + spread) * yscale - 1.0);

                  pixel += edge * scene.Trace((x - spread) * xscale - 1.0,
                                              (y + spread) * yscale - 1.0);

                  pixel += edge * scene.Trace((x + spread) * xscale - 1.0,
                                              (y - spread) * yscale - 1.0);

                  pixel += edge * scene.Trace((x - spread) * xscale - 1.0,
                                              (y - spread) * yscale - 1.0);
                }
                else // just trace one ray
                {
                  pixel = scene.Trace(x * xscale - 1.0, y * yscale - 1.0);
                }

                Color col = pixel.Gamma(scene.Gamma);

                *(pixptr - y - 1) = col.ToArgb();

                if(abort)
                {
                  state.Stop();
                  return;
                }
              });
              bm.UnlockBits(bdata);
            }
            // Update display at end of each line
            if(abort || !target.Update(lineRect))
            {
              abort = true;
              return;
            }
           
            // Slide right
            lineRect.Offset(1, 0);
          }
        }

        catch(AuroraException ex)
        {
          MessageBox.Show(ex.Message, "Aurora exception");
          Application.Exit();
        }
        catch(Exception ex)
        {
          MessageBox.Show(ex.Message, "System exception");
          Application.Exit();
        }

        clock.Stop();
        WriteLine("Render complete after {0} S", clock.ElapsedMilliseconds * 0.001);
        WriteLine("{0} Object rays", scene.Rays);
        WriteLine("{0} Shadow rays", scene.ShadowRays);
      }
    }
    #endregion

    #region Message output
    // Functions to output status messages to the lower pane

    void WriteLine(string fmt, params object[] args)
    {
      output.AppendFormat(fmt, args);
      output.AppendLine();
      txtbOut.Text = output.ToString();
      Application.DoEvents();
    }

    void Write(string fmt, params object[] args)
    {
      output.AppendFormat(fmt, args);
      txtbOut.Text = output.ToString();
      Application.DoEvents();
    }

    void ClearMessages()
    {
      output = new StringBuilder();
      txtbOut.Text = "";
      Application.DoEvents();
    }

    #endregion

    #region Event handlers

    private void Compile(object sender, EventArgs e)
    {
      try
      {
        SetBusy(true);
        ClearMessages();
        assembly = CompileScene(scintilla.Text);
      }
      finally
      {
        SetBusy(false);
      }
    }

    private void Render(object sender, EventArgs e)
    {
      try
      {
        SetBusy(true);
        ClearMessages();
        abort = false;
        scene = null;

        if(assembly == null)
          assembly = CompileScene(scintilla.Text);

        if(assembly != null)
          scene = BuildScene(assembly);

        if(scene != null)
          Render();
      }
      finally
      {
        SetBusy(false);
        if(abort)
          WriteLine("Aborted");
        abort = false;
      }
    }

    // Scintilla is firing off TextChanged events when no changes have ocurred
    private void SourceChanged(object sender, NativeScintillaEventArgs e)
    {
      assembly = null;
      if(saved)
      {
        saved = false;
        Text = WindowTitle();
      }
    }

    private void FileNew(object sender, EventArgs e)
    {
      scintilla.Text = initialScene;
      sceneFile = "";
      Text = WindowTitle();
    }

    private void FileOpen(object sender, EventArgs e)
    {
      openFileDialog1.FileName = sceneFile;
      if(openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
      {
        using(StreamReader sr = new StreamReader(openFileDialog1.FileName))
        {
          scintilla.Text = sr.ReadToEnd();
        }
      }
      saved = true;
      sceneFile = openFileDialog1.FileName;
      Text = WindowTitle();
      ClearMessages();
    }

    private void FileSave(object sender, EventArgs e)
    {
      if(string.IsNullOrEmpty(sceneFile))
      {
        saveFileDialog1.FileName = "";
        FileSaveAs(sender, e);
      }
      else
      {
        using(FileStream fs = new FileStream(sceneFile, FileMode.Create))
        {
          using(StreamWriter sw = new StreamWriter(fs))
          {
            sw.Write(scintilla.Text);
          }
        }
      }
      saved = true;
      Text = WindowTitle();
    }

    private void FileSaveAs(object sender, EventArgs e)
    {
      if(saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
      {
        using(FileStream fs = new FileStream(saveFileDialog1.FileName, FileMode.Create))
        {
          using(StreamWriter sw = new StreamWriter(fs))
          {
            sw.Write(scintilla.Text);
          }
        }
      }
      sceneFile = saveFileDialog1.FileName;
      saved = true;
      Text = WindowTitle();
    }

    private void FileQuit(object sender, EventArgs e)
    {
      abort = true;
      Application.DoEvents();
      Close();
    }

    private void Abort(object sender, EventArgs e)
    {
      abort = true;
    }

    void SetBusy(bool busy)
    {
      menuFile.Enabled = !busy;
      menuEdit.Enabled = !busy;
      buttCompile.Enabled = !busy;
      buttRender.Enabled = !busy;
      buttAbort.Enabled = busy;
      scintilla.Enabled = !busy;
    }

    #endregion

    string WindowTitle()
    {
      return "Aurora Raytracer - " + sceneFile; // +(saved ? "" : " *");
    }

    private void EditCopy(object sender, EventArgs e)
    {
      scintilla.Clipboard.Copy();
    }

    private void EditCut(object sender, EventArgs e)
    {
      scintilla.Clipboard.Cut();
    }

    private void EditPaste(object sender, EventArgs e)
    {
      scintilla.Clipboard.Paste();
    }

    private void EditSelectAll(object sender, EventArgs e)
    {
      scintilla.Selection.SelectAll();
    }

    private void EditFindReplace(object sender, EventArgs e)
    {
      if(findReplace == null)
        findReplace = new formFindReplace(scintilla);

      findReplace.Show();
    }
  }
}
