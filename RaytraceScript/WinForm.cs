using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Threading.Tasks;
using Microsoft.ClearScript.V8;

namespace Aurora
{
    public partial class WinForm
    {
        #region Scene construction
        private Scene CreateScene(string scriptPath)
        {
            var scene = new Scene();
            
            // Set up sensible defaults
            scene.Size = new ImageSize(picture.Width, picture.Height);
            // Cosmic background radiation!
            scene.Background = new Colour(0.0);
            // Render shadows
            scene.Shadow = true;
            // Ambient light
            scene.Ambient = new Colour(0.1);
            // Reflection recursion depth
            scene.TraceMax = 20;
            // Light level threshold for recursion
            scene.MinWeight = 0.004;
            // Enable antialiasing
            scene.Antialias = true;

            var script = System.IO.File.ReadAllText(scriptPath);

            using (var engine = new V8ScriptEngine(V8ScriptEngineFlags.EnableDebugging, 9222))
            {
                InjectTypes(engine);
                InjectFunctions(engine);
                engine.Execute(script);
                engine.Script.Build(scene);
                return scene;
            }
        }

        private static void InjectTypes(V8ScriptEngine engine)
        {
            engine.AddHostType("ImageSize", typeof(ImageSize));
            engine.AddHostType("Point3", typeof(Point3));
            engine.AddHostType("Vector3", typeof(Vector3));
            engine.AddHostType("Axis", typeof(Axis));
            engine.AddHostType("Matrix", typeof(Matrix));
            engine.AddHostType("Transform", typeof(Transform));
            engine.AddHostType("Scene", typeof(Scene));
            engine.AddHostType("Camera", typeof(Camera));
            engine.AddHostType("Light", typeof(Light));
            engine.AddHostType("Colour", typeof(Colour));
            engine.AddHostType("Finish", typeof(Finish));
            engine.AddHostType("Pigment", typeof(Pigment));
            engine.AddHostType("Material", typeof(Material));

            engine.AddHostType("Model", typeof(Model));
            engine.AddHostType("Aggregate", typeof(Aggregate));
            engine.AddHostType("Triangle", typeof(Triangle));

            engine.AddHostType("Solid", typeof(Solid));
            engine.AddHostType("Sphere", typeof(Sphere));
            engine.AddHostType("Plane", typeof(Plane));

            engine.AddHostType("Quadric", typeof(Quadric));
            engine.AddHostType("Ellipsoid", typeof(Ellipsoid));
            engine.AddHostType("CylinderX", typeof(CylinderX));
            engine.AddHostType("CylinderY", typeof(CylinderY));
            engine.AddHostType("CylinderZ", typeof(CylinderZ));
            engine.AddHostType("ConeX", typeof(ConeX));
            engine.AddHostType("ConeY", typeof(ConeY));
            engine.AddHostType("ConeZ", typeof(ConeZ));
            engine.AddHostType("ParaboloidX", typeof(ParaboloidX));
            engine.AddHostType("ParaboloidY", typeof(ParaboloidY));
            engine.AddHostType("ParaboloidZ", typeof(ParaboloidZ));
            engine.AddHostType("Hyperboloid", typeof(Hyperboloid));
            engine.AddHostType("HyperboloidY", typeof(HyperboloidY));

            engine.AddHostType("CSG", typeof(CSG));
            engine.AddHostType("CSGUnion", typeof(CSGUnion));
            engine.AddHostType("CSGIntersection", typeof(CSGIntersection));
            engine.AddHostType("CSGDifference", typeof(CSGDifference));
            engine.AddHostType("CSGModel", typeof(CSGModel));
            engine.AddHostType("Cuboid", typeof(Cuboid));
            engine.AddHostType("Cube", typeof(Cube));

            engine.AddHostType("RealFunction", typeof(RealFunction));
            engine.AddHostType("RealFunctions", typeof(RealFunctions));
            engine.AddHostType("VectorFunction", typeof(VectorFunction));
            engine.AddHostType("VectorFunctions", typeof(VectorFunctions));
        }

        private static void InjectFunctions(V8ScriptEngine engine)
        {
            engine.Execute(@"function glue(s, t) { return new CSGUnion(s, t, s.Material || t.Material); }");
            engine.Execute(@"function cut(s, t) { return new CSGDifference(s, t, s.Material || t.Material); }");
            engine.Execute(@"function intersect(s, t) { return new CSGIntersection(s, t, s.Material || t.Material); }");
        }
    #endregion

    #region  Scan the image and render

    /// <summary>
    /// The user pressed one of the render buttons
    /// </summary>
    /// <param name="sender">The button</param>
    /// <param name="e">EventArgs</param>
    private void buttStart_Click(object sender, System.EventArgs e)
        {
            ButtonsOn(false);

            // Create the scene
            var scene = CreateScene(txtScriptPath.Text);

            // Use all the available space
            var w = picture.Width;
            var h = picture.Height;

            // Output backing store
            bm = new Bitmap(w, h);

            var xscale = 2.0 / w;
            var yscale = 2.0 / h;

            // A thin sliding rectangle that Invalidates the most recently drawn line of output
            var lineRect = new Rectangle(0, 0, 1, h);

            unsafe
            {

                // Lets see how quick we are...
                var clock = new Stopwatch();
                clock.Reset();
                clock.Start();

                try
                {

                    for (var x = 0; x < w; x++)
                    {
                        {
                            var bdata = bm.LockBits(lineRect, ImageLockMode.WriteOnly, PixelFormat.Format32bppRgb);
                            var pixptr = (int*)bdata.Scan0;
                            pixptr += h;
                            Parallel.For(0, h, y =>
                            {
                                Colour pixel;

                                // Simple 5-point (domino) average antialiasing
                                if (scene.Antialias)
                                {
                                    // Distance from edge samples to centre
                                    var spread = 0.4;
                                    // Multiply by 1/sqrt(2) to get x, y displacement
                                    spread *= 0.7071;
                                    // Centre weight [0 - 1]
                                    var centre = 0.25;
                                    // Divide the remainder equally
                                    var edge = (1.0 - centre) / 4.0;
                                    // Convolve!
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

                                if (abort)
                                    abort = false;
                            });
                            bm.UnlockBits(bdata);
                        }
                        // Update display at end of each line
                        picture.Invalidate(lineRect);
                        // Update GUI
                        Application.DoEvents();
                        // Slide right
                        lineRect.Offset(1, 0);
                    }
                }

                catch (AuroraException ex)
                {
                    MessageBox.Show(ex.Message, "Aurora exception");
                    Application.Exit();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "System exception");
                    Application.Exit();
                }

                clock.Stop();
                label1.Text = string.Format("Last render: {0} S", clock.ElapsedMilliseconds * 0.001);
                label2.Text = string.Format("{0} Object rays", scene.Rays);
                label3.Text = string.Format("{0} Shadow rays", scene.ShadowRays);

                ButtonsOn(true);
            }
        }
        #endregion

        #region UI stuff

        private void ButtonsOn(bool b)
        {
            buttStart.Enabled = b;
            buttSave.Enabled = b;
            buttAbort.Enabled = !b;
        }

        // Update screen at end of line
        private void picture_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            if (bm != null)
            {
                //lock(bm)
                //{
                e.Graphics.DrawImage(bm, 0, 0);
                //}
            }
        }

        private void WinForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            abort = true;
        }

        private void butAbort_Click(object sender, EventArgs e)
        {
            abort = true;
            ButtonsOn(true);
        }

        private void SaveImage(object sender, EventArgs e)
        {
            var dr = saveFileDialog1.ShowDialog();
            if (dr != DialogResult.OK)
                return;

            bm.Save(saveFileDialog1.FileName);
        }

        private void OpenScript(object sender, EventArgs e)
        {
            var dr = openFileDialog1.ShowDialog();
            if (dr != DialogResult.OK)
                return;

            txtScriptPath.Text = openFileDialog1.FileName;
            buttStart.Enabled = true;
        }
    }
    #endregion
}
