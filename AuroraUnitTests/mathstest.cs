using System.Drawing;
using System;
using Aurora;
using Microsoft.VisualStudio.QualityTools.UnitTesting.Framework;

 
namespace AuroraUnitTests
{
  /// <summary>
  ///This is a test class for Aurora.Vector and is intended
  ///to contain all Aurora.Vector Unit Tests
  ///</summary>
  [TestClass()]
  public class Vector3Test
  {


    private TestContext testContextInstance;

    /// <summary>
    ///Gets or sets the test context which provides
    ///information about and functionality for the current test run.
    ///</summary>
    public TestContext TestContext
    {
      get { return testContextInstance; }
      set { testContextInstance = value; }
    }

    Vector3 v1;
    Vector3 v2;
    Vector3 v3;

    /// <summary>
    ///Initialize() is called once during test execution before
    ///test methods in this test class are executed.
    ///</summary>
    [TestInitialize()]
    public void Initialize()
    {
       v1 = new Vector3(1.0, 2.0, 3.0);
       v2 = new Vector3(-1.0, 2.0, 1.0);
       v3 = new Vector3(6.0, 4.0, 5.0);
    }

    /// <summary>
    ///Cleanup() is called once during test execution after
    ///test methods in this class have executed unless
    ///this test class' Initialize() method throws an exception.
    ///</summary>
    [TestCleanup()]
    public void Cleanup()
    {
      //  TODO: Add test cleanup code
    }


    /// <summary>
    ///A test case for Addition
    ///</summary>
    [TestMethod()]
    public void AdditionTest()
    {

      Vector3 expected = new Vector3(0.0, 4.0, 4.0);
      Vector3 actual = v1 + v2;

      Assert.AreEqual(expected, actual, "Aurora.Vector3.operator + did not return the expected value.");
    }

    /// <summary>
    ///A test case for Cross Product
    ///</summary>
    [TestMethod()]
    public void ExclusiveOrTest()
    {
      Vector3 expected = new Vector3(-4.0, -4.0,  4.0);
      Vector3 actual = v1 ^ v2;

      Assert.AreEqual(expected, actual, "Aurora.Vector3.operator ^ did not return the expected value.");
    }

    /// <summary>
    ///A test case for Multiply
    ///</summary>
    [TestMethod()]
    public void MultiplyTest()
    {
      double r = 3.0; 

      Vector3 expected = new Vector3(3.0, 6.0, 9.0);
      Vector3 actual = v1 * r;

      Assert.AreEqual(expected, actual, "Aurora.Vector3.operator * did not return the expected value.");

      actual = r * v1;

      Assert.AreEqual(expected, actual, "Aurora.Vector3.operator * did not return the expected value.");

    }

    /// <summary>
    ///A test case for Multiply
    ///</summary>
    [TestMethod()]
    public void MultiplyTest1()
    {
      // Note that the vector should be rotated
      Transform t = new Transform(Axis.Z, 90.0);
      // but not translated
      t.Translate(v3);

      Vector3 expected = new Vector3(2, -1, 1);
      Vector3 actual = v2 * t; 

      Assert.AreEqual(expected, actual, "Aurora.Vector3.operator * did not return the expected value.");
    }

    /// <summary>
    ///A test case for Multiply
    ///</summary>
    [TestMethod()]
    public void MultiplyTest3()
    {
      double expected = 6.0;
      double actual = v1 * v2;

      Assert.AreEqual(expected, actual, "Aurora.Vector3.operator * did not return the expected value.");
    }

    /// <summary>
    ///A test case for MultiplyElements (Vector, Vector)
    ///</summary>
    [TestMethod()]
    public void MultiplyElementsTest()
    {
      Vector3 expected = new Vector3(-1.0, 4.0, 3.0);
      Vector3 actual = Aurora.Vector3.MultiplyElements(v1, v2);

      Assert.AreEqual(expected, actual, "Aurora.Vector3.MultiplyElements did not return the expected value.");
    }

    /// <summary>
    ///A test case for Negate ()
    ///</summary>
    [TestMethod()]
    public void NegateTest()
    {
      Vector3 expected = v1 * -1.0;
      Vector3 target = v1;
      target.Negate();

      Assert.AreEqual(expected, target, "Aurora.Vector3.Negate did not return the expected value.");
    }

    /// <summary>
    ///A test case for Norm ()
    ///</summary>
    [TestMethod()]
    public void NormTest()
    {
      Vector3 target = v3;
      double expected =  Math.Sqrt(77.0);
      double actual = target.Norm();

      Assert.AreEqual(expected, actual, "Aurora.Vector3.Norm did not return the expected value.");
      Assert.AreEqual(v3, target, "Aurora.Vector3.Norm did not return the expected value.");
    }

    /// <summary>
    ///A test case for Normalise ()
    ///</summary>
    [TestMethod()]
    public void NormaliseTest()
    {
      Vector3 target = v1;

      double expected = Math.Sqrt(14.0);
      double actual = target.Normalise();

      Assert.AreEqual(expected, actual, "Aurora.Vector3.Normalise did not return the expected value.");
      Assert.AreEqual(v3 * (1.0 / expected), target, "Aurora.Vector3.Norm did not return the expected value.");
    }

    /// <summary>
    ///A test case for Subtraction
    ///</summary>
    [TestMethod()]
    public void SubtractionTest()
    {
      Vector3 expected = new Vector3(2.0, 0.0, -1.0);
      Vector3 actual = Vector3.UnitX * 2 - Vector3.UnitZ;

      Assert.AreEqual(expected, actual, "Aurora.Vector3.operator - did not return the expected value.");
    }

    /// <summary>
    ///A test case for UnaryNegation
    ///</summary>
    [TestMethod()]
    public void UnaryNegationTest()
    {
      Vector3 v = new Vector3(1.0, -2.0, 3.0);
      Vector3 expected = new Vector3(-1.0, 2.0, -3.0);

      Vector3 actual = -v;
      Assert.AreEqual(expected, actual, "Aurora.Vector.operator - did not return the expected value.");
    }

    ///<summary>
    /// A test case for Vector(double, double, double)
    ///</summary>
    [TestMethod()]
    public void ConstructorTest1()
    {
      double x = 1.0;
      double y = 2.0;
      double z = 3.0;

      Vector3 actual = new Vector3(x, y, z);
      Assert.AreEqual(v1, actual, "Aurora.Vector Constructor - did not return the expected value.");
    }
  }

  /// <summary>
  ///This is a test class for Aurora.Point3 and is intended
  ///to contain all Aurora.Point3 Unit Tests
  ///</summary>
  [TestClass()]
  public class Point3Test
  {
    private TestContext testContextInstance;

    /// <summary>
    ///Gets or sets the test context which provides
    ///information about and functionality for the current test run.
    ///</summary>
    public TestContext TestContext
    {
      get { return testContextInstance; }
      set { testContextInstance = value; }
    }

    Point3 p1;
    Point3 p2;
    Point3 p3;

    Vector3 v1;
    Vector3 v2;
    Vector3 v3;

      
    /// <summary>
    ///Initialize() is called once during test execution before
    ///test methods in this test class are executed.
    ///</summary>
    [TestInitialize()]
    public void Initialize()
    {
       p1 = new Point3(1.0, 2.0, 3.0);
       p2 = new Point3(-1.0, 2.0, 1.0);
       p3 = new Point3(6.0, 4.0, 5.0);

       v1 = new Vector3(1.0, 2.0, 3.0);
       v2 = new Vector3(-1.0, 2.0, 1.0);
       v3 = new Vector3(6.0, 4.0, 5.0);
    }

    /// <summary>
    ///Cleanup() is called once during test execution after
    ///test methods in this class have executed unless
    ///this test class' Initialize() method throws an exception.
    ///</summary>
    [TestCleanup()]
    public void Cleanup()
    {
      //  TODO: Add test cleanup code
    }


    /// <summary>
    ///A test case for Addition
    ///</summary>
    [TestMethod()]
    public void AdditionTest()
    {
      Point3 expected = new Point3(0.0, 4.0, 4.0);
      Point3 actual = p1 + p2;

      Assert.AreEqual(expected, actual, "Aurora.Vertex.operator + did not return the expected value.");
    }

    /// <summary>
    ///A test case for Conversion from a vector
    ///</summary>
    [TestMethod()]
    public void ConversionTest()
    {
      Vector3 actual = p1;

      Assert.AreEqual(v1, actual, "Aurora.Vertex.implicit operator did not return the expected value.");
    }

    /// <summary>
    ///A test case for Division
    ///</summary>
    [TestMethod()]
    public void DivisionTest()
    {
      double s = 2.0;

      Point3 expected = new Point3(0.5, 1.0, 1.5);
      Point3 actual = p1 / s;

      Assert.AreEqual(expected, actual, "Aurora.Vertex.operator / did not return the expected value.");
    }

    /// <summary>
    ///A test case for Multiply
    ///</summary>
    [TestMethod()]
    public void MultiplyTest()
    {
      Point3 p = new Point3(); // TODO: Initialize to an appropriate value

      Transform t = null; // TODO: Initialize to an appropriate value

      Point3 expected = new Point3();
      Point3 actual;

      actual = p * t;

      Assert.AreEqual(expected, actual, "Aurora.Vertex.operator * did not return the expected value.");
      Assert.Inconclusive("Verify the correctness of this test method.");
    }

    /// <summary>
    ///A test case for Multiply
    ///</summary>
    [TestMethod()]
    public void MultiplyTest1()
    {
      Point3 p = new Point3(); // TODO: Initialize to an appropriate value

      double s = 0; // TODO: Initialize to an appropriate value

      Point3 expected = new Point3();
      Point3 actual;

      actual = p * s;

      Assert.AreEqual(expected, actual, "Aurora.Vertex.operator * did not return the expected value.");
      Assert.Inconclusive("Verify the correctness of this test method.");
    }

    /// <summary>
    ///A test case for Negate ()
    ///</summary>
    [TestMethod()]
    public void NegateTest()
    {
      double x = 0; // TODO: Initialize to an appropriate value

      double y = 0; // TODO: Initialize to an appropriate value

      double z = 0; // TODO: Initialize to an appropriate value

      Point3 target = new Point3(x, y, z);

      Point3 expected = new Point3();
      Point3 actual;

      actual = target.Negate();

      Assert.AreEqual(expected, actual, "Aurora.Vertex.Negate did not return the expected value.");
      Assert.Inconclusive("Verify the correctness of this test method.");
    }

    /// <summary>
    ///A test case for Vertex (double, double, double)
    ///</summary>
    [TestMethod()]
    public void ConstructorTest()
    {
      double x = 0; // TODO: Initialize to an appropriate value

      double y = 0; // TODO: Initialize to an appropriate value

      double z = 0; // TODO: Initialize to an appropriate value

      Point3 target = new Point3(x, y, z);

      // TODO: Implement code to verify target
      Assert.Inconclusive("TODO: Implement code to verify target");
    }

    /// <summary>
    ///A test case for Subtraction
    ///</summary>
    [TestMethod()]
    public void SubtractionTest()
    {
      Point3 p = new Point3(); // TODO: Initialize to an appropriate value

      Point3 q = new Point3(); // TODO: Initialize to an appropriate value

      Vector3 expected = new Vector3();
      Vector3 actual;

      actual = p - q;

      Assert.AreEqual(expected, actual, "Aurora.Vertex.operator - did not return the expected value.");
      Assert.Inconclusive("Verify the correctness of this test method.");
    }

    /// <summary>
    ///A test case for Subtraction
    ///</summary>
    [TestMethod()]
    public void SubtractionTest1()
    {
      Point3 p = new Point3(); // TODO: Initialize to an appropriate value

      Vector3 v = new Vector3(); // TODO: Initialize to an appropriate value

      Point3 expected = new Point3();
      Point3 actual;

      actual = p - v;

      Assert.AreEqual(expected, actual, "Aurora.Vertex.operator - did not return the expected value.");
      Assert.Inconclusive("Verify the correctness of this test method.");
    }

    /// <summary>
    ///A test case for UnaryNegation
    ///</summary>
    [TestMethod()]
    public void UnaryNegationTest()
    {
      Point3 p = new Point3(); // TODO: Initialize to an appropriate value

      Point3 expected = new Point3();
      Point3 actual;

      actual = -p;

      Assert.AreEqual(expected, actual, "Aurora.Vertex.operator - did not return the expected value.");
      Assert.Inconclusive("Verify the correctness of this test method.");
    }

    /// <summary>
    ///A test case for UnaryPlus
    ///</summary>
    [TestMethod()]
    public void UnaryPlusTest()
    {
      Point3 p = new Point3(); // TODO: Initialize to an appropriate value

      Point3 expected = new Point3();
      Point3 actual;

      actual = +p;

      Assert.AreEqual(expected, actual, "Aurora.Vertex.operator + did not return the expected value.");
      Assert.Inconclusive("Verify the correctness of this test method.");
    }

  }
  /// <summary>
  ///This is a test class for Aurora.Ray and is intended
  ///to contain all Aurora.Ray Unit Tests
  ///</summary>
  [TestClass()]
  public class RayTest
  {


    private TestContext testContextInstance;

    /// <summary>
    ///Gets or sets the test context which provides
    ///information about and functionality for the current test run.
    ///</summary>
    public TestContext TestContext
    {
      get
      {
        return testContextInstance;
      }
      set
      {
        testContextInstance = value;
      }
    }

    /// <summary>
    ///Initialize() is called once during test execution before
    ///test methods in this test class are executed.
    ///</summary>
    [TestInitialize()]
    public void Initialize()
    {
      //  TODO: Add test initialization code
    }

    /// <summary>
    ///Cleanup() is called once during test execution after
    ///test methods in this class have executed unless
    ///this test class' Initialize() method throws an exception.
    ///</summary>
    [TestCleanup()]
    public void Cleanup()
    {
      //  TODO: Add test cleanup code
    }


    /// <summary>
    ///A test case for At (double)
    ///</summary>
    [TestMethod()]
    public void AtTest()
    {
      Point3 origin = new Point3(); // TODO: Initialize to an appropriate value

      Point3 lookAt = new Point3(); // TODO: Initialize to an appropriate value

      Ray target = new Ray(origin, lookAt);

      double t = 0; // TODO: Initialize to an appropriate value

      Point3 expected = new Point3();
      Point3 actual;

      actual = target.At(t);

      Assert.AreEqual(expected, actual, "Aurora.Ray.At did not return the expected value.");
      Assert.Inconclusive("Verify the correctness of this test method.");
    }

    /// <summary>
    ///A test case for Direction
    ///</summary>
    [TestMethod()]
    public void DirectionTest()
    {
      Point3 origin = new Point3(); // TODO: Initialize to an appropriate value

      Point3 lookAt = new Point3(); // TODO: Initialize to an appropriate value

      Ray target = new Ray(origin, lookAt);

      Vector3 val = new Vector3(); // TODO: Assign to an appropriate value for the property


      Assert.AreEqual(val, target.Direction, "Aurora.Ray.Direction was not set correctly.");
      Assert.Inconclusive("Verify the correctness of this test method.");
    }

    /// <summary>
    ///A test case for Length
    ///</summary>
    [TestMethod()]
    public void LengthTest()
    {
      Point3 origin = new Point3(); // TODO: Initialize to an appropriate value

      Point3 lookAt = new Point3(); // TODO: Initialize to an appropriate value

      Ray target = new Ray(origin, lookAt);

      double val = 0; // TODO: Assign to an appropriate value for the property


      Assert.AreEqual(val, target.Length, "Aurora.Ray.Length was not set correctly.");
      Assert.Inconclusive("Verify the correctness of this test method.");
    }

    /// <summary>
    ///A test case for Origin
    ///</summary>
    [TestMethod()]
    public void OriginTest()
    {
      Point3 origin = new Point3(); // TODO: Initialize to an appropriate value

      Point3 lookAt = new Point3(); // TODO: Initialize to an appropriate value

      Ray target = new Ray(origin, lookAt);

      Point3 val = new Point3(); // TODO: Assign to an appropriate value for the property


      Assert.AreEqual(val, target.Origin, "Aurora.Ray.Origin was not set correctly.");
      Assert.Inconclusive("Verify the correctness of this test method.");
    }

    /// <summary>
    ///A test case for Ray (Vertex, Vertex)
    ///</summary>
    [TestMethod()]
    public void ConstructorTest()
    {
      Point3 origin = new Point3(); // TODO: Initialize to an appropriate value

      Point3 lookAt = new Point3(); // TODO: Initialize to an appropriate value

      Ray target = new Ray(origin, lookAt);

      // TODO: Implement code to verify target
      Assert.Inconclusive("TODO: Implement code to verify target");
    }

    /// <summary>
    ///A test case for Ray (Vertex, Vector)
    ///</summary>
    [TestMethod()]
    public void ConstructorTest1()
    {
      Point3 origin = new Point3(); // TODO: Initialize to an appropriate value

      Vector3 direction = new Vector3(); // TODO: Initialize to an appropriate value

      Ray target = new Ray(origin, direction);

      // TODO: Implement code to verify target
      Assert.Inconclusive("TODO: Implement code to verify target");
    }

  }
  /// <summary>
  ///This is a test class for Aurora.Matrix and is intended
  ///to contain all Aurora.Matrix Unit Tests
  ///</summary>
  [TestClass()]
  public class MatrixTest
  {
    private TestContext testContextInstance;

    /// <summary>
    ///Gets or sets the test context which provides
    ///information about and functionality for the current test run.
    ///</summary>
    public TestContext TestContext
    {
      get { return testContextInstance; }
      set { testContextInstance = value; }
    }

    Matrix mat;
    Matrix mat2;

    /// <summary>
    ///Initialize() is called once during test execution before
    ///test methods in this test class are executed.
    ///</summary>
    [TestInitialize()]
    public void Initialize()
    {
      mat = new Matrix(3.0, 2.0, 1.0, 4.0,
                       1.0, 3.0, 2.0, 3.0,
                       2.0, 4.0, 3.0, 2.0,
                       1.0, 3.0, 4.0, 1.0);
               
      mat2 = new Matrix(17.0, 28.0, 26.0, 24.0,
                        13.0, 28.0, 25.0, 20.0,
                        18.0, 34.0, 27.0, 28.0,
                        15.0, 30.0, 23.0, 22.0);
  
    }

    /// <summary>
    ///Cleanup() is called once during test execution after
    ///test methods in this class have executed unless
    ///this test class' Initialize() method throws an exception.
    ///</summary>
    [TestCleanup()]
    public void Cleanup()
    {
      //  TODO: Add test cleanup code
    }


    /// <summary>
    ///A test case for Adjoint ()
    ///</summary>
    [TestMethod()]
    public void AdjointTest()
    {
      Matrix expected = new Matrix( 10.0, -20.0,  10.0,  -0.0,
                                   -10.0,   5.0,  20.0, -15.0,
                                     4.0,  -2.0, -14.0,  18.0,
                                     4.0,  13.0, -14.0,   3.0);

      Matrix actual = mat.Adjoint();

      Assert.AreEqual<Matrix>(expected, actual, "Aurora.Matrix.Adjoint did not return the expected value.");
    }

    /// <summary>
    ///A test case for Det (double, double, double, double)
    ///</summary>
    [TestMethod()]
    public void DetTest()
    {
      double a = 3.0;
      double b = 5.0;
      double c = 2.0;
      double d = 4.0;

      double expected = 2.0;
      double actual = Aurora.Matrix.Det(a, b, c, d);

      Assert.AreEqual(expected, actual, "Aurora.Matrix.Det did not return the expected value.");
    }

    /// <summary>
    ///A test case for Det (double, double, double, double, double, double, double, double, double)
    ///</summary>
    [TestMethod()]
    public void DetTest1()
    {
      double m00 = 3.0;
      double m01 = 1.0; 
      double m02 = 4.0; 
      double m10 = 1.0; 
      double m11 = 3.0; 
      double m12 = 2.0; 
      double m20 = 2.0; 
      double m21 = 4.0; 
      double m22 = 3.0; 

      double expected = -4.0;
      double actual;

      actual = Aurora.Matrix.Det(m00, m01, m02, m10, m11, m12, m20, m21, m22);

      Assert.AreEqual(expected, actual, "Aurora.Matrix.Det did not return the expected value.");
    }

    /// <summary>
    ///A test case for Inverse()
    ///</summary>
    [TestMethod()]
    public void InverseTest()
    {


      Matrix expected = new Matrix( 1.0, -2.0,  1.0,  0.0,
                                   -1.0,  0.5,  2.0, -1.5,
                                    0.4, -0.2, -1.4,  1.8,
                                    0.4,  1.3, -1.4,  0.3);
      expected = expected * (1.0 / 3.0);

      Matrix actual = mat.Inverse();

      Assert.AreEqual(expected, actual, "Aurora.Matrix.Inverse did not return the expected value.");
    }

    /// <summary>
    ///A test case for Matrix ()
    ///</summary>
    [TestMethod()]
    public void ConstructorTest()
    {
      Matrix actual = new Matrix();

      Matrix expected = new Matrix(0.0);

      Assert.AreEqual(expected, actual, "Aurora.Matrix Constructor did not return the expected value.");
    }

    /// <summary>
    ///A test case for Matrix (double)
    ///</summary>
    [TestMethod()]
    public void ConstructorTest1()
    {
      double x = 3.0;
      Matrix actual = new Matrix(x);

      for(int c = 0; c < 4; c++)
        for(int r = 0; r < 4; r++)
          Assert.AreEqual(c == r ? 3.0 : 0.0, actual[r, c], "Aurora.Matrix Constructor did not return the expected value.");
    }

    /// <summary>
    ///A test case for Matrix (double, double, double, double, double, double, double, double, double, double, double, double, double, double, double, double)
    ///</summary>
    [TestMethod()]
    public void ConstructorTest2()
    {
      double a00 = 3.0; 
      double a01 = 2.0; 
      double a02 = 1.0; 
      double a03 = 4.0; 
      double a10 = 1.0; 
      double a11 = 3.0; 
      double a12 = 2.0; 
      double a13 = 3.0; 
      double a20 = 2.0; 
      double a21 = 4.0; 
      double a22 = 3.0; 
      double a23 = 2.0; 
      double a30 = 1.0; 
      double a31 = 3.0; 
      double a32 = 4.0; 
      double a33 = 1.0; 

      Matrix target = new Matrix(a00, a01, a02, a03, a10, a11, a12, a13, a20, a21, a22, a23, a30, a31, a32, a33);

      Assert.AreEqual(target,  mat, "Aurora.Matrix Constructor did not return the expected value.");
    }

    /// <summary>
    ///A test case for Multiply
    ///</summary>
    [TestMethod()]
    public void MultiplyTest()
    {
      Matrix actual = mat * mat;

      Assert.AreEqual(mat2, actual, "Aurora.Matrix.operator * did not return the expected value.");
    }

    /// <summary>
    ///A test case for Multiply
    ///</summary>
    [TestMethod()]
    public void MultiplyTest1()
    {
      //Matrix expected = null;
      //Matrix actual;

      //actual = (mat + mat) * 0.5;

      //Assert.AreEqual(mat, actual, "Aurora.Matrix.operator * did not return the expected value.");
    }

    /// <summary>
    ///A test case for Transpose ()
    ///</summary>
    [TestMethod()]
    public void TransposeTest()
    {
      Matrix target = new Matrix(1.0, 2.0, 3.0, 4.0,
                                 5.0, 6.0, 7.0, 8.0,
                                 1.0, 2.0, 3.0, 4.0,
                                 5.0, 6.0, 7.0, 8.0);

      Matrix expected = new Matrix(1.0, 5.0, 1.0, 5.0,
                                   2.0, 6.0, 2.0, 6.0,
                                   3.0, 7.0, 3.0, 7.0,
                                   4.0, 8.0, 4.0, 8.0);

      Matrix actual = target.Transpose();

      Assert.AreEqual(expected, actual, "Aurora.Matrix.Transpose did not return the expected value.");
    }

    /// <summary>
    ///A test case for UnaryNegation
    ///</summary>
    [TestMethod()]
    public void UnaryNegationTest()
    {
      Matrix expected = new Matrix(0.0);
      Matrix actual = -mat;

      //Assert.AreEqual(expected, mat + actual, "Aurora.Matrix.operator - did not return the expected value.");
    }

    /// <summary>
    ///A test case for UnaryPlus
    ///</summary>
    [TestMethod()]
    public void UnaryPlusTest()
    {
      Matrix actual;

      actual = +mat;

      Assert.AreEqual(mat, actual, "Aurora.Matrix.operator + did not return the expected value.");
    }
  }

  /// <summary>
  ///This is a test class for Aurora.Transform and is intended
  ///to contain all Aurora.Transform Unit Tests
  ///</summary>
  [TestClass()]
  public class TransformTest
  {
    private TestContext testContextInstance;

    /// <summary>
    ///Gets or sets the test context which provides
    ///information about and functionality for the current test run.
    ///</summary>
    public TestContext TestContext
    { get { return testContextInstance; }
      set { testContextInstance = value; }
    }

    /// <summary>
    ///Initialize() is called once during test execution before
    ///test methods in this test class are executed.
    ///</summary>
    [TestInitialize()]
    public void Initialize()
    {
      //  TODO: Add test initialization code
    }

    /// <summary>
    ///Cleanup() is called once during test execution after
    ///test methods in this class have executed unless
    ///this test class' Initialize() method throws an exception.
    ///</summary>
    [TestCleanup()]
    public void Cleanup()
    {
      //  TODO: Add test cleanup code
    }


    /// <summary>
    ///A test case for Rotate (Axis, double)
    ///</summary>
    [TestMethod()]
    public void RotateTest()
    {
      Transform target = new Transform();

      Axis a = Axis.X;

      double amount = 0; // TODO: Initialize to an appropriate value

      target.Rotate(a, amount);

      Assert.Inconclusive("A method that does not return a value cannot be verified.");
    }

    /// <summary>
    ///A test case for Scale (double)
    ///</summary>
    [TestMethod()]
    public void ScaleTest()
    {
      Transform target = new Transform();

      double s = 0; // TODO: Initialize to an appropriate value

      target.Scale(s);

      Assert.Inconclusive("A method that does not return a value cannot be verified.");
    }

    /// <summary>
    ///A test case for Scale (double, double, double)
    ///</summary>
    [TestMethod()]
    public void ScaleTest1()
    {
      Transform target = new Transform();

      double sx = 0; // TODO: Initialize to an appropriate value

      double sy = 0; // TODO: Initialize to an appropriate value

      double sz = 0; // TODO: Initialize to an appropriate value

      target.Scale(sx, sy, sz);

      Assert.Inconclusive("A method that does not return a value cannot be verified.");
    }

    /// <summary>
    ///A test case for Transform ()
    ///</summary>
    [TestMethod()]
    public void ConstructorTest()
    {
      Transform target = new Transform();

      // TODO: Implement code to verify target
      Assert.Inconclusive("TODO: Implement code to verify target");
    }

    /// <summary>
    ///A test case for Transform (Axis, double)
    ///</summary>
    [TestMethod()]
    public void ConstructorTest1()
    {
      Axis a = Axis.X; // TODO: Initialize to an appropriate value

      double amount = 0; // TODO: Initialize to an appropriate value

      Transform target = new Transform(a, amount);

      // TODO: Implement code to verify target
      Assert.Inconclusive("TODO: Implement code to verify target");
    }

    /// <summary>
    ///A test case for Transform (double)
    ///</summary>
    [TestMethod()]
    public void ConstructorTest2()
    {
      double s = 0; // TODO: Initialize to an appropriate value

      Transform target = new Transform(s);

      // TODO: Implement code to verify target
      Assert.Inconclusive("TODO: Implement code to verify target");
    }

    /// <summary>
    ///A test case for Transform (double, double, double)
    ///</summary>
    [TestMethod()]
    public void ConstructorTest3()
    {
      double sx = 0; // TODO: Initialize to an appropriate value

      double sy = 0; // TODO: Initialize to an appropriate value

      double sz = 0; // TODO: Initialize to an appropriate value

      Transform target = new Transform(sx, sy, sz);

      // TODO: Implement code to verify target
      Assert.Inconclusive("TODO: Implement code to verify target");
    }

    /// <summary>
    ///A test case for Transform (Vector)
    ///</summary>
    [TestMethod()]
    public void ConstructorTest4()
    {
      Vector3 v = new Vector3(); // TODO: Initialize to an appropriate value

      Transform target = new Transform(v);

      // TODO: Implement code to verify target
      Assert.Inconclusive("TODO: Implement code to verify target");
    }

    /// <summary>
    ///A test case for Translate (double, double, double)
    ///</summary>
    [TestMethod()]
    public void TranslateTest()
    {
      Transform target = new Transform();

      double dx = 0; // TODO: Initialize to an appropriate value

      double dy = 0; // TODO: Initialize to an appropriate value

      double dz = 0; // TODO: Initialize to an appropriate value

      target.Translate(dx, dy, dz);

      Assert.Inconclusive("A method that does not return a value cannot be verified.");
    }

    /// <summary>
    ///A test case for Translate (Vector)
    ///</summary>
    [TestMethod()]
    public void TranslateTest1()
    {
      Transform target = new Transform();

      Vector3 v = new Vector3(); // TODO: Initialize to an appropriate value

      target.Translate(v);

      Assert.Inconclusive("A method that does not return a value cannot be verified.");
    }

  }
  /// <summary>
  ///This is a test class for Aurora.Colour and is intended
  ///to contain all Aurora.Colour Unit Tests
  ///</summary>
  [TestClass()]
  public class ColourTest
  {


    private TestContext testContextInstance;

    /// <summary>
    ///Gets or sets the test context which provides
    ///information about and functionality for the current test run.
    ///</summary>
    public TestContext TestContext
    {
      get
      {
        return testContextInstance;
      }
      set
      {
        testContextInstance = value;
      }
    }

    /// <summary>
    ///Initialize() is called once during test execution before
    ///test methods in this test class are executed.
    ///</summary>
    [TestInitialize()]
    public void Initialize()
    {
      //  TODO: Add test initialization code
    }

    /// <summary>
    ///Cleanup() is called once during test execution after
    ///test methods in this class have executed unless
    ///this test class' Initialize() method throws an exception.
    ///</summary>
    [TestCleanup()]
    public void Cleanup()
    {
      //  TODO: Add test cleanup code
    }


    /// <summary>
    ///A test case for Addition
    ///</summary>
    [TestMethod()]
    public void AdditionTest()
    {
      Colour c = new Colour(0x102030); 
      Colour d = new Colour(0x604020); 

      Colour expected = new Colour(0x706050);
      Colour actual;

      actual = c + d;

      Assert.AreEqual(expected, actual, "Aurora.Colour.operator + did not return the expected value.");
    }

    /// <summary>
    ///A test case for Clamp (double)
    ///</summary>
    [TestMethod()]
    public void ClampTest()
    {
      double x = 0; // TODO: Initialize to an appropriate value

      double expected = 0;
      double actual;

      actual = AuroraUnitTests.Aurora_ColourAccessor.Clamp(x);

      Assert.AreEqual(expected, actual, "Aurora.Colour.Clamp did not return the expected value.");
      Assert.Inconclusive("Verify the correctness of this test method.");
    }

    /// <summary>
    ///A test case for Colour (double)
    ///</summary>
    [TestMethod()]
    public void ConstructorTest()
    {
      double greylevel = 0; // TODO: Initialize to an appropriate value

      Colour target = new Colour(greylevel);

      // TODO: Implement code to verify target
      Assert.Inconclusive("TODO: Implement code to verify target");
    }

    /// <summary>
    ///A test case for Colour (double, double, double)
    ///</summary>
    [TestMethod()]
    public void ConstructorTest1()
    {
      double red = 0; // TODO: Initialize to an appropriate value

      double green = 0; // TODO: Initialize to an appropriate value

      double blue = 0; // TODO: Initialize to an appropriate value

      Colour target = new Colour(red, green, blue);

      // TODO: Implement code to verify target
      Assert.Inconclusive("TODO: Implement code to verify target");
    }

    /// <summary>
    ///A test case for Colour (int)
    ///</summary>
    [TestMethod()]
    public void ConstructorTest2()
    {
      int preset = 0; // TODO: Initialize to an appropriate value

      Colour target = new Colour(preset);

      // TODO: Implement code to verify target
      Assert.Inconclusive("TODO: Implement code to verify target");
    }

    /// <summary>
    ///A test case for Colour (System.Drawing.Color)
    ///</summary>
    [TestMethod()]
    public void ConstructorTest3()
    {
      Color c = new Color(); // TODO: Initialize to an appropriate value

      Colour target = new Colour(c);

      // TODO: Implement code to verify target
      Assert.Inconclusive("TODO: Implement code to verify target");
    }

    /// <summary>
    ///A test case for Conversion
    ///</summary>
    [TestMethod()]
    public void ConversionTest()
    {
      Colour c = new Colour(); // TODO: Initialize to an appropriate value

      Color expected = new Color();
      Color actual;

      actual = c;

      Assert.AreEqual(expected, actual, "Aurora.Colour.implicit operator did not return the expected value.");
      Assert.Inconclusive("Verify the correctness of this test method.");
    }

    /// <summary>
    ///A test case for Gamma (double)
    ///</summary>
    [TestMethod()]
    public void GammaTest()
    {
      Color c = new Color(); // TODO: Initialize to an appropriate value

      Colour target = new Colour(c);

      double g = 0; // TODO: Initialize to an appropriate value

      Colour expected = new Colour();
      Colour actual;

      actual = target.Gamma(g);

      Assert.AreEqual(expected, actual, "Aurora.Colour.Gamma did not return the expected value.");
      Assert.Inconclusive("Verify the correctness of this test method.");
    }

    /// <summary>
    ///A test case for Interpolate (Colour, Colour, double)
    ///</summary>
    [TestMethod()]
    public void InterpolateTest()
    {
      Colour c = new Colour(); // TODO: Initialize to an appropriate value

      Colour d = new Colour(); // TODO: Initialize to an appropriate value

      double pc = 0; // TODO: Initialize to an appropriate value

      Colour expected = new Colour();
      Colour actual;

      actual = Aurora.Colour.Interpolate(c, d, pc);

      Assert.AreEqual(expected, actual, "Aurora.Colour.Interpolate did not return the expected value.");
      Assert.Inconclusive("Verify the correctness of this test method.");
    }

    /// <summary>
    ///A test case for Lightness ()
    ///</summary>
    [TestMethod()]
    public void LightnessTest()
    {
      Color c = new Color(); // TODO: Initialize to an appropriate value

      Colour target = new Colour(c);

      double expected = 0;
      double actual;

      actual = target.Lightness();

      Assert.AreEqual(expected, actual, "Aurora.Colour.Lightness did not return the expected value.");
      Assert.Inconclusive("Verify the correctness of this test method.");
    }

    /// <summary>
    ///A test case for Multiply
    ///</summary>
    [TestMethod()]
    public void MultiplyTest()
    {
      double d = 0; // TODO: Initialize to an appropriate value

      Colour c = new Colour(); // TODO: Initialize to an appropriate value

      Colour expected = new Colour();
      Colour actual;

      actual = d * c;

      Assert.AreEqual(expected, actual, "Aurora.Colour.operator * did not return the expected value.");
      Assert.Inconclusive("Verify the correctness of this test method.");
    }

    /// <summary>
    ///A test case for Multiply
    ///</summary>
    [TestMethod()]
    public void MultiplyTest1()
    {
      Colour c = new Colour(); // TODO: Initialize to an appropriate value

      double d = 0; // TODO: Initialize to an appropriate value

      Colour expected = new Colour();
      Colour actual;

      actual = c * d;

      Assert.AreEqual(expected, actual, "Aurora.Colour.operator * did not return the expected value.");
      Assert.Inconclusive("Verify the correctness of this test method.");
    }

    /// <summary>
    ///A test case for Multiply
    ///</summary>
    [TestMethod()]
    public void MultiplyTest2()
    {
      Colour c = new Colour(); // TODO: Initialize to an appropriate value

      Colour d = new Colour(); // TODO: Initialize to an appropriate value

      Colour expected = new Colour();
      Colour actual;

      actual = c * d;

      Assert.AreEqual(expected, actual, "Aurora.Colour.operator * did not return the expected value.");
      Assert.Inconclusive("Verify the correctness of this test method.");
    }

  }


}
