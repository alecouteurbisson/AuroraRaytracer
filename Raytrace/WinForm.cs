using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Threading.Tasks;

namespace Aurora
{
  public partial class WinForm
  {
    #region Scene selection and construction
    void SceneSetup(int n)
    {
      //Parser.Parse("");
      scene = new Scene();

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

      try
      {
        switch(n)
        {
          case 1: Scene1(); break;
          case 2: Scene2(); break;
          case 3: Scene3(); break;
          case 4: Scene4(); break;
          case 5: Scene5(); break;
          case 6: Scene6(); break;
          case 7: Scene7(); break;
          case 8: Scene8(); break;
          case 9: Scene9(); break;
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
    }
    #endregion

    #region Scene1  Two Spheres
    private string b1hint = "Two spheres";
    public void Scene1()
    {
      var camera = new Camera(new Point3(1.0, 2.0, -15.0),    // Position
                                 Point3.Origin,                  // Look at
                                 Vector3.UnitY,                  // Up
                                 4.0,                            // Focal length
                                 picture.Width, picture.Height); // Aspect

      scene.Camera = camera;
      scene.Antialias = true;
      scene.Ambient = new Colour(0.2);

      var l = new Light(new Point3(-1.0, -2.0, -10.0), // Position
                          new Colour(1.0),               // Colour
                          0.3);                          // Intensity
      scene.Add(l);

            l = new Light(new Point3(1.0, 5.0, -10.0),   // Position
                          new Colour(1.0),               // Colour
                          0.3);                          // Intensity
      scene.Add(l);

      // A transparent sphere
      var glassfinish = new Finish(0.0, 0.1, 0.1, 0.3);
      var black = new Pigment(Colour.Black);
      var glass = new Material(black, glassfinish);
      glass.MakeTransparent(new Colour(0.8, 0.9, 1.0), 1.5);
      // glass.PerturbNormal(1.0, 0.1, VectorFunctions.Perlin);
      Solid s = new Sphere(new Point3(-1.4, 1.0, 0.0), 2.5, glass);
      scene.Add(s);

      // A strange textured Sphere
      var blueSmoke = new Pigment(Colour.PeachPuff, Colour.CadetBlue,
                                      RealFunctions.PerlinRidged);
      blueSmoke.Scale = 0.3;
      var f = new Finish(0.8, 0.4, 0.1, 0.0);
      var mr = new Material(blueSmoke, f);
      mr.PerturbNormal(0.1, 0.2, VectorFunctions.Perlin);
      s = new Sphere(new Point3(2.0, -1.0, -3.0), 1.5, mr);
      scene.Add(s);

      // A checkered plane
      var chex = new Pigment(new Colour(0.0), new Colour(1.0), RealFunctions.Checker);
      // Surface finish - diffuse, specular, phongsize, reflectance
      var finish = new Finish(0.6, 0.6, 0.1, 0.0);
      var mat = new Material(chex, finish);
      var p = new Plane(new Point3(0.0, 0.0, 5.001), -Vector3.UnitZ, mat);
      scene.Add(p);

      scene.Gamma = 1.2;
    }
    #endregion

    #region Scene2 - Quadric cylinders in a fish-eye lens
    private string b2hint = "Quadric cylinders in a fish-eye lens";
    public void Scene2()
    {
      var camera = new Camera(new Point3(5.0, 5.0, -5.0),
                                 new Point3(0.0, 0.0, 0.0),
                                 Vector3.UnitY,
                                 1.0,   // Short focal length
                                 picture.Width, picture.Height);
      scene.Camera = camera;
      scene.Antialias = true;

      var l = new Light(new Point3(7.0, 7.0, -7.0),    // Position
                          new Colour(1.0),              // Colour
                          0.5);                         // Intensity
      scene.Add(l);

//      l = new Light(new Point(-1.0, 5.0, 3.0),          // Position
//                    new Colour(1.0),                    // Colour
//                    0.5);                               // Intensity
//      scene.Add(l);

      Material mat;
      Finish fin;

      fin = new Finish(0.6, 0.4, 0.4, 0.5);
      var blue = new Pigment(Colour.CornflowerBlue);
      mat = new Material(blue, fin);

      Solid s1 = new CylinderX(1.0, 1.0, mat);

      var orchid = new Pigment(Colour.Orchid);
      mat = new Material(orchid, fin);

      Solid s2 = new CylinderY(1.0, 1.0, mat);

      var pink = new Pigment(Colour.DeepPink);
      mat = new Material(pink, fin);

      Solid s3 = new CylinderZ(1.0, 1.0, mat);

      scene.Add(s1 | s2 | s3);

      var orange = new Pigment(Colour.Orange);
      fin = new Finish(0.3, 0.4, 10.0, 0.5);
      mat = new Material(orange, fin);
      mat.MakeTransparent(Colour.Gold, 1.3);
      Solid s = new Sphere(new Point3(0.0, 0.0, 0.0), 2.0, mat);

      scene.Add(s);
    }
    #endregion

    #region Scene3 - Funny looking cubes
    private string b3hint = "Funny looking cubes";
    public void Scene3()
    {
      var camera = new Camera(new Point3( 8.0,  8.0, -8.0),    // Position
                                 new Point3( 0.0,  0.0,  0.0),    // Look at
                                 Vector3.UnitY,                   // Up
                                 4.0,                            // Focal length
                                 picture.Width, picture.Height); // Aspect
      scene.Camera = camera;

      var l = new Light(new Point3(-1.0, 3.0, -10.0),  // Position
                          new Colour(1.0),              // Colour
                          0.5);                         // Intensity
      scene.Add(l);

      scene.Ambient = new Colour(0.4);
      scene.Antialias = true;
      scene.Background = Colour.DeepSkyBlue;

      Material mat;
      Finish fin;
      Pigment pig;
      Solid s;
      Transform t;

      /////////////////////////////////////////////////////
      fin = new Finish(0.7, 0.3, 1.0, 0.0);
      pig = new Pigment(Colour.Blue, Colour.Red, RealFunctions.Onion);
      pig.Scale = 0.1;
      mat = new Material(pig, fin);

      // A rotation about Z
      t = new Transform(Axis.Z, 45.0);

      s = new Cube(new Point3(0.0, 0.0, 2.0), 2.0, mat);
      s.Apply(t);
      scene.Add(s);

      s = new Cube(new Point3(0.0, 0.0, -2.0), 2.0, mat);
      s.Apply(t);
      scene.Add(s);

      /////////////////////////////////////////////////////
      pig = new Pigment(Colour.Orange, Colour.Lime, RealFunctions.Perlin);
      pig.Scale = 0.1;
      mat = new Material(pig, fin);

      // A rotation about X
      t = new Transform(Axis.X, 45.0);

      s = new Cube(new Point3(2.0, 0.0, 0.0), 2.0, mat);
      s.Apply(t);
      scene.Add(s);

      s = new Cube(new Point3(-2.0, 0.0, 0.0), 2.0, mat);
      s.Apply(t);
      scene.Add(s);

      /////////////////////////////////////////////////////
      pig = new Pigment(Colour.Green);
      mat = new Material(pig, fin);
      mat.PerturbNormal(0.1, 0.3, VectorFunctions.Perlin);

      // A rotation about Y
      t = new Transform(Axis.Y, 45.0);

      s = new Cube(new Point3(0.0, 2.0, 0.0), 2.0, mat);
      s.Apply(t);
      scene.Add(s);

      s = new Cube(new Point3(0.0, -2.0, 0.0), 2.0, mat);
      s.Apply(t);
      scene.Add(s);
    }

    #endregion

    #region scene4 - The ever popular snail
    private string b4hint = "The ever popular snail";
    public void Scene4()
    {
      var camera = new Camera(new Point3(-14.0, 0.0, -5.0),   // Position
                                 new Point3(0.0, 1.0, 0.0),      // Look at
                                 Vector3.UnitY,                  // Up
                                 4.0,                            // Focal length
                                 picture.Width, picture.Height); // Aspect
      scene.Camera = camera;
      //scene.Antialias = true;
      scene.Ambient = new Colour(0.5);
      scene.Background = Colour.SkyBlue;

      var l = new Light(new Point3(-10.0, 2.0, -5.0),   // Position
                          new Colour(1.0),                // Colour
                          0.6);                           // Intensity
      scene.Add(l);

      Finish fin;
      Pigment pig;
      Material mat;
      Solid so, si;
      Transform t;
      Point3 centre;
      Solid outer;
      Solid inner;

      /////////////////////////////////////////////////////
      fin = new Finish(0.5, 0.5, 0.5, 0.0);
      pig = new Pigment(Colour.Beige, Colour.RosyBrown, RealFunctions.Perlin);
      pig.Scale = 0.1;

      mat = new Material(pig, fin);

      t = new Transform();
      centre = new Point3(0.4, 0.0, 0.0);

      outer = new Sphere(centre, 0.200);
      inner = new Sphere(centre, 0.195);
      so = outer;  // Compiler can't see init of so

      for(var k = 1; k < 100; k++)
      {
        so = new Sphere(centre, 0.200 + k / 80.0);

        t.Rotate(Axis.Z, -5.0);
        t.Scale(1.02);
        so.Apply(t);

        outer = new CSGUnion(outer, so);

        if(k > 80)
        {
          si = new Sphere(centre, 0.190 + k / 80.0);
          si.Apply(t);
          inner = new CSGUnion(inner, si);
        }
      }
      var sp = (so as Sphere);

      var nml = new Vector3(0.210, 0.133, 0.000).Normalise();
      var pt = new Point3(sp.Centre.x, sp.Centre.y, 0.0);
      si = new Plane(pt, nml);
      nml = new Vector3(nml.y, -nml.x, 0.0);
      so = new Plane(pt + (nml * sp.Radius), nml);

      si = new CSGIntersection(si, so);
      inner = new CSGUnion(inner, si);

      inner.Bound = new Sphere(centre, 1.9);

      scene.Add(new CSGDifference(outer, inner, mat));
    }
    #endregion

    #region Scene5 - Shiny ball grid
    private string b5hint = "Shiny ball grid";
    public void Scene5()
    {
      var camera = new Camera(new Point3(0.0, 0.0, -5.0),     // Position
                                 new Point3(0.0, 0.0, 0.0),      // Look at
                                 Vector3.UnitY,                  // Up
                                 4.0,                            // Focal length
                                 picture.Width, picture.Height); // Aspect
      scene.Camera = camera;

      var l = new Light(new Point3(-1.0, 5.0, -10.0),  // Position
                          new Colour(1.0),               // Colour
                          0.5);                          // Intensity
      scene.Add(l);
      scene.Antialias = true;
      scene.Gamma = 0.8;
      scene.TraceMax = 30;

      Finish fin;
      Pigment pig;
      Material mat;
      Solid s;

      /////////////////////////////////////////////////////
      fin = new Finish(0.5, 0.5, 0.5, 0.5);
      pig = new Pigment(Colour.Tomato);
      mat = new Material(pig, fin);

      for(var j = -5; j < 6; j++)
      {
        for(var k = -5; k < 6; k++)
        {
          s = new Sphere(new Point3(j, k, 0.0), 0.4, mat);
          scene.Add(s);
        }
      }

      pig = new Pigment(Colour.HotPink);
      mat = new Material(pig, fin);

      for(var j = -5; j < 6; j++)
      {
        for(var k = -5; k < 6; k++)
        {
          s = new Sphere(new Point3(j + 0.5, k + 0.5, -1.0), 0.4, mat);
          scene.Add(s);
        }
      }
    }
    #endregion

    #region Scene6 - Texture
    private string b6hint = "Texture";
    public void Scene6()
    {
      var camera = new Camera(new Point3( 0.0,  0.0, -5.0),    // Position
                                 new Point3( 0.0,  0.0,  0.0),    // Look at
                                 Vector3.UnitY,                   // Up
                                 4.0,                            // Focal length
                                 picture.Width, picture.Height); // Aspect
      scene.Camera = camera;

      var l = new Light(new Point3(-1.0, 5.0, -10.0),  // Position
                          new Colour(1.0),              // Colour
                          0.5);                         // Intensity
      scene.Add(l);
      scene.Antialias = false;


      // A wavy plane
      var wavy = new Pigment(Colour.DarkGreen, Colour.LawnGreen, Ripple);
      // Surface finish - diffuse, specular, phongsize, reflectance
      var finish = new Finish(0.6, 0.6, 0.1, 0.0);
      var mat = new Material(wavy, finish);
      var p = new Plane(new Point3(0.0, 0.0, 10.001), -Vector3.UnitZ, mat);
      scene.Add(p);

      // A transparent sphere
      var glassfinish = new Finish(0.0, 0.1, 0.1, 0.3);
      var black = new Pigment(Colour.Black);
      var glass = new Material(black, glassfinish);
      glass.MakeTransparent(Colour.White, 1.5);
      // glass.PerturbNormal(1.0, 0.1, VectorFunctions.Perlin);
      Solid s = new Sphere(new Point3(0.0, 0.0, 0.0), 1.0, glass);
      Solid b = new Sphere(new Point3(0.4, 0.2, 0.0), 0.2, glass);
      scene.Add(s - b);
    }

    double Ripple(Point3 p)
    {
      return Math.Abs(Math.Cos(RealFunctions.Perlin(p/5) * 20 * Math.PI));
    }

    #endregion

    #region Scene7 - A win for Gold
    private string b7hint = "A win for Gold";
    public void Scene7()
    {
      var camera = new Camera(new Point3(4.0, 12.0, -15.0), // Location
                                 new Point3(0.0, 0.0, 0.0),    // Look at
                                 new Vector3(0.0, 1.0, 0.0),   // Sky Vector
                                 4.0);                         // Focal length
      scene.Camera = camera;

      var l = new Light(new Point3(-1.0, 5.0, -10.0),  // Position
                          new Colour(1.0),               // Colour
                          1.0);                          // Intensity
      scene.Add(l);
      scene.Antialias = true;
      scene.Gamma = 0.8;

      var matt = new Finish(1.0, 0.0, 0.0, 0.0);
      var boardmaterial = new Material(new Colour(0.204, 0.467, 0.620), matt);
      Solid c = new Cuboid(new Point3(0.0, 0.5, 0.0), 4.0, 1.0, 4.0, boardmaterial);

      var metal = new Finish(0.3, 0.4, 50.0, 0.5);
      var gold = new Material(new Colour(0.851, 0.812, 0.067), metal);
      var silver = new Material(new Colour(0.820, 0.969, 0.988), metal);

      // Form board and pieces using CSG
      Solid s = new Sphere(new Point3(-1.4, 1.1, 1.4), 0.45, gold);
      c = c | s;
      s = new Sphere(new Point3(0.0, 1.1, 1.4), 0.45, boardmaterial);
      c = c - s;
      s = new Sphere(new Point3(1.4, 1.1, 1.4), 0.45, gold);
      c = c | s;

      s = new Sphere(new Point3(-1.4, 1.1, 0.0), 0.45, silver);
      c = c | s;
      s = new Sphere(new Point3(0.0, 1.1, 0.0), 0.45, gold);
      c = c | s;
      s = new Sphere(new Point3(1.4, 1.1, 0.0), 0.45, silver);
      c = c | s;

      s = new Sphere(new Point3(-1.4, 1.1, -1.4), 0.45, gold);
      c = c | s;
      s = new Sphere(new Point3(0.0, 1.1, -1.4), 0.45, boardmaterial);
      c = c - s;
      s = new Sphere(new Point3(1.4, 1.1, -1.4), 0.45, silver);
      c = c | s;

      scene.Add(c);

      // Unplayed pieces
      s = new Sphere(new Point3(-3.1, 0.45, -1.0), 0.45, gold);
      scene.Add(s);
      s = new Sphere(new Point3(3.0, 0.45, 1.4), 0.45, silver);
      scene.Add(s);
      s = new Sphere(new Point3(3.3, 0.45, -1.2), 0.45, silver);
      scene.Add(s);

      // The floor
      var floormaterial = new Material(new Colour(0.2, 1.0, 0.4), matt);
      scene.Add(new Plane(new Point3(0.0, 0.0, 0.0),    // Point in plane
                          new Vector3(0.0, 1.0, 0.0),   // Normal
                          floormaterial));              // Material
    }
    #endregion

    #region Scene8 - SphereFlake
    private string b8hint = "SphereFlake";
    public void Scene8()
    {
      var camera = new Camera(new Point3(5.0, 8.0, -10.0),    // Position
                                 new Point3(0.0, 0.0, 0.0),      // Look at
                                 Vector3.UnitY,                  // Up
                                 4.0,                            // Focal length
                                 picture.Width, picture.Height); // Aspect
      scene.Camera = camera;

      var l = new Light(new Point3(-1.0, 5.0, -10.0),  // Position
                          new Colour(1.0),               // Colour
                          0.5);                          // Intensity
      scene.Add(l);

      l = new Light(new Point3(10.0, -5.0, 0.0),   // Position
                    new Colour(1.0),               // Colour
                    0.5);                          // Intensity
      scene.Add(l);

      scene.Antialias = true;

      Finish fin;
      Pigment pig;
      Material mat;

      fin = new Finish(0.5, 0.5, 0.5, 0.5);
      pig = new Pigment(Colour.Tomato);
      mat = new Material(pig, fin);

      pig = new Pigment(Colour.Lime, Colour.Navy, RealFunctions.Perlin);
      pig.Scale = 0.1;
      mat = new Material(pig, fin);

      //Material flakematerial = new Material(new Colour(0.8, 0.5, 0.2), 0.4, 0.2, 30.0, 0.4);
      var a = new Aggregate(mat);
      sphereflake(a, Point3.Origin , 1.0, 0, 0.5, 4);
      // Create a top level bounding volume
      var bound = new Sphere(Point3.Origin, 2.9);
      a.Bound = bound;
      scene.Add(a);
    }

    void sphereflake(Aggregate a, Point3 p, double r, int orient, double ratio, int level)
    {
      a.Add(new Sphere(p, r));
      if(level < 1) return;
      var dist = (1.0 + ratio) * r;
      var rr = r * ratio;
      var X = new Vector3(dist, 0.0, 0.0);
      var Y = new Vector3(0.0, dist, 0.0);
      var Z = new Vector3(0.0, 0.0, dist);

      if(orient != 1) sphereflake(a, p + X, rr, 2, ratio, level - 1);
      if(orient != 2) sphereflake(a, p - X, rr, 1, ratio, level - 1);
      if(orient != 3) sphereflake(a, p + Y, rr, 4, ratio, level - 1);
      if(orient != 4) sphereflake(a, p - Y, rr, 3, ratio, level - 1);
      if(orient != 5) sphereflake(a, p + Z, rr, 6, ratio, level - 1);
      if(orient != 6) sphereflake(a, p - Z, rr, 5, ratio, level - 1);
    }

    #endregion

    #region Scene9 - widget
    private string b9hint = "Widget";
    public void Scene9()
    {
      var camera = new Camera(new Point3(12.0, -6.0, -20.0),
                                 new Point3(12.0, -6.0, 0.0),
                                 Vector3.UnitY,
                                 1.0,   // Short focal length
                                 picture.Width, picture.Height);
      scene.Camera = camera;
      scene.Antialias = true;

      var l = new Light(new Point3(7.0, 7.0, -7.0),    // Position
                          new Colour(1.0),               // Colour
                          1.0);                          // Intensity
      scene.Add(l);

      var orange = new Pigment(Colour.Orange);
      var fin = new Finish(0.3, 0.4, 10.0, 0.5);
      var mat = new Material(orange, fin);

      var path = new GraphicsPath();
      var ff = new FontFamily("Celtic Knots");
      path.AddString("QWE\r\nASD\r\nZXC", ff, 0, 10f, new Point(0, 0), StringFormat.GenericTypographic);
      path.Flatten();
      var pts = path.PathData.Points;
      var cmds = path.PathData.Types;

      var closeTo = pts[0];
      for(var i = 1; i < pts.Length + 1; i++)
      {
        Solid c;
        if(cmds[i - 1] > 127)
        {
          c = Cylinder(new Point3((double)closeTo.X, 8 - (double)closeTo.Y, 0.0), new Point3((double)pts[i - 1].X, 8 - (double)pts[i - 1].Y, 0.0), 0.3);
          if(i < pts.Length)
            closeTo = pts[i];
        }
        else
        {
          c = Cylinder(new Point3((double)pts[i].X, 8 - (double)pts[i].Y, 0.0), new Point3((double)pts[i - 1].X, 8 - (double)pts[i - 1].Y, 0.0), 0.3);
        }
        if(c != null)
        {
          c.Material = mat;
          scene.Add(c);
        }
        if(i < pts.Length)
          scene.Add(new Sphere(new Point3((double)pts[i].X, 8 - (double)pts[i].Y, 0.0), 0.3, mat));
      }
    }

    private Solid Cylinder(Point3 a, Point3 b, double diameter)
    {
      Solid c = new CylinderX(diameter, diameter);
      var t = new Transform();
      var along = b - a;
      if(along.Norm() < 0.001)
        return null;
      var angle = Math.Atan2(b.y - a.y, b.x - a.x) * 180.0 / Math.PI;
      t.Rotate(Axis.Z, -angle);
      t.Translate(a);
      c.Apply(t);
      var p1 = new Plane(a, -along);
      var p2 = new Plane(b, along);

      c = c & p1;
      c = c & p2;
      return c;
    }
    #endregion

    #region  Scan the image and render

    /// <summary>
    /// The user pressed one of the render buttons
    /// </summary>
    /// <param name="sender">The button</param>
    /// <param name="e">EventArgs</param>
    private void buttGo_Click(object sender, System.EventArgs e)
    {
      // Get the buttons tag number
      var s = (sender as Button).Tag as string;
      var select = int.Parse(s);

      ButtonsOn(false);

      // Create the scene
      SceneSetup(select);

      // Use all the available space
      var w = picture.Width;
      var h = picture.Height;

      // Output backing store
      bm = new Bitmap(w, h);

      var xscale = 2.0 / w;
      var yscale = 2.0 / h;

      // A thin sliding rectangle that Invalidates the most recently drawn line of output
      var lineRect = new Rectangle(0, 0, 1, h);

      // If Antialias is three-stated then use (and display) the default setting for the scene
      // Otherwise override scene settings
      if(checkBox1.CheckState == CheckState.Indeterminate)
        checkBox1.CheckState = scene.Antialias ? CheckState.Checked : CheckState.Unchecked;
      else
        scene.Antialias = checkBox1.Checked;

      unsafe
      {

        // Lets see how quick we are...
        var clock = new Stopwatch();
        clock.Reset();
        clock.Start();

        try
        {
          
          for(var x = 0; x < w; x++)
          {
            {
              var bdata = bm.LockBits(lineRect, ImageLockMode.WriteOnly, PixelFormat.Format32bppRgb);
              var pixptr = (int*)bdata.Scan0;
              pixptr += h;
              Parallel.For(0, h, y =>
              {
                Colour pixel;

                // Simple 5-point (domino) average antialiasing
                if(scene.Antialias)
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

                if(abort)
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
        label1.Text = string.Format("Last render: {0} S", clock.ElapsedMilliseconds * 0.001);
        label2.Text = string.Format("{0} Object rays", scene.Rays);
        label3.Text = string.Format("{0} Shadow rays", scene.ShadowRays);

        ButtonsOn(true);
        buttSave.Enabled = true; // The save button
      }
    }
    #endregion

    #region UI stuff

    private void ButtonsOn(bool b)
    {
      button1.Enabled = b;
      button2.Enabled = b;
      button3.Enabled = b;
      button4.Enabled = b;
      button5.Enabled = b;
      button6.Enabled = b;
      button7.Enabled = b;
      button8.Enabled = b;
      button9.Enabled = b;
    }

    // Update screen at end of line
    private void picture_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
    {
      if(bm != null)
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

    private void checkBox1_CheckedChanged(object sender, EventArgs e)
    {
      if((scene == null) || (checkBox1.CheckState == CheckState.Indeterminate))
        return;
      scene.Antialias = checkBox1.Checked;
    }


    private void SaveImage(object sender, EventArgs e)
    {
      var dr = saveFileDialog1.ShowDialog();
      if(dr != DialogResult.OK)
        return;

      bm.Save(saveFileDialog1.FileName);
      buttSave.Enabled = false;
    }
  }
  #endregion
}
