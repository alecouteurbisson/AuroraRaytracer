using System;
using System.Collections.Generic;
using System.Threading;

namespace Aurora
{
    #region Camera
    // A camera with variable aspect ratio and focal length
    public class Camera 
  {
    Point3   location;
    Vector3  los;                // line of sight
    Vector3  vx, vy;             // View x, y
    Vector3  vxsave;
    double   d;
    bool     aspectSet;

    public Camera()
    {
      d = 0.0;      // d = 0 indicates an uninitialised camera
    }

    public Camera(Point3 at, Point3 lookat, Vector3 up, double flength, double arx = 0.0, double ary = 0.0)
    {
      if(flength <= Constant.Epsilon)
        throw new AuroraException("Camera() Camera focal length must be > 0");

      location = at;
      los = (lookat - at).Normalise();          // Get line of sight
      vx = (up ^ los).Normalise();              // Get view x direction (note: ^ is cross product)
      vy = los ^ vx;                            // Get view y direction 
      vxsave = vx;                              // Save to allow changes in aspect ratio
      aspectSet = ary != 0.0;
      if(aspectSet)
        vx = vx * (arx / ary);                  // Set aspect ratio
      d = flength;                              // Set focal length
    }

      public Camera(Point3 at, Point3 lookat, Vector3 up, double flength, ImageSize size)
      : this(at, lookat, up, flength, size.Width, size.Height)
    { }

    /// <summary>
    /// If the aspect ratio has not been explicitly set then get the
    /// value from the scene
    /// </summary>
    /// <param name="size">Scene size</param>
    public void SetImageSize(ImageSize size)
    {
      if(!aspectSet)
        vx = vxsave * (size.Width / (double)size.Height);
    }

    /// <summary>
    /// Generate a ray corresponding to a single "pixel" of the camera
    /// </summary>
    /// <param name="x">Pixel X coordinate</param>
    /// <param name="y">Pixel Y coordinate</param>
    /// <returns>The ray</returns>
    public Ray GetRay(double x, double y)
    {
      if(d == 0.0)
        throw new AuroraException("Camera::GetRay() Camera not initialised");
      var direction = (los * d + vx * x + vy * y).Normalise();
      return  new Ray(location, direction);
    }
  }
  #endregion

  #region Scene
  // A container for all of the parts of the scene
  // camera, lights and objects
  public class Scene
  {
    List<Model> models;          // All physical objects...
    List<Light> lights;          // ...all scene lighting...
    Camera      camera;          // ...and one camera
    ImageSize   size;            // Image size
    bool        shadow;          // Trace shadows?
    bool        antialias;       // Basic 5 point antialiasing
    Colour      ambient;         // Ambient light colour
    Colour      background;      // Cosmic background radiation
    double      gamma;           // Gamma correction 
    int         traceMax;        // Maximum trace depth
    double      minWeight;       // Minimum reflected ray contribution
    double      rayOffset;       // Shadow ray origin standoff
    int         shadowRays;      // Shadowray counter
    int         cacheHits;       // Shadow cache hit counter
    int         rays;            // Total number of rays traced

    public Scene()
    {
      size = new ImageSize(1024, 768);
      models = new List<Model>();
      lights = new List<Light>();
      shadow = true;
      antialias = true;
      ambient = new Colour(0.1, 0.1, 0.1);
      background = new Colour(0.0, 0.0, 0.0);
      traceMax = 5;
      minWeight = 0.01;
      rayOffset = 0.0001;
      shadowRays = 0;
      cacheHits = 0;
      rays = 0;
      gamma = 1.0;
    }

    // Add a Model to the scene
    public void Add(Model m)
    {
      models.Add(m);
    }

    // Add a light to the scene
    public void Add(Light l)
    {
      lights.Add(l);
    }

    // Image size
    public ImageSize Size
    {
      set { size = value; }
      get { return size; }
    }

    // The camera
    public Camera Camera
    {
      set { camera = value; }
      get { return camera; }
    }

    // Shadow flag (true = calculate shadows)
    public bool Shadow
    {
      set { shadow = value; }
      get { return shadow; }
    }

    // Antialias flag
    public bool Antialias
    {
      set { antialias = value; }
      get { return antialias; }
    }

    // Ambient light colour
    public Colour Ambient
    {
      set { ambient = value; }
      get { return ambient; }
    }

    // Cosmic background radiation
    public Colour Background
    {
      set { background = value; }
      get { return background; }
    }

    // Set the maximum trace level (reflection/refraction ray depth)
    public int TraceMax
    {
      set { traceMax = value; }
      get { return traceMax; }
    }

    // Set minimum weight before abandoning reflection/refraction rays
    public double MinWeight
    {
      set { minWeight = value; }
      get { return minWeight; }
    }

    // Set the ray offset
    // Set to a very small (invisible) distance in the view so that surfaces
    // do not self-shadow or self-obstruct.
    public double RayOffset
    {
      set { rayOffset = value; }
      get { return rayOffset; }
    }

    // Approximate? gamma correction of displayed images
    public double Gamma
    {
      set { gamma = value; }
      get { return gamma; }
    }

    public int ShadowRays { get { return shadowRays; } }
    public int CacheHits { get { return cacheHits; } }
    public int Rays { get { return rays; } }
    
    // Trace a single scene pixel
    public Colour Trace(double vx, double vy)
    {
      var r = camera.GetRay(vx, vy);

      var hit = Intersect(r);

      if(hit.Model == null)
        return background;

      //if(!hit.Entering && !hit.Medium.Transparent)
      //  throw new AuroraException("Scene.Trace:  Camera inside opaque solid");

      return Shade(hit, r);
    }

    // Obtain the colour of a ray/model intersection
    public Colour Shade(Intersection hit, Ray r, int level, double weight)
    {
      if (level > traceMax)
        return background;     // Too deep

      // Get the surface normal
      var normal = hit.Model.Normal(hit.Location);

            // If looking at the inside of a model (e.g. rhs of CSGDifference; or transparent object)
            // then the normal will be inverted
            if ((normal * r.Direction) > 0.0)
                normal = -normal;

      var colour = new Colour();

      // Point p, where shadow ray originates, is elevated from the model
      // surface to prevent erroneous self-shadowing.
      var p = hit.Location + normal * rayOffset;
      var filter = new Colour(1.0);

      foreach(var l in lights)
      {
        // If shadowing, determine if l is blocked
        // If only blocked by transparent objects then return the
        // overall filter colour in filter;
        if(!(shadow && Shadowed(p, l, out filter)))
          colour += (filter * (l.Illumination(hit, normal)));
      }

      var materialHit = hit.Model.Material;
      if(materialHit == null)
        materialHit = Material.NullMaterial;

      // Add ambient light
      colour += ambient * materialHit.GetPigment(hit.Location);

      var reflectance = materialHit.Reflectance;

      // Do reflections, but only if they will make a visible contribution
      if((reflectance != 0.0) &&
         (reflectance * weight > minWeight) &&
         (level <= traceMax))
      {
        // launch reflected ray from rayOffset above the surface
        var rray = new Ray(hit.Location + normal * rayOffset,
                            (normal * (-2.0 * (r.Direction * normal)) +
                            r.Direction));

        var rhit = Intersect(rray);
        if(rhit.Model != null)
        {
          if(rhit.Model.Material == null)
            throw new AuroraException("Scene.Trace  Model has null material");

          colour += Shade(rhit, rray, level + 1, reflectance * weight) *
                    reflectance;
        }
        // Reflected into space
        else colour += background * reflectance;
      }

      if(materialHit.Transparent)
      {
        // The transmitted (or internally reflected) ray
        Ray tray;
        // The medium for tray
        var medium = materialHit; 

        if(materialHit.Ior != hit.Medium.Ior) // Refracted?
        {
          var eta = hit.Medium.Ior / materialHit.Ior;
          var ci = normal * -r.Direction;
          var disc = 1.0 + eta * eta * (ci * ci - 1.0);

          // disc discriminates between refraction and tir
          if(disc < 0.0)
          {
            // TODO: Why did this originally just return White?
            // Total internal reflection
            // launch transmitted ray from rayOffset above the surface
            tray = new Ray(hit.Location + normal * rayOffset, (normal * (-2.0 * (r.Direction * normal)) + r.Direction));
            medium = hit.Medium;
            // return new Colour(1.0, 1.0, 0.0);
          }
          else
          {
            // Refraction
            // launch transmitted ray from rayOffset beyond the surface
            var tdir = (eta * r.Direction + (eta * ci - Math.Sqrt(disc)) * normal).Normalise();
            tray = new Ray(hit.Location + tdir * rayOffset, tdir);
          }
        }
        else
        {
          // Simple transmission (identical refractive indices)
          tray = new Ray(hit.Location + r.Direction * rayOffset, r.Direction);
        }
        var thit = Intersect(tray);
        if(thit.Model != null)
        {
          var w = medium.Filter.Lightness() * weight;
          colour += Shade(thit, tray, level + 1, w) * medium.Filter;
        }
        else
        {
          colour += background * medium.Filter;
        }
      }
      return colour;
    }

    // This simplified version is used for camera rays
    public Colour Shade(Intersection hit, Ray r)
    {
      return Shade(hit, r, 1, 1.0);
    }
    
    // Intersect a ray with the scene and get an Intersection
    public Intersection Intersect(Ray r)
    {
      rays++;

      var hit = new Intersection();

      foreach(var  m in models)
      {
        // Check bounding volume
        if((m.Bound != null) && (m.Bound.Intersect(r).Distance > Constant.Huge))
          continue;
        
        // Ignore all but first (nearest) intersection
        var p = m.Intersect(r);
        if(p < hit) hit = p;
      }
      return hit;
    }

    // Does this light shine on p
    // and if so, is it filtered
    public bool Shadowed(Point3 p, Light l, out Colour tint)
    {
      tint = new Colour(1.0);
      // Every light caches the model that last obscured it because that
      // model is likely to shadow the next intersection
      // (Shadowing object coherency)
      Interlocked.Increment(ref shadowRays);
      var beam = new Ray(p, l.Location);

      // Test models for shadowing
      foreach(var m in models)
      {
        if(m.Intersect(beam).Distance < beam.Length)
        {
          if(!m.Material.Transparent)
          {
            return true;       // One model is as good as a hundred
          }
          // Multiply filtering objects colours
          tint *= m.Material.Filter;
        }
      }
      // Leave the cached object since it is still the best guess
      return false;
    }
  }
  #endregion

  #region SceneBuilder stuff

  public struct ImageSize
  {
    public int Width;
    public int Height;

    public ImageSize(int width, int height)
    {
      Width = width;
      Height = height;
    }
  }

  public interface ISceneBuilder
  {
    Scene Build();
  }

  #endregion

  #region AuroraException
  // Exception class
  [Serializable]
  public class AuroraException : ApplicationException
  {
    public AuroraException(string msg, params object[] args)
      :  base(string.Format(msg, args))
    {}
  }
  #endregion
}
