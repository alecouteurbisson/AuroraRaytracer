using System;
using System.Collections.Generic;

namespace Aurora
{
  #region Model
  /// <summary>
  /// The base class of all visible objects
  /// </summary>
  public abstract class Model
  {
    // Optional bounding volume
    protected Model     bound;

    // The relevant visible properties of this object
    protected Material  material;

    /// <summary>
    /// Construct and set the material
    /// </summary>
    /// <param name="m">The material properties</param>
    protected Model(Material m)
    {
      bound = null;
      material = m;
    }

    /// <summary>
    /// A protected default constructor
    /// </summary>
    protected Model() : this(null)
    {}

    /// <summary>
    /// Determine if this model intercepts a ray and provide
    /// a detailed account if it does.  The nearest intersection
    /// to the ray origin is returned
    /// </summary>
    /// <param name="r">An arbitrary ray</param>
    /// <returns>The intersection</returns>
    public abstract Intersection Intersect(Ray r);

    /// <summary>
    /// Returns the surface normal at point p with any normal perturbation
    /// included.  This method will return nonsensical results for points
    /// off the surface.
    /// </summary>
    /// <param name="p">A point on the surface</param>
    /// <returns>The normal at that point</returns>
    public virtual Vector3 Normal(Point3 p)
    {
      var n = ShapeNormal(p);
      if((material != null) && material.Perturbed)
      {
        n += material.NormalPerturbation(p);
        n = n.Normalise();
      }
      return n;
    }

    /// <summary>
    /// Surface normal at point p due to the shape alone and excluding
    /// any normal perturbation.This method will return nonsensical
    /// results for points off the surface.
    /// </summary>
    /// <param name="p">A point on the surface</param>
    /// <returns>The normal at that point</returns>
    public virtual Vector3 ShapeNormal(Point3 p)
    {
      // Non primitive objects leave the normal calculation
      // to their primitive components
      throw new AuroraException("Model.Normal() unimplemented - Non primitive object");
    }

    /// <summary>
    /// Apply a transformation to this object
    /// </summary>
    /// <param name="t">The transformation to be applied</param>
    public abstract void Apply(Transform t);
   
    
    /// <summary>
    /// Test if a point lies inside this model
    /// </summary>
    /// <param name="p">Point to test</param>
    /// <returns>True if the point is inside the model</returns>
    public abstract bool Inside(Point3 p);

    /// <summary>
    /// A bounding object for complex shapes ( CSGs and Aggregates )
    /// </summary>
    public Model Bound
    {
      set { bound = value; }
      get { return bound; }
    }

    /// <summary>
    /// The material that this model is made of
    /// </summary>
    public virtual Material Material
    {
      set { material = value; }
      get { return material; }
    }
  }
  #endregion

  #region Aggregate
  /// <summary>
  /// A collection of objects transformed together
  /// </summary>
  public class Aggregate : Model
  {
    private List<Model> models;

    public Aggregate(Material m) : base(m)
    {
      models = new List<Model>();
    }

    public Aggregate() : this(null)
    {}


    public override Intersection Intersect(Ray r)
    {
      Intersection i = null;

      foreach(var m in models)
      {
        var mi = m.Intersect(r);
        if((i == null) || (mi < i))
          i = mi;
      }
      return i;
    }

    public override Material Material
    {
      set
      {
        material = value;
        // Propogate material to children
        if(value != null)
        {
          foreach(var m in models)
          {
            if(m.Material == null)
              m.Material = value;
          }
        }
      }
      get { return material; }
    }

    // Currently used only in CSG calculations.  Easily implemented though
    // so here it is
    public override bool Inside(Point3 p)
    {
      foreach(var m in models)
        if(m.Inside(p))
          return true;

      return false;
    }

    // Transform all children
    public override void Apply(Transform t)
    {
      foreach(var m in models)
        m.Apply(t);
    }

    // Adding models to an Aggregate
    public void Add(Model m)
    {
      models.Add(m);
      // Propogate material
      if(m.Material == null)
        m.Material = Material;
    }
  }
  #endregion

  #region Triangle
  /// <summary>
  /// A single sided triangular sheet. Not a solid (it has zero volume)
  /// and it's invisible when you look from the back.  Only really useful
  /// with automated mesh generating code.
  /// </summary>
  sealed class Triangle : Model
  {
    // The three corners, the plane that they lie in and the dominant axis
    private Point3 va;
    private Point3 vb;
    private Point3 vc;
    private readonly Plane  plane;
    private Axis   dominant;

    /// <summary>
    /// Create a triangle between 3 points with the specified surface material
    /// (Bulk properties like refractive index do not apply)
    /// The corners must be given in an anti-clockwise order when seen from the
    /// visible side.
    /// </summary>
    /// <param name="va">first corner</param>
    /// <param name="vb">second corner</param>
    /// <param name="vc">third corner</param>
    /// <param name="m">material</param>
    public Triangle(Point3 va, Point3 vb, Point3 vc, Material m = null)
    {
      // Locate the plane that we lie in
      plane = new Plane(va, (vb - va) ^ (vc - vb), m);

      this.va = va;
      this.vb = vb;
      this.vc = vc;

      // Determine dominant axis
      DominantAxis();

      Material = m;
    }

    /// <summary>
    /// Determine if we intersect a ray
    /// </summary>
    /// <param name="r">A ray</param>
    /// <returns>An intersection which may be empty</returns>
    public override Intersection Intersect(Ray r)
    {
      // Trivial rejection on wrong side of triangle
      // This form of test ensures that back-to-back triangles dont wink out edge on
      if(r.Direction * plane.Normal() > Constant.Epsilon)
        return new Intersection();

      // Intersect with the plane first
      var i = plane.Intersect(r);

      // No luck! return a null intersection
      if(i.Model == null)
        return new Intersection();

      i.Model = this;

      // Unfortunately the compiler cannot see that these are
      // always initialised
      double u0 = 0.0, u1 = 0.0, u2 = 0.0;
      double v0 = 0.0, v1 = 0.0, v2 = 0.0;

      // Determine if the intersection lies inside the triangle
      switch(dominant)
      {
        case Axis.X: u0 = i.Location.y - va.y;
                     v0 = i.Location.z - va.z;
                     u1 = vb.y - va.y;
                     v1 = vb.z - va.z;
                     u2 = vc.y - va.y;
                     v2 = vc.z - va.z;
        break;
        case Axis.Y: u0 = i.Location.x - va.x;
                     v0 = i.Location.z - va.z;
                     u1 = vb.x - va.x;
                     v1 = vb.z - va.z;
                     u2 = vc.x - va.x;
                     v2 = vc.z - va.z;
        break;
        case Axis.Z: u0 = i.Location.x - va.x;
                     v0 = i.Location.y - va.y;
                     u1 = vb.x - va.x;
                     v1 = vb.y - va.y;
                     u2 = vc.x - va.x;
                     v2 = vc.y - va.y;
        break;
      }

      double alpha, beta;
      if(Math.Abs(u1) < 1e-9)
      {
        beta = u0/u2;
        if((beta < 0.0) || (beta > 1.0))
          return new Intersection();
        alpha = (v0 - beta * v2) / v1;
      }
      else
      {
        beta = (v0 * u1 - u0 * v1) / (v2 * u1 - u2 * v1);
        if((beta < 0.0) || (beta > 1.0))
          return new Intersection();
        alpha = (u0 - beta * u2) / u1;
      }
      if((alpha >= 0.0) && ((alpha + beta) <= 1.0))
        return i;
      // Outside triangle
      return new Intersection();
    }

    /// <summary>
    /// Return the surface normal (a constant)
    /// </summary>
    /// <param name="p">Any old point whatsoever</param>
    /// <returns>The constant surface normal</returns>
    public override Vector3 ShapeNormal(Point3 p)
    {
      return plane.Normal();
    }

    /// <summary>
    /// You can never be inside a zero-volume object!
    /// </summary>
    /// <param name="p">Any old point whatsoever</param>
    /// <returns>false</returns>
    public override bool Inside(Point3 p)
    {
      return false;
    }

    /// <summary>
    /// Apply a transformation to this triangle
    /// </summary>
    /// <param name="t"></param>
    public override void Apply(Transform t)
    {
      va *= t;
      vb *= t;
      vc *= t;

      // Tranform the underlying plane
      plane.Apply(t);

      // Recalculate dominant axis
      DominantAxis();
    }

    /// <summary>
    /// Determine which of the space axes we are most nearly
    /// perpendicular to so that we can choose a non-degenerate
    /// projection plane to work in.  (if the dominant axis is Z
    /// then we project onto the X-Y plane etc.  
    /// </summary>
    private void DominantAxis()
    {
      var norm = plane.Normal();
      var nx = Math.Abs(norm.x);
      var ny = Math.Abs(norm.y);
      var nz = Math.Abs(norm.z);

      dominant = Axis.X;

      if(ny > nx)
      {
        dominant = Axis.Y;
        if(nz > ny)
          dominant = Axis.Z;
      }
      else
      {
        if(nz > nx)
          dominant = Axis.Z;
      }
    }
  }
  #endregion

  #region Solid
  /// <summary>
  /// Base class of all solid objects (with a clearly defined interior)
  /// </summary>
  public abstract class Solid : Model
  {
    /// <summary>
    /// Calculate all intersections with a ray (used for CSG)
    /// Returns a list of intersections, sorted by distance from the
    /// ray origin. Points where the ray exits the object are included
    /// in the list.  All of the intersections in a list must lie on a
    /// straight line (rays are never reflected or refracted.)
    /// </summary>
    /// <param name="r">The ray to test against</param>
    /// <returns>A list of intersections (if any)</returns>
    public abstract IntersectionList AllIntersections(Ray r);

    // These operators provide quick CSG operations
    // The material for the CSG is taken from Solid a if not null.
    // otherwise use the material from b.
    // Note that a material can be set subsequently and it will
    // propogate to the children that have no material set
    public static CSG operator | (Solid a, Solid b)
    {
      return new CSGUnion(a, b, BaseMaterial(a, b));
    }

    public static CSG operator & (Solid a, Solid b)
    {
      return new CSGIntersection(a, b, BaseMaterial(a, b));
    }

    public static CSG operator - (Solid a, Solid b)
    {
      return new CSGDifference(a, b, BaseMaterial(a, b));
    }

    /// <summary>
    /// Try to find a material to assign to every object because its a
    /// fatal error if we find a null material during a run.  
    /// We only overwrite null materials.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    private static Material BaseMaterial(Solid a, Solid b)
    {
      var m = a.Material ?? b.Material;
      return m;
    }
 }
  #endregion

  #region Sphere
  /// <summary>
  /// A solid sphere. Very fast and efficient but it cannot be deformed
  /// by a transform, only translated.  Use the quadric sphere if you
  /// need to apply deforming transformations.  It is quite OK to apply
  /// a deforming transformation to a sphere and it will properly ignore
  /// all but the linear and rotational translations.
  /// </summary>
  public sealed class Sphere : Solid
  {
    Point3 centre;
    double radius;
    double radius2;

    /// <summary>
    /// Default to unit sphere at the origin
    /// </summary>
    public Sphere()
    {
      centre = new Point3(0.0, 0.0, 0.0);
      radius = 1.0;
      radius2 = 1.0;
    }

    /// <summary>
    /// Construct a general sphere
    /// </summary>
    /// <param name="centre">Centre point</param>
    /// <param name="radius">The radius</param>
    /// <param name="m">The material</param>
    public Sphere(Point3 centre, double radius, Material m)
    {
      this.centre = centre;
      this.radius = radius;
      radius2 = this.radius * this.radius;
      Material = m;
    }

    /// <summary>
    /// Construct a general sphere with a default material
    /// </summary>
    /// <param name="centre">Centre point</param>
    /// <param name="radius">The radius</param>
    public Sphere(Point3 centre, double radius)
      : this(centre, radius, null)
    {}

    /// <summary>
    /// Sphere/ray intersection calculations
    /// </summary>
    /// <param name="r">The ray</param>
    /// <returns>An intersection, possibly empty</returns>
    public override Intersection Intersect(Ray r)
    {
      var v = centre - r.Origin;
      var b = v * r.Direction;
      var disc = b * b - v * v + radius2;
      if(disc <= 0.0) return new Intersection();
      disc = Math.Sqrt(disc);

      var t = b - disc;
      if(t > Constant.Epsilon)
        return new Intersection(r.At(t), t, true, this, null);

      t = b + disc;
      if(t > Constant.Epsilon)
        return new Intersection(r.At(t), t, true, this, null);

      return new Intersection();
    }

    /// <summary>
    /// Sphere/ray intersection calculations for CSG
    /// </summary>
    /// <param name="r">The ray</param>
    /// <returns>An intersection list, possibly empty</returns>
    public override IntersectionList AllIntersections(Ray r)
    {
      var hit = new IntersectionList();
      
      var v = centre - r.Origin;
      var b = v * r.Direction;
      var disc = b * b - v * v + radius2;

      if(disc < 0.0) return hit;   // Missed
      disc = Math.Sqrt(disc);

      var t2 = b + disc;
      if(t2 < Constant.Epsilon) return hit;     // Looking away

      var t1 = b - disc;
      if(t1 > Constant.Epsilon)
        hit.Enqueue(new Intersection(r.At(t1), t1, true, this, null));

      hit.Enqueue(new Intersection(r.At(t2), t2, false, this, Material));
      return hit;
    }

    /// <summary>
    /// Surface normal at a point 
    /// </summary>
    /// <param name="p">The point</param>
    /// <returns>The corresponding surface normal</returns>
    public override Vector3 ShapeNormal(Point3 p)
    {
      return (p - centre).Normalise();
    }

    /// <summary>
    /// Is a point inside the sphere?
    /// </summary>
    /// <param name="p">The point</param>
    /// <returns>true if the point is inside this sphere</returns>
    public override bool Inside(Point3 p)
    {
      return (p - centre).Norm() < radius;
    }

    /// <summary>
    /// As stated above, we just move the centre
    /// </summary>
    /// <param name="t"></param>
    public override void Apply(Transform t)
    {
      centre *= t;
    }

    /// <summary>
    /// The location of the centre
    /// </summary>
    public Point3 Centre { get { return centre; } }

    /// <summary>
    /// The radius
    /// </summary>
    public double Radius { get { return radius; } }
  }
  #endregion

  #region Plane
  /// <summary>
  /// An semi-infinite plane with an infinite surface.  Note that planes
  /// are Solids and they have an interior which fills precisely half of
  /// the Universe (regardless of where they are located, thats the clever bit ;)
  /// Typically served with a checkerboard pigment and garnished with spheres
  /// of a suspiciously high specular reflection coefficient.
  /// </summary>
  public class Plane : Solid
  {
    /// <summary>
    /// Any point in the plane
    /// </summary>
    Point3  location;

    /// <summary>
    /// The surface normal
    /// </summary>
    Vector3 normal;

    /// <summary>
    /// The perpendicular distance from the plane to the origin.  Negative if the
    /// plane encloses the origin
    /// </summary>
    double  d;

    /// <summary>
    /// Construct a plane with a material 
    /// </summary>
    /// <param name="location">Any point in the plane</param>
    /// <param name="normal">The surface normal vector</param>
    /// <param name="m">The material</param>
    public Plane(Point3 location, Vector3 normal, Material m)
    {
      this.location = location;
      this.normal = normal.Normalise();
      d = -(this.location * this.normal);
      Material = m;
    }

    /// <summary>
    /// Construct a plane with a default material 
    /// </summary>
    /// <param name="location">Any point in the plane</param>
    /// <param name="normal">The surface normal vector</param>
    public Plane(Point3 location, Vector3 normal)
      : this(location, normal, null)
    {}
    
    /// <summary>
    /// Calculate ray/plane intersections
    /// </summary>
    /// <param name="r">The ray</param>
    /// <returns>An intersection, possibly empty </returns>
    public override Intersection Intersect(Ray r)
    {
      var no = normal * r.Origin;
      var outside = no >= -d;
      var nd = normal * r.Direction;
      if(Math.Abs(nd) < Constant.Epsilon)
        return new Intersection();           // Parallel ray

      var t = -(d + no) / nd;
      if(Math.Abs(t) < Constant.Epsilon)
      {
        // If the viewpoint is very close to the plane rounding errors are a problem
        // Use a more direct approach to determine intersection
        if((nd > 0.0) ^ outside)
          return new Intersection(r.At(Constant.Epsilon),      // Location
                                  Constant.Epsilon,            // Distance
                                  outside,                     // Entering?
                                  this,                        // Model hit
                                  outside ? null : Material);
        else return new Intersection();
      }

      if(t < 0.0) return new Intersection();                   // Looking away
      return new Intersection(r.At(t),                         // Location
                              t,                               // Distance
                              outside,                         // Entering?
                              this,                            // Model hit
                              outside ? null : Material);      // Medium

   }

    /// <summary>
    /// Returns an intersection list with zero or one intersection
    /// in it.
    /// </summary>
    /// <param name="r">The ray</param>
    /// <returns>An intersection list, possibly empty</returns>
    public override IntersectionList AllIntersections(Ray r)
    {
      var l = new IntersectionList();
      var i = Intersect(r);
      if(i.model != null)
        l.Enqueue(i);
      return l;
    }

    public override Vector3 ShapeNormal(Point3 p)
    {
      return normal;
    }

    public Vector3 Normal()
    {
      return normal;
    }

    public override bool Inside(Point3 p)
    {
      return (normal * p) < -d;
    }

    public override void Apply(Transform t)
    {
      location *= t;
      normal *= t;
      normal = normal.Normalise();
      d = -(location * normal);
    }
  }
  #endregion

  #region Quadric

  /// <summary>
  /// A general quadric equation 
  /// ax^2 + by^2 + cz^2 + 2dxy, + 2eyz + 2fxz + 2gx + 2hy + 2jz + k = 0.0
  /// Makes spheres, cones, paraboloids, hyperboloids etc...
  /// </summary>
  public class Quadric : Solid
  {
    Matrix q;

    // Build the quadric
    public Quadric(double a, double b, double c, double d, double e, double f, double g, double h, double j, double k,
                   Material m = null)
    {
      // Build the matrix form of the Quadric equation
      q = new Matrix(a, d, f, g, d, b, e, h, f, e, c, j, g, h, j, k);
      Material = m;
    }

    void GetABC(Ray r, out double a, out double b, out double c)
    {
      // Believe it or not there are no common products of ray parameters here!
      a = q[0, 0] * r.Direction.x * r.Direction.x +
          q[1, 1] * r.Direction.y * r.Direction.y +
          q[2, 2] * r.Direction.z * r.Direction.z +
          2.0 * ( q[0, 1] * r.Direction.x * r.Direction.y +
                  q[1, 2] * r.Direction.y * r.Direction.z +
                  q[2, 0] * r.Direction.x * r.Direction.z );

      b = 2.0 * ( q[0, 0] * r.Origin.x * r.Direction.x +
                  q[1, 1] * r.Origin.y * r.Direction.y +
                  q[2, 2] * r.Origin.z * r.Direction.z +
                  q[0, 1] * (r.Origin.x * r.Direction.y +
                             r.Origin.y * r.Direction.x) +
                  q[1, 2] * (r.Origin.y * r.Direction.z +
                             r.Origin.z * r.Direction.y) +
                  q[2, 0] * (r.Origin.x * r.Direction.z +
                             r.Origin.z * r.Direction.x) +
                  q[3, 0] * r.Direction.x +
                  q[1, 3] * r.Direction.y +
                  q[2, 3] * r.Direction.z );

      c = q[0, 0] * r.Origin.x * r.Origin.x +
          q[1, 1] * r.Origin.y * r.Origin.y +
          q[2, 2] * r.Origin.z * r.Origin.z +
          q[3, 3] +
          2.0 * ( q[0, 1] * r.Origin.x * r.Origin.y +
                  q[1, 2] * r.Origin.y * r.Origin.z +
                  q[2, 0] * r.Origin.x * r.Origin.z +
                  q[3, 0] * r.Origin.x +
                  q[1, 3] * r.Origin.y +
                  q[2, 3] * r.Origin.z );
    }

    public override Intersection Intersect(Ray r)
    {
      var l = AllIntersections(r);

      if (l.Empty())
        return new Intersection();
      else
        return l.Dequeue();
    }

    public override IntersectionList AllIntersections(Ray r)
    {
      var hit = new IntersectionList();

      double a, b, c, d;
      double t, t1, t2;

      // Obtain a quadratic in t
      GetABC(r, out a, out b, out c);

      // Solve it in the usual way
      d = b * b - 4 * a * c;
      if(d <= 0)
        return hit; // Empty list - fail

      d = Math.Sqrt(d);
      a += a;

      if(Math.Abs(a) < Constant.Epsilon)
        return hit; // Empty list - fail

      t1 = (-b + d) / a;
      t2 = (-b - d) / a;

      // Must keep intersections sorted
      if(t1 > t2)
      {
        t = t1;    // Swap
        t1 = t2;
        t2 = t;
      }

      if(t1 > Constant.Epsilon)
        hit.Enqueue(new Intersection(r.At(t1), t1, true, this, null));
      if(t2 > Constant.Epsilon)
        hit.Enqueue(new Intersection(r.At(t2), t2, false, this, Material));

      return hit;
    }

    public override Vector3 ShapeNormal(Point3 p)
    {
      return new Vector3
         (q[0, 0] * p.x + q[0, 1] * p.y + q[2, 0] * p.z + q[3, 0],
          q[0, 1] * p.x + q[1, 1] * p.y + q[1, 2] * p.z + q[1, 3],
          q[2, 0] * p.x + q[1, 2] * p.y + q[2, 2] * p.z + q[2, 3]).Normalise();
     }

    public override bool Inside(Point3 p)
    {
      // Evaluate Quadric at p (= p*Q.p in homogeneous coordinates)
      var r = ( p.x * q[0, 0] +
                   p.y * q[0, 1] +
                   p.z * q[0, 2] +
                         q[0, 3] ) * p.x +
                 ( p.x * q[1, 0] +
                   p.y * q[1, 1] +
                   p.z * q[1, 2] +
                         q[1, 3] ) * p.y +
                 ( p.x * q[2, 0] +
                   p.y * q[2, 1] +
                   p.z * q[2, 2] +
                         q[2, 3] ) * p.z +
                 ( p.x * q[3, 0] +
                   p.y * q[3, 1] +
                   p.z * q[3, 2] +
                         q[3, 3] );
      return(r < 0.0);
    }

    public override void Apply(Transform t)
    {
      var ti = t.Inverse();
      q = (ti * q) * ti.Transpose();
    }
  }
  #endregion

  #region PresetQuadrics
  // A collection of standard Quadrics
  #region Ellipsoid
  public class Ellipsoid : Quadric
  {
    public Ellipsoid(double rx, double ry, double rz, Material m)
      : base(1.0 / (rx * rx), 1.0 / (ry * ry), 1.0 / (rz * rz),
             0.0, 0.0, 0.0,
             0.0, 0.0, 0.0, -1.0, m)
    {}

    public Ellipsoid(double rx, double ry, double rz) : this(rx, ry, rz, null)
    {}
  }
  #endregion

  #region CylinderX
  public class CylinderX : Quadric
  {
    public CylinderX(double ry, double rz, Material m)
      : base(0.0, 1.0 /(ry * ry), 1.0 / (rz * rz),
             0.0, 0.0, 0.0,
             0.0, 0.0, 0.0, -1.0, m)
    {}

    public CylinderX(double ry, double rz) : this(ry, rz, null)
    {}
  }
  #endregion

  #region CylinderY
  public class CylinderY : Quadric
  {
    public CylinderY(double rx, double rz, Material m)
      : base(1.0 / (rx * rx), 0.0, 1.0 / (rz * rz),
             0.0, 0.0, 0.0,
             0.0, 0.0, 0.0, -1.0, m)
    {}

    public CylinderY(double rx, double rz) : this(rx, rz, null)
    {}
  }
  #endregion

  #region CylinderZ
  public class CylinderZ : Quadric
  {
    public CylinderZ(double rx, double ry, Material m)
      : base(1.0 / (rx * rx), 1.0 / (ry * ry), 0.0,
             0.0, 0.0, 0.0,
             0.0, 0.0, 0.0, -1.0, m)
    {}

    public CylinderZ(double rx, double ry) : this(rx, ry, null)
    {}
  }
  #endregion

  #region ConeX
  public class ConeX : Quadric
  {
    public ConeX(double ry, double rz, Material m)
      : base(-1.0, 1.0 / (ry * ry), 1.0 / (rz * rz),
              0.0, 0.0, 0.0,
              0.0, 0.0, 0.0, 0.0, m)
    {}

    public ConeX(double ry, double rz) : this(ry, rz, null)
    {}
  }
  #endregion

  #region ConeY
  public class ConeY : Quadric
  {
    public ConeY(double rx, double rz, Material m)
      : base(1.0 / (rx * rx), -1.0, 1.0 / (rz * rz),
             0.0, 0.0, 0.0,
             0.0, 0.0, 0.0, 0.0, m)
    {}

    public ConeY(double rx, double rz) : this(rx, rz, null)
    {}
  }
  #endregion

  #region ConeZ
  public class ConeZ : Quadric
  {
    public ConeZ(double rx, double ry, Material m)
      : base(1.0 / (rx * rx), 1.0 / (ry * ry), -1.0,
             0.0, 0.0, 0.0,
             0.0, 0.0, 0.0, 0.0, m)
    {}

    public ConeZ(double rx, double ry) : this(rx, ry, null)
    {}
  }
  #endregion

  #region ParaboloidX
  public class ParaboloidX : Quadric
  {
    public ParaboloidX(double ry, double rz, Material m = null)
      : base( 0.0, 1.0 / (ry * ry), 1.0 / (rz * rz),
              0.0, 0.0, 0.0,
             -1.0, 0.0, 0.0, 0.0, m)
    {}
  }
  #endregion

  #region ParaboloidY
  public class ParaboloidY : Quadric
  {
    public ParaboloidY(double rx, double rz, Material m = null)
      : base(1.0 / (rx * rx), 0.0, 1.0 / (rz * rz),
             0.0, 0.0, 0.0,
             0.0, -1.0, 0.0, 0.0, m)
    {}
  }
  #endregion

  #region ParaboloidY
  public class ParaboloidZ : Quadric
  {
    public ParaboloidZ(double rx, double ry, Material m = null)
      : base(1.0 / (rx * rx), 1.0 / (ry * ry), 0.0,
             0.0, 0.0, 0.0,
             0.0, 0.0, -1.0, 0.0, m)
    {}
  }
  #endregion

  #region Hyperboloid
  public class Hyperboloid : Quadric
  {
    public Hyperboloid(Material m)
      : base(-1.0, 0.0, 1.0,
              0.0, 0.0, 0.0,
              0.0, 1.0, 0.0, 0.0, m)
    {}

    public Hyperboloid() : this(null)
    {}
  }
  #endregion

  #region HyperboloidY
  public class HyperboloidY : Quadric
  {
    public HyperboloidY(double rx, double rz, Material m = null)
      : base(1.0 / (rx * rx), -1.0, 1.0 / (rz * rz),
               0.0, 0.0, 0.0,
               0.0, 0.0, 0.0, -1.0, m)
    {}
  }
  #endregion

  #endregion

  #region CSG
  // Base class for constructive solid geometry
  public abstract class CSG : Solid
  {
    protected Solid left;
    protected Solid right;

    public CSG(Solid l, Solid r, Material m = null)
    {
      if((l == null) || (r == null))
        throw new AuroraException("CSG  Null solid!");


      left = l;
      right = r;

      Material = m;
    }

    public override Material Material
    {
      set
      {
        material = value;
        if(value != null)
        {
          if(left.Material == null)
            left.Material = material;
          if(right.Material == null)
            right.Material = material;
        }
      }
      get { return material; }
    }

    // Apply a transformation
    public override void Apply(Transform t)
    {
      left.Apply(t);
      right.Apply(t);
    }

    // Don't override Model.Normal() which throws an exception
    // Intersection.Model is always a primitive and primitives all
    // return a valid normal
  }
  #endregion

  #region CSGUnion
  // The union of any two solids
  public class CSGUnion : CSG
  {
    public CSGUnion(Solid l, Solid r, Material m = null) : base(l, r, m) {}

    // Calculate nearest point of intersection with ray
    public override Intersection Intersect(Ray r)
    {
      var lint = left.Intersect(r);
      var rint = right.Intersect(r);
      return (lint < rint) ? lint : rint;
    }

    // Calculate all intersections with ray
    public override IntersectionList AllIntersections(Ray r)
    {
      var lint = left.AllIntersections(r);
      var rint = right.AllIntersections(r);
      var result = new IntersectionList();

      // If one list is empty then return the other
      if(lint.Empty()) return rint;
      if(rint.Empty()) return lint;

      var insidel = left.Inside(r.Origin);
      var insider = right.Inside(r.Origin);

      // Copy intersections that fall without any span of the other leaf
      while(!lint.Empty() && !rint.Empty())
      {
        // Always maintain the distance ordering in the result list
        if(lint.Distance() < rint.Distance())
        {
          insidel = lint.Entering();
          if(!insider)                       // Outside span on right?
            result.Enqueue(lint.Dequeue());  // It's a real intersection
          else
            lint.Drop();                     // It's not
        }
        else
        {
          insider = rint.Entering();         // Much as the above
          if(!insidel)
            result.Enqueue(rint.Dequeue());
          else
            rint.Drop();
        }
      }
      // Copy remaining intersections if outside other leaf
      while(!lint.Empty() && !insider)
        result.Enqueue(lint.Dequeue());
      while(!rint.Empty() && !insidel)
        result.Enqueue(rint.Dequeue());

      return result;
    }

    // Test if inside a model
    public override bool Inside(Point3 p)
    {
      return left.Inside(p) || right.Inside(p);
    }
  }
  #endregion

  #region CSGIntersection
  // The intersection of any two solids
  public class CSGIntersection : CSG
  {
    public CSGIntersection(Solid l, Solid r, Material m = null) : base(l, r, m) {}

    // Calculate nearest point of intersection with ray
    public override Intersection Intersect(Ray r)
    {
      var all = AllIntersections(r);
      return (all.Empty()) ? new Intersection() : all.Dequeue();
    }

    // Calculate all intersections with ray
    public override IntersectionList AllIntersections(Ray r)
    {
      var lint = left.AllIntersections(r);
      var rint = right.AllIntersections(r);
      var result = new IntersectionList();

      var insidel = left.Inside(r.Origin);
      var insider = right.Inside(r.Origin);

      // If we never get inside the left leaf then return an empty list
      if(lint.Empty() && !insidel)
        return result;

      // ...likewise on the right
      if(rint.Empty() && !insider)
        return result;

      // Copy intersections that fall within any span of the other leaf
      while(!lint.Empty() && !rint.Empty())
      {
        // Always maintain the distance ordering in the result list
        if(lint.Distance() < rint.Distance())  // What if they are equal?
        {
          insidel = lint.Entering();
          if(insider)                          // Inside span on right?
            result.Enqueue(lint.Dequeue());    // It's a real intersection
          else
            lint.Drop();                       // It's not
        }
        else  // Nearest on right
        {
          insider = rint.Entering();     // Much as the above
          if(insidel)
            result.Enqueue(rint.Dequeue());
          else
            rint.Drop();
        }
      }
      // Copy remaining intersections if inside other leaf
      while(!lint.Empty() && insider)
        result.Enqueue(lint.Dequeue());
      while(!rint.Empty() && insidel)
        result.Enqueue(rint.Dequeue());

      return result;
    }

    // Test if inside a model
    public override bool Inside(Point3 p)
    {
      return left.Inside(p) && right.Inside(p);
    }
  }
  #endregion

  #region CSGDifference
  // The difference of any two solids
  public class CSGDifference : CSG
  {
    public CSGDifference(Solid l, Solid r, Material m = null) : base(l, r, m) {}

    // Calculate nearest point of intersection with ray
    public override Intersection Intersect(Ray r)
    {
      var all = AllIntersections(r);
      return (all.Empty()) ? new Intersection() : all.Dequeue();
    }

    // Calculate all intersections with ray
    public override IntersectionList AllIntersections(Ray r)
    {
      var lint = left.AllIntersections(r);
      var result = new IntersectionList();

      var insidel = left.Inside(r.Origin);
      // If we never get inside the left leaf then return an empty list
      if(lint.Empty() && !insidel)
        return result;

      var rint = right.AllIntersections(r);

      // Invert sense of all intersections on right
      foreach(var i in rint)
        i.Flip();

      var insider = !right.Inside(r.Origin);
      // If we never get inside the (inverted) right leaf then return an empty list
      if(rint.Empty() && !insider)
        return result;

      // Copy intersections that fall within any span of the other leaf
      while(!lint.Empty() && !rint.Empty())
      {
        // Always maintain the distance ordering in the result list
        if(lint.Distance() < rint.Distance())    // What if they are equal?
        {
          insidel = lint.Entering();
          if(insider)                            // Inside span on right?
            result.Enqueue(lint.Dequeue());      // It's a real intersection
          else
            lint.Drop();                         // It's not
        }
        else  // Nearest on right
        {
          insider = rint.Entering();             // Much as the above
          if(insidel)
            result.Enqueue(rint.Dequeue());
          else
            rint.Drop();
        }
      }
      // Copy remaining intersections if inside other leaf
      while(!lint.Empty() && insider)
        result.Enqueue(lint.Dequeue());
      while(!rint.Empty() && insidel)
        result.Enqueue(rint.Dequeue());

      return result;
    }

    // Test if inside a model
    public override bool Inside(Point3 p)
    {
      return left.Inside(p) && !right.Inside(p);
    }
  }
  #endregion

  #region CSGModel
  // An encapsulated CSG model for CSG primitives
  public abstract class CSGModel : Solid
  {
    // Derived classes *must* instantiate csg
    protected Solid csg;

    // Delegate everything to csg

    // Calculate nearest point of intersection with ray
    public override Intersection Intersect(Ray r)
    {
      return csg.Intersect(r);
    }

    // Calculate all intersections with ray
    public override IntersectionList AllIntersections(Ray r)
    {
      return csg.AllIntersections(r);
    }

    // Apply a transformation
    public override void Apply(Transform t)
    {
      csg.Apply(t);
    }

    // Is point Inside?
    public override bool Inside(Point3 p)
    {
      return csg.Inside(p);
    }
  }
  #endregion

  #region Cuboid
  // A CSG axis-aligned cuboid
  public class Cuboid : CSGModel
  {
    // CSGModels need only supply a constructor (or two!)
    public Cuboid(Point3 centre, double sx, double sy, double sz, Material m = null)
    {
      sx /= 2.0;
      sy /= 2.0;
      sz /= 2.0;
      
      csg  = new Plane(centre + new Vector3(sx, 0.0, 0.0),  Vector3.UnitX);
      csg &= new Plane(centre - new Vector3(sx, 0.0, 0.0), -Vector3.UnitX);
      csg &= new Plane(centre + new Vector3(0.0, sy, 0.0),  Vector3.UnitY);
      csg &= new Plane(centre - new Vector3(0.0, sy, 0.0), -Vector3.UnitY);
      csg &= new Plane(centre + new Vector3(0.0, 0.0, sz),  Vector3.UnitZ);
      csg &= new Plane(centre - new Vector3(0.0, 0.0, sz), -Vector3.UnitZ);

      material = csg.Material = m;
    }
  }
  #endregion

  #region Cube
  // A CSG axis-aligned cube
  public class Cube : Cuboid
  {
    public Cube(Point3 centre, double size, Material m = null)
      : base(centre, size, size, size, m)
    {}
  }
  #endregion
}

