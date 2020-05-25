using System;

namespace Aurora
{
  #region Finish
  // A surface finish - part of a material definition
  public class Finish
  {
    public double Diffuse     // Diffuse reflection coefficient
    { get;  private set; }
    public  double Specular    // Specular reflection coefficient
    { get;  private set; }
    public double PhongSize   // Tightness of specular relection
    { get;  private set; }
    public double Reflectance // Perfect specular reflection coefficient
    { get; private set; }

    public Finish(double diffuse, double specular,
                  double phongSize, double reflectance)
    {
      Diffuse = diffuse;
      Specular = specular;
      PhongSize = phongSize;
      Reflectance = reflectance;
    }

    public Finish() : this(1.0, 0.0, 1.0, 0.0) { }
  }
  #endregion

  #region Pigment

  public class Pigment
  {
    private Colour a;
    private Colour b;
    private double scale;

    // A vector function that is interpreted as a colour
    public VectorFunction fv;

    // A real function that interpolates two colours
    public RealFunction fr;

    public Pigment(Colour d)
    {
      a = d;
    }

    public Pigment(Colour d, Colour e, RealFunction f)
    {
      a = d;
      b = e;
      fr = f;
      scale = 1.0;
    }

    public Pigment(VectorFunction f)
    {
      fv = f;
      scale = 1.0;
    }

    public Colour SpotColour(Point3 p)
    {
      if(fv == null)
      {
        if(fr == null)
          return a;
        else
          return Colour.Interpolate(a, b, Math.Abs(fr(p / scale)));
      }
      else
      {
        var v = fv(p / scale);
        
        return new Colour(Math.Abs(v.x), Math.Abs(v.y), Math.Abs(v.z));
      }
    }

    public double Scale
    {
      get { return  scale; }
      set { scale = value; }
    }
  }

  #endregion

  #region Material
  // A material combines a surface finish with a pigment pattern
  public class Material
  {
    readonly Finish  finish;
    readonly Pigment pigment;

    Colour  filter;
    double  ior;
    bool    transparent;

    // Vector function added to normal
    VectorFunction perturbationFunction;

    // Scaling of perturb
    double pScale;
    // Amount of perturbation applied
    double pAmount;

    public static readonly Material NullMaterial;

    static Material()
    {
      NullMaterial = new Material(new Pigment(new Colour(0.0)), new Finish(0.0, 0.0, 0.0, 0.0));
      NullMaterial.MakeTransparent(new Colour(1.0), 1.0);
    }

    public Material(Pigment p, Finish f)
    {
      pigment = p;
      finish = f;
      filter = new Colour(1.0);
      ior = 1.0;
      transparent = false;
    }

    public Material(Colour c, Finish f) : this(new Pigment(c), f)
    { }

    public void MakeTransparent(Colour tfilter, double tior)
    {
      filter = tfilter;
      ior = tior;
      transparent = true;
    }

    // Setup a normal perturbation function
    public void PerturbNormal(double scale, double amount, VectorFunction f)
    {
      perturbationFunction = f;
      pScale = scale;
      pAmount = amount;
    }

    // Calculate the amount of perturbation
    public Vector3 NormalPerturbation(Point3 p)
    {
      if(perturbationFunction == null)
        return Vector3.Zero;
      else
        return pAmount * perturbationFunction(p / pScale);
    }

    // Check if normal is perturbed
    public bool Perturbed
    {
      get { return perturbationFunction != null; }
    }

    // Direct access to the various components of the material
    public Colour GetPigment(Point3 p)
    {
      return pigment.SpotColour(p);
    }

    public double Diffuse
    {
      get { return finish.Diffuse; }
    }

    public double Specular
    {
      get { return finish.Specular; }
    }

    public double PhongSize
    {
      get { return finish.PhongSize; }
    }

    public double Reflectance
    {
      get { return finish.Reflectance; }
    }

    public bool Transparent
    {
      get { return transparent; }
    }

    public Colour Filter
    {
      get { return filter; }
    }

    public double Ior
    {
      get { return ior; }
    }

    // Get the colour contribution for a surface illuminated by a single light
    // Add these for all lights to get the total colour (excluding any
    // contribution from perfect specular reflection)
    public Colour Illumination(Point3 p, Vector3 normal, Ray lightray, Light l)
    {
      // Cosine of angle of incidence for diffuse reflection
      var cosine = Math.Abs(lightray.Direction * normal);

      // The pixel colour is the product of the surface and light colours
      // scaled by the diffuse coefficient and the cosine derived above.
      // The specular component is added with the colour of the light source alone
      var illum = (pigment.SpotColour(p) * l.Colour) * (cosine * finish.Diffuse) +
                     l.Colour * (Math.Pow(cosine, finish.PhongSize) * finish.Specular);
      return illum * l.Intensity;
    }
  }
  #endregion
}

