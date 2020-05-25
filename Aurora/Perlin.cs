#region Using directives

using System;

#endregion

namespace Aurora
{
  // Found on the Web with no attribution
  // Modified/translated to C#
  public class ImprovedNoise
  {
    // Yer basic Perlin noise function in 3D
    static public double noise(double x, double y, double z)
    {
      var X = (int)Math.Floor(x) & 255;                  // find unit cube that
      var Y = (int)Math.Floor(y) & 255;                  // contains point.
      var Z = (int)Math.Floor(z) & 255;
      x -= Math.Floor(x);                                // find relative x,y,z
      y -= Math.Floor(y);                                // of point in cube.
      z -= Math.Floor(z);
      var u = fade(x);                                   // compute fade curves
      var v = fade(y);                                   // for each of x,y,z.
      var w = fade(z);
      var A = p[X] + Y;                                  // hash coordinates of
      var AA = p[A] + Z;                                 // the 8 cube corners,
      var AB = p[A + 1] + Z;      
      var B = p[X + 1] + Y;
      var BA = p[B] + Z;
      var BB = p[B + 1] + Z;      

      return lerp(w, lerp(v, lerp(u, grad(p[AA], x, y, z),                      // and add
                                     grad(p[BA], x - 1, y, z)),                 // blended
                             lerp(u, grad(p[AB], x, y - 1, z),                  // results
                                     grad(p[BB], x - 1, y - 1, z))),            // from  8
                             lerp(v, lerp(u, grad(p[AA + 1], x, y, z - 1),      // corners
                                             grad(p[BA + 1], x - 1, y, z - 1)), // of cube
                                     lerp(u, grad(p[AB + 1], x, y - 1, z - 1),
                                             grad(p[BB + 1], x - 1, y - 1, z - 1))));
    }
    static double fade(double t) { return t * t * t * (t * (t * 6 - 15) + 10); }

    static double lerp(double t, double a, double b) { return a + t * (b - a); }

    static double grad(int hash, double x, double y, double z)
    {
      var h = hash & 15;                                // Convert low 4 bits of hash code
      var u = h < 8 || h == 12 || h == 13 ? x : y;      // into 12 gradient directions.
      var v = h < 4 || h == 12 || h == 13 ? y : z;
      return ((h & 1) == 0 ? u : -u) + ((h & 2) == 0 ? v : -v);
    }

    // This is probably wrong in so many ways
    // but it produces interesting normals
    static public Vector3 noiseVector(double x, double y, double z)
    {
      return new Vector3(noise(x, y, z), noise(y, z, x), noise(z, x, y));
    }

    static int[] p = {151,160,137,91,90,15,
    131,13,201,95,96,53,194,233,7,225,140,36,103,30,69,142,8,99,37,240,21,10,23,
    190, 6,148,247,120,234,75,0,26,197,62,94,252,219,203,117,35,11,32,57,177,33,
    88,237,149,56,87,174,20,125,136,171,168, 68,175,74,165,71,134,139,48,27,166,
    77,146,158,231,83,111,229,122,60,211,133,230,220,105,92,41,55,46,245,40,244,
    102,143,54, 65,25,63,161, 1,216,80,73,209,76,132,187,208, 89,18,169,200,196,
    135,130,116,188,159,86,164,100,109,198,173,186, 3,64,52,217,226,250,124,123,
    5,202,38,147,118,126,255,82,85,212,207,206,59,227,47,16,58,17,182,189,28,42,
    223,183,170,213,119,248,152, 2,44,154,163, 70,221,153,101,155,167, 43,172,9,
    129,22,39,253, 19,98,108,110,79,113,224,232,178,185, 112,104,218,246,97,228,
    251,34,242,193,238,210,144,12,191,179,162,241, 81,51,145,235,249,14,239,107,
    49,192,214, 31,181,199,106,157,184, 84,204,176,115,121,50,45,127, 4,150,254,
    138,236,205,93,222,114,67,29,24,72,243,141,128,195,78,66,215,61,156,180,
    151,160,137,91,90,15,
    131,13,201,95,96,53,194,233,7,225,140,36,103,30,69,142,8,99,37,240,21,10,23,
    190, 6,148,247,120,234,75,0,26,197,62,94,252,219,203,117,35,11,32,57,177,33,
    88,237,149,56,87,174,20,125,136,171,168, 68,175,74,165,71,134,139,48,27,166,
    77,146,158,231,83,111,229,122,60,211,133,230,220,105,92,41,55,46,245,40,244,
    102,143,54, 65,25,63,161, 1,216,80,73,209,76,132,187,208, 89,18,169,200,196,
    135,130,116,188,159,86,164,100,109,198,173,186, 3,64,52,217,226,250,124,123,
    5,202,38,147,118,126,255,82,85,212,207,206,59,227,47,16,58,17,182,189,28,42,
    223,183,170,213,119,248,152, 2,44,154,163, 70,221,153,101,155,167, 43,172,9,
    129,22,39,253, 19,98,108,110,79,113,224,232,178,185, 112,104,218,246,97,228,
    251,34,242,193,238,210,144,12,191,179,162,241, 81,51,145,235,249,14,239,107,
    49,192,214, 31,181,199,106,157,184, 84,204,176,115,121,50,45,127, 4,150,254,
    138,236,205,93,222,114,67,29,24,72,243,141,128,195,78,66,215,61,156,180};
  }
}
