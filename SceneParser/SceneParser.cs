using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Aurora;
using System.Diagnostics;
using System.Reflection;
using System.Drawing;

namespace Aurora.SceneParser
{
  public class Parser
  {
    static XElement xscene;
    static Scene scene;

    static Dictionary<string, Point3> pointref;
    static Dictionary<string, Vector3> vectorref;
    static Dictionary<string, Colour> colourref;
    static Dictionary<string, Finish> finishref;
    static Dictionary<string, Material> materialref;
    static Dictionary<string, Transform> transformref;
    static Dictionary<string, Model> modelref;

    public static Scene Parse(string path)
    {
      xscene = XElement.Load(path);
      scene = new Scene();

      InitRefs();

      ReadScene();

      return scene;
    }

    private static void InitRefs()
    {
      pointref = new Dictionary<string, Point3>();         // Points
      pointref.Add("O", new Point3());                     // preset origin 

      vectorref = new Dictionary<string, Vector3>();       // Vectors
      vectorref.Add("X", new Vector3(1.0, 0.0, 0.0));      // preset X axis 
      vectorref.Add("Y", new Vector3(1.0, 0.0, 0.0));      // preset Y axis 
      vectorref.Add("Z", new Vector3(1.0, 0.0, 0.0));      // preset Z axis 

      transformref = new Dictionary<string, Transform>();  // Transformations

      colourref = new Dictionary<string, Colour>();        // Colours
      finishref = new Dictionary<string, Finish>();        // Finishes
      materialref = new Dictionary<string, Material>();    // Materials

      modelref = new Dictionary<string, Model>();          // Models
    }

    /// <summary>
    /// A scene contains a camera, and lights, materials and models sections
    /// </summary>
    private static void ReadScene()
    {
      // Read scene globals and apply defaults
      scene.Antialias = (bool?)xscene.Attribute("antialias") ?? false;
      scene.Shadow = (bool?)xscene.Attribute("shadows") ?? false;
      scene.TraceMax = (int?)xscene.Attribute("tracemax") ?? 20;
      scene.MinWeight = (double?)xscene.Attribute("minweight") ?? 0.004;
      scene.Gamma = (double?)xscene.Attribute("gamma") ?? 1.2;
      scene.Ambient = Colour.Black;
      scene.Background = Colour.Black;

      foreach(XElement scenepart in xscene.Elements())
      {
        switch(scenepart.Name.LocalName)
        {
          case "geometry":
            ReadGeometry(scenepart);
            break;

          case "materials":
            ReadMaterials(scenepart);
            break;

          case "camera":
            ReadCamera(scenepart);
            break;

          case "lights":
            ReadLights(scenepart);
            break;

          case "models":
            ReadModels(scenepart);
            break;

          default:
            Debug.WriteLine("Ignoring scene/" + scenepart.Name.LocalName);
            break;
        }
      }
    }

    private static void ReadGeometry(XElement xgeometry)
    {
      foreach(XElement elem in xgeometry.Elements())
      {
        if(elem.Attribute("id") != null)
          throw new ApplicationException("Geometry elements must have an id");

        switch(elem.Name.ToString())
        {
          case "point":
            ReadPoint(elem);
            break;

          case "vector":
            ReadVector(elem);
            break;

          case "transform":
            ReadTransform(elem);
            break;

          default:
            throw new ApplicationException("Unknown geometry element: " + elem.Name);
        }
      }
    }

    // Read a camera 
    private static void ReadCamera(XElement xcamera)
    {
      Point3 position = ReadPoint(xcamera, "position", "Camera must be positioned");
      Point3 lookat = ReadPoint(xcamera, "lookat", Point3.Origin);
      Vector3 up = ReadVector(xcamera, "up", Vector3.UnitY);
      double flength = (double?)xcamera.Element("focal_length") ?? 1.0;

      scene.Camera = new Camera(position, lookat, up, flength);
    }

    // Read ambient and point source lights
    private static void ReadLights(XElement xlights)
    {
      foreach(XElement xlight in xlights.Elements())
      {
        Colour lc = Colour.White;
        double li = 1.0;

        XElement xcol = xlight.Element("colour");
        if(xcol != null)
          lc = ReadColour(xcol);

        li = (double?)xlight.Element("intensity") ?? 1.0;

        switch(xlight.Name.LocalName)
        {
          // Set ambient light colour and intensity
          case "ambient":
            scene.Ambient = lc * li;
            break;

          case "light":
            // Create a new point source light
            Point3 point = ReadPoint(xlight, "position", "Light must be positioned");
            scene.Add(new Light(point, lc, li)); 
            break;

          default:
            Debug.WriteLine("Ignoring light/" + xlight.Name.LocalName);
            break;
        }
      }
    }

    private static void ReadMaterials(XElement xmaterials)
    {
      throw new NotImplementedException();
    }

    private static Model ReadModels(XElement xmodels)
    {
      foreach(XElement xmodel in xmodels.Elements())
      {
        switch(xmodel.Name.ToString())
        {
          case "aggregate":
            return null; // ReadAggregate(xmodel);

          case "triangle":
            return null; // ReadTriangle(xmodel);

          default:
            return ReadSolid(xmodel);
        }
      }
      return null;
    }

    private static Solid ReadSolid(XElement xmodel)
    {
      switch(xmodel.Name.ToString())
      {
        case "sphere":
          return null; // ReadSphere(xmodel);

        default:
          return null;
      }
    }

    //////////////////////////////////////////////////////////////////////
    // Reading Points and Vectors
    //
    // To enter numerical values just put the coordinates in the 
    // appropriate field 
    //
    // e.g. 
    //  <light>
    //    <position> 7.0 7.0 -7.0 </position>
    //    <colour ref="White" />
    //    <intensity> 0.5 </intensity>
    //  </light>
    //
    // Calling ReadPoint with xe at the 'light' node and 'position'
    // in name will read the coordinates and assume that they represent
    // a point
    //
    // To create a point with an id it is neccessary to use an explicit
    // point node Such definitions are usually confined to the top level
    // of the geometry section but this is not enforced. 
    // e.g.
    // <geometry>
    //   <point id="base"> 5.0 12.4 3.2 </point>
    // </geometry>
    //
    // A point node is also required to reference this id
    // e.g.
    // <transform>
    //   <translate> <point ref="base" /> </translate>
    // </transform>
    //
    // Finally, here is an example using an explicit point node 
    // and defining an id at the point of use.  
    // <light>
    //   <position> 
    //     <point id="lightpos"> 7.0 7.0 -7.0 </point>
    //   </position>
    //   <colour ref="White" />
    //   <intensity> 0.5 </intensity>
    // </light>
    //
    // To use a default value omit the defaulted node altogether
    // e.g. this camera definition allows 'up' and 'lookat' to
    // assume their default values of the Y axis and the origin
    // respectively
    //
    // <camera>
    //   <position> 1.0 2.0 -15.0 </position>
    //   <focal_length> 4.0 </focal_length>
    // </camera>
    //
    // It is an error to provide an empty defininition, 
    // e.g. <up /> 
    // as this overrides the default without providing an alternative
    //

    // Read an XML Point returning the default point if no point found
    private static Point3 ReadPoint(XElement xe, string name, Point3 def)
    {
      return ReadPoint(xe, name, null, def);
    }

    // Read an XML Point throwing an exception with exmessage if not found
    private static Point3 ReadPoint(XElement xe, string name, string exmessage)
    {
      return ReadPoint(xe, name, exmessage, Point3.Origin);
    }

    // Read an explicit XML Point
    private static Point3 ReadPoint(XElement xe)
    {
      return ReadPoint(xe, null, "Failed to read point", Point3.Origin);
    }

    // Actually read a point
    private static Point3 ReadPoint(XElement xe, string name, string exmessage, Point3 def)
    {
      Point3 point = def;
      XElement xpoint;

      // If name is not given then xe is the point
      if(String.IsNullOrEmpty(name))
      {
        xpoint = xe;
      }
      else 
      {
        // Get the named point element
        xpoint = xe.Element(name);
        // If this element contains an explicit point then get it
        xpoint = xpoint.Element("point") ?? xpoint;
      }

      if(xpoint == null)
      {
        if(exmessage == null)
          return point;
        
        throw new ApplicationException(exmessage);
      }

      // True for explicit point nodes
      bool exp = xpoint.Name == "point";

      // Explicit point nodes can have a reference
      if(exp && GetReference(xpoint, pointref, ref point))
        return point;

      try
      {
        double[] c = ReadCoordinates(xpoint);
        point = new Point3(c[0], c[1], c[2]);      
      }
      catch(Exception ex)
      {
        throw new ApplicationException(exmessage, ex);
      }
      
      if(exp)
        // Store if id given in a explicit point node
        SetReference(xpoint, point, pointref);

      return point;
    }

    // Read an XML Vector returning the default vector if none found
    private static Vector3 ReadVector(XElement xe, string name, Vector3 def)
    {
      return ReadVector(xe, name, null, def);
    }

    // Read an XML vector throwing an exception with exmessage if none found
    private static Vector3 ReadVector(XElement xe, string name, string exmessage)
    {
      return ReadVector(xe, name, exmessage, Vector3.Zero);
    }

    // Read an explicit XML Vector throwing an exception with exmessage if not found
    private static Vector3 ReadVector(XElement xe)
    {
      return ReadVector(xe, null, "Failed to read vector", Vector3.Zero);
    }

    // Actually read a vector
    private static Vector3 ReadVector(XElement xe, string name, string exmessage, Vector3 def)
    {
      Vector3 vector = def;
      XElement xvector;

      // If name is not given then xe is the point
      if(String.IsNullOrEmpty(name))
      {
        xvector = xe;
      }
      else
      {
        // Get the named point element
        xvector = xe.Element(name);
        // If this element contains an explicit point then get it
        xvector = xvector.Element("vector") ?? xvector;
      }

      if(xvector == null)
      {
        if(exmessage == null)
          return vector;

        throw new ApplicationException(exmessage);
      }

      // True for explicit point nodes
      bool exp = xvector.Name == "vector";

      // Explicit point nodes can have a reference
      if(exp && GetReference(xvector, vectorref, ref vector))
        return vector;

      try
      {
        double[] c = ReadCoordinates(xvector);
        vector = new Vector3(c[0], c[1], c[2]);
      }
      catch(Exception ex)
      {
        throw new ApplicationException(exmessage, ex);
      }

      if(exp)
        SetReference(xvector, vector, vectorref);

      return vector;
    }

    public static Transform ReadTransform(XElement xe)
    {
      Transform t = new Transform();

      // Check for reference
      if(GetReference(xe, transformref, ref t))
        return t;

      Transform telem;
      try
      {
        foreach(XElement tr in xe.Elements())
        {
          switch(tr.Name.ToString())
          {
            case "translate":
              telem = ReadTranslation(tr);
              break;

            case "rotate":
              telem = ReadRotation(tr);
              break;

            case "scale":
              telem = ReadScaling(tr);
              break;

            case "transform":
              telem = ReadTransform(tr);
              break;

            default:
              throw new ApplicationException("Unknown transform component: " + tr.Name);
          }
          t.Apply(telem);
        }
        SetReference(xe, t, transformref);

        return t;
      }
      catch(Exception ex)
      {
        throw new ApplicationException("Error reading transform", ex);
      }
    }

    public static Transform ReadTranslation(XElement xe)
    {
      return new Transform(ReadVector(xe));
    }

    public static Transform ReadRotation(XElement xe)
    {
      Axis a;
      switch(xe.Attribute("axis").Value)
      {
        case "X": a = Axis.X; break;
        case "Y": a = Axis.Y; break;
        case "Z": a = Axis.Y; break;
        default: throw new ApplicationException("Unknown rotation axis in transformation");
      }

      double angle = (double)xe;
      return new Transform(a, angle);
    }

    public static Transform ReadScaling(XElement xe)
    {
      Vector3 scale;
      try
      {
        scale = ReadVector(xe);
        return new Transform(scale.x, scale.y, scale.z);
      }
      catch(Exception ex)
      {
        double uscale;
        if(double.TryParse(xe.Value, out uscale))
          return new Transform(uscale);
        throw new ApplicationException("Bad scaling parameters in transformation", ex);
      }
    }

    // Read an XML Colour
    public static Colour ReadColour(XElement xcolour)
    {
      Colour colour = Colour.Black;

      // Check for reference
      if(GetReference(xcolour, colourref, ref colour))
        return colour;

      // Check for stock colour
      FieldInfo fi = typeof(Colour).GetField(xcolour.Value, BindingFlags.Public | BindingFlags.Static); 
      if(fi != null && fi.FieldType.Name == "Colour")
        return (Colour)fi.GetValue(null);

      // Read RGB coordinates
      double[] c = ReadCoordinates(xcolour);
      colour = new Colour(c[0], c[1], c[2]);

      // Store if id given
      SetReference(xcolour, colour, colourref);

      return colour;
    }

    // Read three coordinate values
    private static double[] ReadCoordinates(XElement e)
    {
      string[] v = e.Value.Split(" \t,;".ToCharArray());
      double[] coords = new double[3];
      if(v.Length != 3 || 
        !double.TryParse(v[0], out coords[0]) ||
        !double.TryParse(v[1], out coords[1]) ||
        !double.TryParse(v[2], out coords[2]) )
        throw new ApplicationException("Unreadable Coordinates: " + e.Value);

      return coords;
    }

    private static bool GetReference<T>(XElement ex, Dictionary<string, T> dict, ref T item)
    {
      XAttribute r = ex.Attribute("ref");
      if(r != null)
      {
        if(dict.TryGetValue(r.Value, out item))
        {
          if(ex.HasElements)
            throw new ApplicationException("Element reference cannot have definition");
          return true;
        }
        throw new ApplicationException("Element reference not found: " + r.Value);
      }
      return false;
    }

    private static void SetReference<T>(XElement ex, T item, Dictionary<string, T> dict)
    {
      XAttribute id = ex.Attribute("id");
      if(id != null && !String.IsNullOrEmpty(id.Value))
        dict.Add(id.Value, item);
    }
  }
}
