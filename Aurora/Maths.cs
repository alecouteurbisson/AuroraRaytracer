using System;

namespace Aurora
{
  #region Constants
  // Some constants

  public enum Axis { X = 0, Y = 1, Z = 2 };

  public struct Constant
  {
    // A very small value
    public static double Epsilon = Double.Epsilon * 1e16;

    // A very large value
    public static double Huge = Double.MaxValue * 1e-6;

    // A small value (fuzz allowed when comparing matrices)
    public static double Fuzz = 1e-9;
  }
  #endregion

  #region Vector3

  /// <summary>
  /// A Vector in three dimensions
  /// Actually it's a 4-component homogeneous vector, [x, y, z, w]
  /// but w is implicitly zero.
  /// </summary>
  public struct Vector3
  {
    /// <summary>
    /// X coordinate
    /// </summary>
    public readonly double x;
    /// <summary>
    /// Y coordinate
    /// </summary>
    public readonly double y;
    /// <summary>
    /// Z coordinate
    /// </summary>
    public readonly double z;

    /// <summary>
    /// Unit X vector
    /// </summary>
    public static Vector3 UnitX = new Vector3(Axis.X);
    /// <summary>
    /// Unit Y Vector
    /// </summary>
    public static Vector3 UnitY = new Vector3(Axis.Y);
    /// <summary>
    /// Unit Z Vector
    /// </summary>
    public static Vector3 UnitZ = new Vector3(Axis.Z);

    /// <summary>
    /// Ready made zero vector
    /// </summary>
    public static Vector3 Zero  = new Vector3(0.0, 0.0, 0.0);

    /// <summary>
    /// Construct from three coordinate values
    /// </summary>
    /// <param name="x">x-coordinate</param>
    /// <param name="y">y-coordinate</param>
    /// <param name="z">z-coordinate</param>
    public Vector3(double x, double y, double z)
    {
      this.x = x;
      this.y = y;
      this.z = z;
    }

    private Vector3(Axis a)
    {
      x = (a == Axis.X) ? 1.0 : 0.0;
      y = (a == Axis.Y) ? 1.0 : 0.0;
      z = (a == Axis.Z) ? 1.0 : 0.0;
    }

    //public static Vector3 operator+ (Vector3 v)
    //{
    //  return v;
    //}

    public static Vector3 operator- (Vector3 v)
    {
        return new Vector3(-v.x, -v.y, -v.z);
    }

    public static Vector3 operator +(Vector3 u, Vector3 v)
    {
      return new Vector3(u.x + v.x, u.y + v.y, u.z + v.z);
    }

    public static Vector3 operator -(Vector3 u, Vector3 v)
    {
      return new Vector3(u.x - v.x, u.y - v.y, u.z - v.z);
    }

    public static double operator*(Vector3 u, Vector3 v)     // Dot product
    {
      return(u.x * v.x + u.y * v.y + u.z * v.z);
    }

    public static Vector3 operator^(Vector3 u, Vector3 v)    // Cross product
    {
      return new Vector3(u.y * v.z - u.z * v.y,
                        u.z * v.x - u.x * v.z,
                        u.x * v.y - u.y * v.x);
    }

    public static Vector3 operator*(Vector3 v, double r)     // Scalar product
    {
      return new Vector3(v.x * r, v.y * r, v.z * r);
    }

    public static Vector3 operator*(double r, Vector3 v)     // Scalar product
    {
      return new Vector3(v.x * r, v.y * r, v.z * r);
    }

    public static Vector3 operator/(Vector3 v, double r) 
    {
        return new Vector3(v.x / r, v.y / r, v.z / r);
    }

    // Element by element multiply used in Quadric
    public static Vector3 MultiplyElements(Vector3 u, Vector3 v)
    {
      return new Vector3(u.x * v.x, u.y * v.y, u.z * v.z);
    }

    public double Norm()
    {
      return Math.Sqrt(x*x + y*y + z*z);
    }

    // Normalise() normalises a vector and returns the original length
    public Vector3 Normalise()
    {
      var n = Norm();
      if(n < Constant.Epsilon)
        throw new AuroraException("Vector.Normalise - Norm() near zero");
      return this / n;
    }

    public static Vector3 operator*(Vector3 v, Transform t)
    {
       return new Vector3(v.x * t[0, 0] + v.y * t[1, 0] + v.z * t[2, 0],
                          v.x * t[0, 1] + v.y * t[1, 1] + v.z * t[2, 1],
                          v.x * t[0, 2] + v.y * t[1, 2] + v.z * t[2, 2]);
    }

    public override string ToString()
    {
      return string.Format("<{0:F3},{1:F3},{2:F3}, 0>", x, y, z);
    }

  }
  #endregion

  #region Point3
  // A point in space
  // Fully affected by transformations
  public struct Point3
  {
    public readonly double x;
    public readonly double y;
    public readonly double z;

    public static Point3 Origin = new Point3(0.0, 0.0, 0.0);

    public Point3(double x, double y, double z)
    {
      this.x = x;
      this.y = y;
      this.z = z;
    }

    // Allow a point to be silently coerced to a vector from the origin
    public static implicit operator Vector3(Point3 p)
    {
      return new Vector3(p.x, p.y, p.z);
    }


    public static Point3 operator+ (Point3 p)
    {
      return p;
    }

    // Reflect
    public static Point3 operator- (Point3 p)
    {
      return new Point3(-p.x, -p.y, -p.z);
    }

    // Translation by a vector
    public static Point3 operator +(Point3 p, Vector3 v)
    {
      return new Point3(p.x + v.x, p.y + v.y, p.z + v.z);
    }

    // Translation by a vector
    public static Point3 operator -(Point3 p, Vector3 v)
    {
      return new Point3(p.x - v.x, p.y - v.y, p.z - v.z);
    }

    // A vector between two points
    public static Vector3 operator -(Point3 p, Point3 q)
    {
      return new Vector3(p.x - q.x, p.y - q.y, p.z - q.z);
    }

    // Divide by real for texture scaling
    public static Point3 operator /(Point3 p, double s)
    {
      return new Point3(p.x / s, p.y / s, p.z / s);
    }

    // Multiply by real for texture scaling
    public static Point3 operator *(Point3 p, double s)
    {
      return new Point3(p.x * s, p.y * s, p.z * s);
    }

    // Applying transformations
    public static Point3 operator*(Point3 p, Transform t)
    {
      var d =  p.x * t[0, 3] + p.y * t[1, 3] + p.z * t[2, 3] + t[3, 3];

      if (d < Constant.Epsilon) throw
        new AuroraException("Point * Transform - Invalid matrix");

       return new Point3((p.x * t[0, 0] + p.y * t[1, 0] + p.z * t[2, 0] + t[3, 0]) / d,
                         (p.x * t[0, 1] + p.y * t[1, 1] + p.z * t[2, 1] + t[3, 1]) / d,
                         (p.x * t[0, 2] + p.y * t[1, 2] + p.z * t[2, 2] + t[3, 2]) / d);
    }

    public override string ToString()
    {
      return string.Format("<{0:F3},{1:F3},{2:F3}, 1>", x, y, z);
    }

  }
  #endregion

  #region Colour
  // A colour in RGB format
  public struct Colour
  {
    readonly double red;
    readonly double green;
    readonly double blue;

    // Construct an arbitrary colour
    public Colour(double red, double green, double blue)
    {
      this.red   = Clamp(red);
      this.green = Clamp(green);
      this.blue  = Clamp(blue);
    }

    // Shades of grey (useful for lights)
    public Colour(double greylevel) : this(greylevel, greylevel, greylevel)
    {}

    // Splits a 24-bit colour value into components
    // Mainly intended for use with the colour presets
    public Colour(int preset)
    {
      var bytes = BitConverter.GetBytes(preset);
      red   = bytes[2] / 255.0;
      green = bytes[1] / 255.0;
      blue  = bytes[0] / 255.0;
    }

    // Translate a Color (sic)
    public Colour(System.Drawing.Color c)
    {
      red   = c.R / 255.0;
      green = c.G / 255.0;
      blue  = c.B / 255.0;
    }

    // Rough measure of lightness of a colour to allow trace depth limiting
    public double Lightness()
    {
      return Math.Max(red, Math.Max(green, blue));
    }

    // Add colours (eg. mixing coloured light)
    public static Colour operator +(Colour c, Colour d)
    {
      return new Colour(c.red + d.red, c.green + d.green, c.blue + d.blue);
    }

    // Multiply colours (e.g. coloured light falling on a coloured surface)
    public static Colour operator *(Colour c, Colour d)
    {
      return new Colour(c.red * d.red, c.green * d.green, c.blue * d.blue);
    }

    // Scalar multiply
    public static Colour operator *(Colour c, double d)
    {
      return new Colour(c.red * d, c.green * d, c.blue * d);
    }

    public static Colour operator *(double d, Colour c)
    {
      return new Colour(c.red * d, c.green * d, c.blue * d);
    }

    // Interpolated colour
    public static Colour Interpolate(Colour c, Colour d, double pc)
    {
      if(pc < 0.0) return c;
      if(pc > 1.0) return d;
      return c * pc + d * (1.0 - pc);
    }

    // Automatic conversion to system colours
    // All framework functions will work with Colour as well as Color
    public static implicit operator System.Drawing.Color(Colour c)
    {
      var r = Math.Min(255, (int)(c.red * 255.0));
      var g = Math.Min(255, (int)(c.green * 255.0));
      var b = Math.Min(255, (int)(c.blue * 255.0));
      return System.Drawing.Color.FromArgb(r, g, b);
    }

    // Clamp a colour component to [0.0, 1.0]
    private static double Clamp(double x)
    {
      if((x >= 0.0) && (x <= 1.0))
        return x;
      return x > 0.0 ? 1.0 : 0.0;
    }

    public Colour Gamma(double g)
    {
      if(g == 1.0) return this;
      var lum = 0.3 * red + 0.59 * green + 0.11 * blue;
      var gc = Math.Pow(lum, g)/lum;
      var corr = this * gc;
      return corr;
    }

    #region PresetColours
    public static readonly Colour AliceBlue              = new Colour(0xF0F8FF);
    public static readonly Colour AntiqueWhite           = new Colour(0xFAEBD7);
    public static readonly Colour Aqua                   = new Colour(0x00FFFF);
    public static readonly Colour Aquamarine             = new Colour(0x7FFFD4);
    public static readonly Colour Azure                  = new Colour(0xF0FFFF);
    public static readonly Colour Beige                  = new Colour(0xF5F5DC);
    public static readonly Colour Bisque                 = new Colour(0xFFE4C4);
    public static readonly Colour Black                  = new Colour(0x000000);
    public static readonly Colour BlanchedAlmond         = new Colour(0xFFEBCD);
    public static readonly Colour Blue                   = new Colour(0x0000FF);
    public static readonly Colour BlueViolet             = new Colour(0x8A2BE2);
    public static readonly Colour Brown                  = new Colour(0xA52A2A);
    public static readonly Colour BurlyWood              = new Colour(0xDEB887);
    public static readonly Colour CadetBlue              = new Colour(0x5F9EA0);
    public static readonly Colour Chartreuse             = new Colour(0x7FFF00);
    public static readonly Colour Chocolate              = new Colour(0xD2691E);
    public static readonly Colour Coral                  = new Colour(0xFF7F50);
    public static readonly Colour CornflowerBlue         = new Colour(0x6495ED);
    public static readonly Colour Cornsilk               = new Colour(0xFFF8DC);
    public static readonly Colour Crimson                = new Colour(0xDC143C);
    public static readonly Colour Cyan                   = new Colour(0x00FFFF);
    public static readonly Colour DarkBlue               = new Colour(0x00008B);
    public static readonly Colour DarkCyan               = new Colour(0x008B8B);
    public static readonly Colour DarkGoldenrod          = new Colour(0xB8860B);
    public static readonly Colour DarkGray               = new Colour(0xA9A9A9);
    public static readonly Colour DarkGreen              = new Colour(0x006400);
    public static readonly Colour DarkKhaki              = new Colour(0xBDB76B);
    public static readonly Colour DarkMagenta            = new Colour(0x8B008B);
    public static readonly Colour DarkOliveGreen         = new Colour(0x556B2F);
    public static readonly Colour DarkOrange             = new Colour(0xFF8C00);
    public static readonly Colour DarkOrchid             = new Colour(0x9932CC);
    public static readonly Colour DarkRed                = new Colour(0x8B0000);
    public static readonly Colour DarkSalmon             = new Colour(0xE9967A);
    public static readonly Colour DarkSeaGreen           = new Colour(0x8FBC8F);
    public static readonly Colour DarkSlateBlue          = new Colour(0x483D8B);
    public static readonly Colour DarkSlateGray          = new Colour(0x2F4F4F);
    public static readonly Colour DarkTurquoise          = new Colour(0x00CED1);
    public static readonly Colour DarkViolet             = new Colour(0x9400D3);
    public static readonly Colour DeepPink               = new Colour(0xFF1493);
    public static readonly Colour DeepSkyBlue            = new Colour(0x00BFFF);
    public static readonly Colour DimGray                = new Colour(0x696969);
    public static readonly Colour DodgerBlue             = new Colour(0x1E90FF);
    public static readonly Colour FireBrick              = new Colour(0xB22222);
    public static readonly Colour FloralWhite            = new Colour(0xFFFAF0);
    public static readonly Colour ForestGreen            = new Colour(0x228B22);
    public static readonly Colour Fuchsia                = new Colour(0xFF00FF);
    public static readonly Colour Gainsboro              = new Colour(0xDCDCDC);
    public static readonly Colour GhostWhite             = new Colour(0xF8F8FF);
    public static readonly Colour Gold                   = new Colour(0xFFD700);
    public static readonly Colour Goldenrod              = new Colour(0xDAA520);
    public static readonly Colour Gray                   = new Colour(0x808080);
    public static readonly Colour Green                  = new Colour(0x008000);
    public static readonly Colour GreenYellow            = new Colour(0xADFF2F);
    public static readonly Colour Honeydew               = new Colour(0xF0FFF0);
    public static readonly Colour HotPink                = new Colour(0xFF69B4);
    public static readonly Colour IndianRed              = new Colour(0xCD5C5C);
    public static readonly Colour Indigo                 = new Colour(0x4B0082);
    public static readonly Colour Ivory                  = new Colour(0xFFFFF0);
    public static readonly Colour Khaki                  = new Colour(0xF0E68C);
    public static readonly Colour Lavender               = new Colour(0xE6E6FA);
    public static readonly Colour LavenderBlush          = new Colour(0xFFF0F5);
    public static readonly Colour LawnGreen              = new Colour(0x7CFC00);
    public static readonly Colour LemonChiffon           = new Colour(0xFFFACD);
    public static readonly Colour LightBlue              = new Colour(0xADD8E6);
    public static readonly Colour LightCoral             = new Colour(0xF08080);
    public static readonly Colour LightCyan              = new Colour(0xE0FFFF);
    public static readonly Colour LightGoldenrodYellow   = new Colour(0xFAFAD2);
    public static readonly Colour LightGreen             = new Colour(0x90EE90);
    public static readonly Colour LightGrey              = new Colour(0xD3D3D3);
    public static readonly Colour LightPink              = new Colour(0xFFB6C1);
    public static readonly Colour LightSalmon            = new Colour(0xFFA07A);
    public static readonly Colour LightSeaGreen          = new Colour(0x20B2AA);
    public static readonly Colour LightSkyBlue           = new Colour(0x87CEFA);
    public static readonly Colour LightSlateGray         = new Colour(0x778899);
    public static readonly Colour LightSteelBlue         = new Colour(0xB0C4DE);
    public static readonly Colour LightYellow            = new Colour(0xFFFFE0);
    public static readonly Colour Lime                   = new Colour(0x00FF00);
    public static readonly Colour LimeGreen              = new Colour(0x32CD32);
    public static readonly Colour Linen                  = new Colour(0xFAF0E6);
    public static readonly Colour Magenta                = new Colour(0xFF00FF);
    public static readonly Colour Maroon                 = new Colour(0x800000);
    public static readonly Colour MediumAquamarine       = new Colour(0x66CDAA);
    public static readonly Colour MediumBlue             = new Colour(0x0000CD);
    public static readonly Colour MediumOrchid           = new Colour(0xBA55D3);
    public static readonly Colour MediumPurple           = new Colour(0x9370DB);
    public static readonly Colour MediumSeaGreen         = new Colour(0x3CB371);
    public static readonly Colour MediumSlateBlue        = new Colour(0x7B68EE);
    public static readonly Colour MediumSpringGreen      = new Colour(0x00FA9A);
    public static readonly Colour MediumTurquoise        = new Colour(0x48D1CC);
    public static readonly Colour MediumVioletRed        = new Colour(0xC71585);
    public static readonly Colour MidnightBlue           = new Colour(0x191970);
    public static readonly Colour MColourCream           = new Colour(0xF5FFFA);
    public static readonly Colour MistyRose              = new Colour(0xFFE4E1);
    public static readonly Colour Moccasin               = new Colour(0xFFE4B5);
    public static readonly Colour NavajoWhite            = new Colour(0xFFDEAD);
    public static readonly Colour Navy                   = new Colour(0x000080);
    public static readonly Colour OldLace                = new Colour(0xFDF5E6);
    public static readonly Colour Olive                  = new Colour(0x808000);
    public static readonly Colour OliveDrab              = new Colour(0x6B8E23);
    public static readonly Colour Orange                 = new Colour(0xFFA500);
    public static readonly Colour OrangeRed              = new Colour(0xFF4500);
    public static readonly Colour Orchid                 = new Colour(0xDA70D6);
    public static readonly Colour PaleGoldenrod          = new Colour(0xEEE8AA);
    public static readonly Colour PaleGreen              = new Colour(0x98FB98);
    public static readonly Colour PaleTurquoise          = new Colour(0xAFEEEE);
    public static readonly Colour PaleVioletRed          = new Colour(0xDB7093);
    public static readonly Colour PapayaWhip             = new Colour(0xFFEFD5);
    public static readonly Colour PeachPuff              = new Colour(0xFFDAB9);
    public static readonly Colour Peru                   = new Colour(0xCD853F);
    public static readonly Colour Pink                   = new Colour(0xFFC0CB);
    public static readonly Colour Plum                   = new Colour(0xDDA0DD);
    public static readonly Colour PowderBlue             = new Colour(0xB0E0E6);
    public static readonly Colour Purple                 = new Colour(0x800080);
    public static readonly Colour Red                    = new Colour(0xFF0000);
    public static readonly Colour RosyBrown              = new Colour(0xBC8F8F);
    public static readonly Colour RoyalBlue              = new Colour(0x4169E1);
    public static readonly Colour SaddleBrown            = new Colour(0x8B4513);
    public static readonly Colour Salmon                 = new Colour(0xFA8072);
    public static readonly Colour SandyBrown             = new Colour(0xF4A460);
    public static readonly Colour SeaGreen               = new Colour(0x2E8B57);
    public static readonly Colour Seashell               = new Colour(0xFFF5EE);
    public static readonly Colour Sienna                 = new Colour(0xA0522D);
    public static readonly Colour Silver                 = new Colour(0xC0C0C0);
    public static readonly Colour SkyBlue                = new Colour(0x87CEEB);
    public static readonly Colour SlateBlue              = new Colour(0x6A5ACD);
    public static readonly Colour SlateGray              = new Colour(0x708090);
    public static readonly Colour Snow                   = new Colour(0xFFFAFA);
    public static readonly Colour SpringGreen            = new Colour(0x00FF7F);
    public static readonly Colour SteelBlue              = new Colour(0x4682B4);
    public static readonly Colour Tan                    = new Colour(0xD2B48C);
    public static readonly Colour Teal                   = new Colour(0x008080);
    public static readonly Colour Thistle                = new Colour(0xD8BFD8);
    public static readonly Colour Tomato                 = new Colour(0xFF6347);
    public static readonly Colour Turquoise              = new Colour(0x40E0D0);
    public static readonly Colour Violet                 = new Colour(0xEE82EE);
    public static readonly Colour Wheat                  = new Colour(0xF5DEB3);
    public static readonly Colour White                  = new Colour(0xFFFFFF);
    public static readonly Colour WhiteSmoke             = new Colour(0xF5F5F5);
    public static readonly Colour Yellow                 = new Colour(0xFFFF00);
    public static readonly Colour YellowGreen            = new Colour(0x9ACD32);
    #endregion
  }
  #endregion

  #region Matrix

  /// <summary>
  /// 4x4 matrix used for homogeneous transforms and as
  /// the basis of Quadric equations
  /// </summary>
  public class Matrix : IEquatable<Matrix>
  {
    public double this[int r, int c]
    {
      get { return m[r][c]; }
      set { m[r][c] = value; }
    }
    // Apparently jagged arrays are much faster
    protected readonly double [][] m;

    /// <summary>
    /// Construct a zero matrix
    /// </summary>
    public Matrix()
    {
      m = new double[4][];
      m[0] = new double[4];
      m[1] = new double[4];
      m[2] = new double[4];
      m[3] = new double[4];
    }

    /// <summary>
    /// Construct I * x, where I is the identity transform
    /// </summary>
    /// <param name="x"></param>
    public Matrix(double x)
    {
      m = new double[4][];
      m[0] = new [] {   x, 0.0, 0.0, 0.0 };
      m[1] = new [] { 0.0,   x, 0.0, 0.0 };
      m[2] = new [] { 0.0, 0.0,   x, 0.0 };
      m[3] = new [] { 0.0, 0.0, 0.0,   x };
    }

    /// <summary>
    /// Construct an arbitrary 4*4 matrix
    /// </summary>
    /// <param name="a00">r 0 c 0</param>
    /// <param name="a01">r 0 c 1</param>
    /// <param name="a02">r 0 c 2</param>
    /// <param name="a03">r 0 c 3</param>
    /// <param name="a10">r 1 c 0</param>
    /// <param name="a11">r 1 c 1</param>
    /// <param name="a12">r 1 c 2</param>
    /// <param name="a13">r 1 c 3</param>
    /// <param name="a20">r 2 c 0</param>
    /// <param name="a21">r 2 c 1</param>
    /// <param name="a22">r 2 c 2</param>
    /// <param name="a23">r 2 c 3</param>
    /// <param name="a30">r 3 c 0</param>
    /// <param name="a31">r 3 c 1</param>
    /// <param name="a32">r 3 c 2</param>
    /// <param name="a33">r 3 c 3</param>
    public Matrix(double a00, double a01, double a02, double a03,
                  double a10, double a11, double a12, double a13,
                  double a20, double a21, double a22, double a23,
                  double a30, double a31, double a32, double a33)
    {
      m = new double[4][];
      m[0] = new [] { a00, a01, a02, a03 };
      m[1] = new [] { a10, a11, a12, a13};
      m[2] = new [] { a20, a21, a22, a23};
      m[3] = new [] { a30, a31, a32, a33};
    }

    /// <summary>
    /// Transpose a matrix into another
    /// </summary>
    /// <returns>A transposed copy of this</returns>
    public Matrix Transpose()
    {
      return new Matrix(m[0][0],  m[1][0],  m[2][0],  m[3][0],
                        m[0][1],  m[1][1],  m[2][1],  m[3][1],
                        m[0][2],  m[1][2],  m[2][2],  m[3][2],
                        m[0][3],  m[1][3],  m[2][3],  m[3][3]);
    }

    /// <summary>
    /// Calculate the adjoint matrix
    /// </summary>
    /// <returns>The adjoint matrix</returns>
    public Matrix Adjoint()
    {
      return new Matrix( Det(m[1][1], m[1][2], m[1][3],
                             m[2][1], m[2][2], m[2][3],
                             m[3][1], m[3][2], m[3][3]),

                        -Det(m[0][1], m[0][2], m[0][3],
                             m[2][1], m[2][2], m[2][3],
                             m[3][1], m[3][2], m[3][3]),

                         Det(m[0][1], m[0][2], m[0][3],
                             m[1][1], m[1][2], m[1][3],
                             m[3][1], m[3][2], m[3][3]),

                        -Det(m[0][1], m[0][2], m[0][3],
                             m[1][1], m[1][2], m[1][3],
                             m[2][1], m[2][2], m[2][3]),

                        -Det(m[1][0], m[1][2], m[1][3],
                             m[2][0], m[2][2], m[2][3],
                             m[3][0], m[3][2], m[3][3]),

                         Det(m[0][0], m[0][2], m[0][3],
                             m[2][0], m[2][2], m[2][3],
                             m[3][0], m[3][2], m[3][3]),

                        -Det(m[0][0], m[0][2], m[0][3],
                             m[1][0], m[1][2], m[1][3],
                             m[3][0], m[3][2], m[3][3]),

                         Det(m[0][0], m[0][2], m[0][3],
                             m[1][0], m[1][2], m[1][3],
                             m[2][0], m[2][2], m[2][3]),

                         Det(m[1][0], m[1][1], m[1][3],
                             m[2][0], m[2][1], m[2][3],
                             m[3][0], m[3][1], m[3][3]),

                        -Det(m[0][0], m[0][1], m[0][3],
                             m[2][0], m[2][1], m[2][3],
                             m[3][0], m[3][1], m[3][3]),

                         Det(m[0][0], m[0][1], m[0][3],
                             m[1][0], m[1][1], m[1][3],
                             m[3][0], m[3][1], m[3][3]),

                        -Det(m[0][0], m[0][1], m[0][3],
                             m[1][0], m[1][1], m[1][3],
                             m[2][0], m[2][1], m[2][3]),

                        -Det(m[1][0], m[1][1], m[1][2],
                             m[2][0], m[2][1], m[2][2],
                             m[3][0], m[3][1], m[3][2]),

                         Det(m[0][0], m[0][1], m[0][2],
                             m[2][0], m[2][1], m[2][2],
                             m[3][0], m[3][1], m[3][2]),

                        -Det(m[0][0], m[0][1], m[0][2],
                             m[1][0], m[1][1], m[1][2],
                             m[3][0], m[3][1], m[3][2]),

                         Det(m[0][0], m[0][1], m[0][2],
                             m[1][0], m[1][1], m[1][2],
                             m[2][0], m[2][1], m[2][2]));
    }

    /// <summary>
    ///  Matrix multiplicative inverse, useful to invert a transform
    /// </summary>
    /// <returns></returns>
    public Matrix Inverse()
    {
      var i = Adjoint();

      // The adjoint kindly provides all required 3*3 determinants
      var det = m[0][0] * i.m[0][0] + m[0][1] * i.m[1][0] +
                   m[0][2] * i.m[2][0] + m[0][3] * i.m[3][0];

      if(det < Constant.Epsilon) throw
        new AuroraException("Matrix::Invert() Singular matrix");

      return i * (1.0 / det);
    }

    /// <summary>
    /// Matrix product used to combine transforms
    /// </summary>
    /// <param name="t">Multiplicand</param>
    /// <param name="u">Multiplyer</param>
    /// <returns>Product matrix</returns>
    public static Matrix operator *(Matrix t, Matrix u)
    {
      var r = new Matrix();
      for(var i = 0; i < 4; i++)
      {
        for(var j = 0; j < 4; j++)
        {
           r.m[i][j] = t.m[i][0] * u.m[0][j] +
                       t.m[i][1] * u.m[1][j] +
                       t.m[i][2] * u.m[2][j] +
                       t.m[i][3] * u.m[3][j];
        }
      }
      return r;
    }

    /// <summary>
    /// Simple element by element multiply for matrix * scalar
    /// </summary>
    /// <param name="t">Multiplicand</param>
    /// <param name="u">Scalar multiplyer</param>
    /// <returns></returns>
    public static Matrix operator *(Matrix t, double u)
    {
      var r = new Matrix();
      for(var i = 0; i < 4; i++)
      {
        r.m[i][0] = u * t.m[i][0];
        r.m[i][1] = u * t.m[i][1];
        r.m[i][2] = u * t.m[i][2];
        r.m[i][3] = u * t.m[i][3];
      }

      return r;
    }

    /// <summary>
    /// Essentially useless
    /// </summary>
    /// <param name="t">Matrix</param>
    /// <returns>Matrix</returns>
    public static Matrix operator +(Matrix t)
    {
      return t;
    }

    /// <summary>
    /// Returns the additive inverse of a matrix.
    /// </summary>
    /// <param name="t">Matrix to invert</param>
    /// <returns>Negative</returns>
    public static Matrix operator -(Matrix t)
    {
      return new Matrix(-t.m[0][0], -t.m[0][1], -t.m[0][2], -t.m[0][3],
                        -t.m[1][0], -t.m[1][1], -t.m[1][2], -t.m[1][3],
                        -t.m[2][0], -t.m[2][1], -t.m[2][2], -t.m[2][3],
                        -t.m[3][0], -t.m[3][1], -t.m[3][2], -t.m[3][3]);

    }
    // 2x2 determinant
    public static double Det(double a, double b, double c, double d)
    {
      return(a * d - b * c);
    }

    // 3x3 determinant
    public static double Det(double m00, double m01, double m02,
                             double m10, double m11, double m12,
                             double m20, double m21, double m22)
    {
      return(m00 * Det(m11, m12, m21, m22) -
             m01 * Det(m10, m12, m20, m22) +
             m02 * Det(m10, m11, m20, m21));
    }

    /// <summary>
    /// Compare matrices for a close numerical match rather than
    /// identity.  This function is only here to support the unit
    /// tests.
    /// </summary>
    /// <param name="obj">Object to compare with</param>
    /// <returns></returns>
    public override bool Equals(object obj)
    {
      return Equals(obj as Matrix);
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public override int  GetHashCode()
    {
      return m[0].GetHashCode() ^
             m[1].GetHashCode() ^
             m[2].GetHashCode() ^
             m[3].GetHashCode();
    }

    #region IEquatable<Matrix> Members

    public bool Equals(Matrix other)
    {
      if(other == null)
        return false;

      return (Math.Abs(m[0][0] - other.m[0][0]) < Constant.Fuzz) &&
             (Math.Abs(m[0][1] - other.m[0][1]) < Constant.Fuzz) &&
             (Math.Abs(m[0][2] - other.m[0][2]) < Constant.Fuzz) &&
             (Math.Abs(m[0][3] - other.m[0][3]) < Constant.Fuzz) &&
             (Math.Abs(m[1][0] - other.m[1][0]) < Constant.Fuzz) &&
             (Math.Abs(m[1][1] - other.m[1][1]) < Constant.Fuzz) &&
             (Math.Abs(m[1][2] - other.m[1][2]) < Constant.Fuzz) &&
             (Math.Abs(m[1][3] - other.m[1][3]) < Constant.Fuzz) &&
             (Math.Abs(m[2][0] - other.m[2][0]) < Constant.Fuzz) &&
             (Math.Abs(m[2][1] - other.m[2][1]) < Constant.Fuzz) &&
             (Math.Abs(m[2][2] - other.m[2][2]) < Constant.Fuzz) &&
             (Math.Abs(m[2][3] - other.m[2][3]) < Constant.Fuzz) &&
             (Math.Abs(m[3][0] - other.m[3][0]) < Constant.Fuzz) &&
             (Math.Abs(m[3][1] - other.m[3][1]) < Constant.Fuzz) &&
             (Math.Abs(m[3][2] - other.m[3][2]) < Constant.Fuzz) &&
             (Math.Abs(m[3][3] - other.m[3][3]) < Constant.Fuzz);
    }

    #endregion

    public override string ToString()
    {
      return string.Format("[[{0:F2},{1:F2},{2:F2},{3:F2}][{4:F2},{5:F2},{6:F2},{7:F2}][{8:F2},{9:F2},{10:F2},{11:F2}][{12:F2},{13:F2},{14:F2},{15:F2}]]",
                    m[0][0], m[0][1], m[0][2], m[0][3],
                    m[1][0], m[1][1], m[1][2], m[1][3],
                    m[2][0], m[2][1], m[2][2], m[2][3],
                    m[3][0], m[3][1], m[3][2], m[3][3]);
    }
}
  #endregion

  #region Transform
  /// <summary>
  /// A homogeneous transform in a 4x4 matrix.
  /// </summary>
  public class Transform : Matrix
  {
    private readonly Matrix matrix;
    // Constructors for the basic transformations

    /// <summary>
    /// Construct an identity transform
    /// </summary>
    public Transform() : base(1.0) { }

    /// <summary>
    /// Construct a rotation about an axis
    /// </summary>
    /// <param name="a">An Axis</param>
    /// <param name="amount">The rotation angle in degrees</param>
    public Transform(Axis a, double amount) : base(1.0)
    {
      Rotate(a, amount);
    }

    /// <summary>
    /// Construct a translation
    /// </summary>
    /// <param name="v">The vector that specifies the translation</param>
    public Transform(Vector3 v) : base(1.0)
    {
      Translate(v);
    }

    /// <summary>
    /// Construct a scaling
    /// </summary>
    /// <param name="sx">X axis scale factor</param>
    /// <param name="sy">Y axis scale factor</param>
    /// <param name="sz">Z axis scale factor</param>
    public Transform(double sx, double sy, double sz) : base(1.0)
    {
      Scale(sx, sy, sz);
    }

    /// <summary>
    /// Construct a uniform scaling
    /// </summary>
    /// <param name="s">The uniform scale factor</param>
    public Transform(double s) : base(1.0)
    {
      Scale(s);
    }

    /// <summary>
    /// Construct an axis aligned shear transformation centred at the origin.
    /// First read the parameter comments below.
    /// If a shear displacement, s, is required in the X-Axis for each
    /// distance d, travelled along the Z-Axis then use
    /// <code>new Transform(Axis.X, s, Axis.Z, d);</code>
    /// Note that the shear angle will be Atan2(s, d)
    /// </summary>
    /// <param name="shearAxis">The axis parallel to the shear displacement</param>
    /// <param name="shear">The 'per unit' displacement</param>
    /// <param name="shearNormal">The axis normal to the shear slip planes</param>
    /// <param name="scale">Defines the unit distance along the shear normal axis</param>
    public Transform(Axis shearAxis, double shear, Axis shearNormal, double scale) : base(1.0)
    {
      if((scale == 0.0) || (shearAxis == shearNormal))
        throw new AuroraException("Illegal shear specification");

      m[(int)shearNormal][(int)shearAxis] = shear / scale;
    }

    public Transform(Matrix matrix)
    {
      this.matrix = matrix;
    }

    // The following transformation operations allow the construction
    // of more complex transformations by post-concatenation.
    //
    // This just means that the concatenated transformation
    // will be (effectively) appended to the list of transformations
    // held by the Transform rather than being prepended or inserted.
    // In practice, there is no actual list of transformations
    // since the cumulative Transform is calculated (by means
    // of a matrix multiply) as each transformation is added.
    // Post-concatenating a transform directly corresponds to post-
    // multiplication of the cumulative matrix by the concatenated
    // one.

    /// <summary>
    /// Post concatenate a translation
    /// </summary>
    /// <param name="dx">X axis translation</param>
    /// <param name="dy">Y axis translation</param>
    /// <param name="dz">Z axis translation</param>

    public void Translate(double dx, double dy, double dz)
    {
      for(var i = 0; i < 4; i++)
      {
        m[i][0] += m[i][3] * dx;
        m[i][1] += m[i][3] * dy;
        m[i][2] += m[i][3] * dz;
      }
    }

    /// <summary>
    /// Post concatenate a translation
    /// </summary>
    /// <param name="v">A vector that specifies the translation</param>
    public void Translate(Vector3 v)
    {
      for(var i = 0; i < 4; i++)
      {
        m[i][0] += m[i][3] * v.x;
        m[i][1] += m[i][3] * v.y;
        m[i][2] += m[i][3] * v.z;
      }
    }

    /// <summary>
    /// Post concatenate a rotation about an axis
    /// </summary>
    /// <param name="a"></param>
    /// <param name="amount"></param>
    public void Rotate(Axis a, double amount)
    {
      amount *= Math.PI / 180.0;  // Convert to radians

      var s = Math.Sin(amount);
      var c = Math.Cos(amount);
      double t;
      switch(a)
      {
        case Axis.X:
          for(var i = 0; i < 4; i++)
          {
            t = m[i][1];
            m[i][1] = t * c + m[i][2] * s;
            m[i][2] = -t * s + m[i][2] * c;
          }
          break;

        case Axis.Y:
          for(var i = 0; i < 4; i++)
          {
            t = m[i][0];
            m[i][0] = t * c - m[i][2] * s;
            m[i][2] = t * s + m[i][2] * c;
          }
          break;

        case Axis.Z:
          for(var i = 0; i < 4; i++)
          {
            t = m[i][0];
            m[i][0] = t * c + m[i][1] * s;
            m[i][1] = -t * s + m[i][1] * c;
          }
          break;
      }
    }

    /// <summary>
    /// Post concatenate an Axis aligned shear
    /// </summary>
    /// <param name="shearAxis">The axis parallel to the shear displacement</param>
    /// <param name="shear">The 'per unit' displacement</param>
    /// <param name="shearNormal">The axis normal to the shear slip planes</param>
    /// <param name="scale">Defines the unit distance along the shear normal axis</param>
    public void Shear(Axis shearAxis, double shear, Axis shearNormal, double scale)
    {
      if((scale == 0.0) || (shearAxis == shearNormal))
        throw new AuroraException("Illegal shear specification");

      var sn = (int)shearNormal;
      var sa = (int)shearAxis;

      var r = shear / scale;

      m[0][sa] += r * m[0][sn];
      m[1][sa] += r * m[1][sn];
      m[2][sa] += r * m[2][sn];
      m[3][sa] += r * m[3][sn];
    }


    /// <summary>
    /// Non uniform scaling
    /// </summary>
    /// <param name="sx">X Axis scale factor</param>
    /// <param name="sy">Y Axis scale factor</param>
    /// <param name="sz">Z Axis scale factor</param>

    public void Scale(double sx, double sy, double sz)
    {
      for(var i = 0; i < 4; i++)
      {
        m[i][0] *= sx;
        m[i][1] *= sy;
        m[i][2] *= sz;
      }
    }

    /// <summary>
    /// Uniform scaling
    /// </summary>
    /// <param name="s">Scale factor</param>
    public void Scale(double s)
    {
      for(var i = 0; i < 4; i++)
      {
        m[i][0] *= s;
        m[i][1] *= s;
        m[i][2] *= s;
      }
    }

    /// <summary>
    /// Post-concatenate a transform
    /// </summary>
    /// <param name="t">Transform to be post-concatenated</param>
    public void Apply(Transform t)
    {
      var r = new Matrix();
      for(var i = 0; i < 4; i++)
      {
        for(var j = 0; j < 4; j++)
        {
          r[i, j] = m[i][0] * t.m[0][j] +
                    m[i][1] * t.m[1][j] +
                    m[i][2] * t.m[2][j] +
                    m[i][3] * t.m[3][j] ;
        }
        for(var j = 0; j < 4; j++)
          m[i][j] = r[i, j];
      }
    }
  }
#endregion

  #region Ray
  /// <summary>
  /// A semi-infinite line with a vector direction and a point as origin
  /// The entire line is defined parametrically as:  origin + t * direction
  /// The ray parameter, t, is the distance along the ray from the its origin.
  /// t is positive (by definition) and so the ray is that part of  the line
  /// with a positive ray parameter.
  ///
  /// Ray tracer rays have little in common with light rays.  They originate
  /// at the camera and extend outwards to 'feel' the scene geometry.
  /// Rays are always straight, never reflected or refracted.  Additional
  /// child rays are created as required to trace each linear segment of the
  /// path that a laser beam would take (in reverse!).
  /// Rays are also used for shadow calculations.  The 'length' of a shadow ray
  /// is defined as the distance from a point on an object to the light source
  /// being tested.
  /// </summary>
  public struct Ray
  {
    Point3    origin;
    Vector3   direction;
    double    length;

    /// <summary>
    /// Basic constructor
    /// </summary>
    /// <param name="origin">Ray origin</param>
    /// <param name="direction">Ray direction</param>
    public Ray(Point3 origin, Vector3 direction)
    {
      this.origin = origin;
      length = direction.Norm();
      this.direction = direction / length;
    }

    /// <summary>
    /// Construct ray that intersects a specific point
    /// </summary>
    /// <param name="origin">Ray origin</param>
    /// <param name="lookAt">Point looked at</param>
    public Ray(Point3 origin, Point3 lookAt)
    {
      this.origin = origin;
      direction = lookAt - origin;
      length = direction.Norm();
      direction = direction / length;
    }

    /// <summary>
    /// The ray origin
    /// </summary>
    public Point3 Origin
    {
      get { return origin; }
    }

    /// <summary>
    /// The ray direction vector
    /// </summary>
    public Vector3 Direction
    {
      get { return direction; }
    }

    /// <summary>
    /// The distance to the lookAt point
    /// Normally the location of a light for shadow testing
    /// </summary>
    public double Length
    {
      get { return length; }
    }

    /// <summary>
    /// Calculates the point on the ray at parameter t
    /// where t is distance from the ray origin
    /// </summary>
    /// <param name="t">Ray parameter</param>
    /// <returns>Point on ray</returns>
    public Point3 At(double t)  // Point on ray at parameter t
    {
      return origin + direction * t;
    }
  }
  #endregion
}
