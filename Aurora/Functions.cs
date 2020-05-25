#region Using directives

using System;

#endregion

namespace Aurora
{
  // These functions are used for procedural textures
  // The two delegates define the signatures for pigments (RealFunction)
  // and normals (VectorFunction)

  // A real function in R^3
  public delegate double RealFunction(Point3 p);

  // A vector function in R^3
  public delegate Vector3 VectorFunction(Point3 p);

  public static class RealFunctions
  {
    /// <summary>
    /// Checkerboard texture
    /// </summary>
    /// <param name="p">point</param>
    /// <returns>function value</returns>
    public static double Checker(Point3 p)
    {
      var nx = (int)Math.Floor(p.x);
      var ny = (int)Math.Floor(p.y);
      var nz = (int)Math.Floor(p.z);

      return ((nx & 1) ^ (ny & 1) ^ (nz & 1)) == 0 ? 1.0 : 0.0;
    }

    /// <summary>
    /// Layered onion texture
    /// </summary>
    /// <param name="p">point</param>
    /// <returns>function value</returns>
    public static double Onion(Point3 p)
    {
      var r = Math.Sqrt(p.x * p.x + p.y * p.y + p.z * p.z);
      return r - Math.Floor(r);
    }

    /// <summary>
    /// Basic perlin noise
    /// </summary>
    /// <param name="p"></param>
    /// <returns>function value</returns>
    public static double Perlin(Point3 p)
    {
      var r = ImprovedNoise.noise(p.x, p.y, p.z);
      return r / 2 + 0.5;
    }

    /// <summary>
    /// Rectified perlin noise
    /// </summary>
    /// <param name="p"></param>
    /// <returns>function value</returns>
    public static double PerlinRidged(Point3 p)
    {
      var r = ImprovedNoise.noise(p.x, p.y, p.z);
      return Math.Abs(r);
    }

    /// <summary>
    /// Just return p.GetHashCode()
    /// </summary>
    /// <param name="p"></param>
    /// <returns>function value</returns>
    public static double Noise(Point3 p)
    {
      var r = p.GetHashCode();
      return Math.Abs(r / int.MaxValue);
    }
  }

  /// <summary>
  /// Self similar noise
  /// </summary>
  public class PerlinOctaves
  {
    private readonly double[] mult;
    private readonly double sum;

    /// <summary>
    /// Construct a noise generator with a specified spectrum
    /// </summary>
    /// <param name="multipliers"></param>
    public PerlinOctaves(params double[] multipliers)
    {
      mult = multipliers;
      sum = 0.0;
      foreach(var m in mult)
        sum += m;
    }

    /// <summary>
    /// Evaluate noise
    /// </summary>
    /// <param name="p">point</param>
    /// <returns>function value</returns>
    public double Eval(Point3 p)
    {
      var result = 0.0;
      foreach(var m in mult)
      {
        result += m * ImprovedNoise.noise(p.x, p.y, p.z);
        p = p * 2;
      }
      var r = (result / sum + 1) / 2;
      return r;
    }
  }

  public static class VectorFunctions
  {
    /// <summary>
    /// Good old Perlin noise
    /// </summary>
    /// <param name="p">point</param>
    /// <returns>function value</returns>
    public static Vector3 Perlin(Point3 p)
    {
      return ImprovedNoise.noiseVector(p.x, p.y, p.z);
    }
  }

  /// <summary>
  /// Self similar noise
  /// </summary>
  public class VectorPerlinOctaves
  {
    readonly double[] mult;

    /// <summary>
    /// Construct a noise generator with a specified spectrum
    /// </summary>
    /// <param name="multipliers"></param>
    public VectorPerlinOctaves(params double[] multipliers)
    {
      mult = multipliers;
    }

    /// <summary>
    /// Evaluate noise
    /// </summary>
    /// <param name="p">point</param>
    /// <returns>function value</returns>
    public Vector3 Eval(Point3 p)
    {
      double x = 0.0, y = 0.0, z = 0.0;
      foreach(var m in mult)
      {
        x += m * ImprovedNoise.noise(p.x, p.y, p.z);
        y += m * ImprovedNoise.noise(p.z, p.x, p.y);
        z += m * ImprovedNoise.noise(p.y, p.z, p.x);
        p = p * 2;
      }
      var v = new Vector3(x, y, z);
      return v;
    }
  }
}
