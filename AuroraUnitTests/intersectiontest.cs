using Aurora;
using Microsoft.VisualStudio.QualityTools.UnitTesting.Framework;
namespace AuroraUnitTests
{
  /// <summary>
  ///This is a test class for Aurora.Intersection and is intended
  ///to contain all Aurora.Intersection Unit Tests
  ///</summary>
  [TestClass()]
  public class IntersectionTest
  {

    Intersection target;
    Intersection nearer;
    Intersection farther;

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
      target = new Intersection(new Point3(1.0, 2.0, 3.0), 5.0, true, null, null);
      nearer = new Intersection(new Point3(1.0, 2.0, 3.0), 4.9, true, null, null);
      farther = new Intersection(new Point3(1.0, 2.0, 3.0), 5.1, true, null, null);
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
    ///A test case for Distance
    ///</summary>
    [TestMethod()]
    public void DistanceTest()
    {
      double val = 5.0; 

      Assert.AreEqual(val, target.Distance, "Aurora.Intersection.Distance was not set correctly.");
    }

    /// <summary>
    ///A test case for Entering
    ///</summary>
    [TestMethod()]
    public void EnteringTest()
    {
      bool val = true; 
      Assert.AreEqual(val, target.Entering, "Aurora.Intersection.Entering was not set correctly.");
    }

    /// <summary>
    ///A test case for Flip ()
    ///</summary>
    [TestMethod()]
    public void FlipTest()
    {
      target.Flip();

      bool val = false;
      Assert.AreEqual(val, target.Entering, "Aurora.Intersection.Flip did not operate correctly.");

    }

    /// <summary>
    ///A test case for GreaterThan
    ///</summary>
    [TestMethod()]
    public void GreaterThanTest()
    {
      bool actual;
      bool expected;

      expected = true;
      actual = farther > target;
      Assert.AreEqual(expected, actual, "Aurora.Intersection.operator > did not return the expected value.");

      expected = false;
      actual = nearer > target;
      Assert.AreEqual(expected, actual, "Aurora.Intersection.operator > did not return the expected value.");

    }

    /// <summary>
    ///A test case for Intersection ()
    ///</summary>
    [TestMethod()]
    public void ConstructorTest()
    {
      Intersection target = new Intersection();

      // TODO: Implement code to verify target
      Assert.Inconclusive("TODO: Implement code to verify target");
    }

    /// <summary>
    ///A test case for Intersection (Point, double, bool, Model, Material)
    ///</summary>
    [TestMethod()]
    public void ConstructorTest1()
    {
      Point3 location = new Point3(); // TODO: Initialize to an appropriate value

      double distance = 0; // TODO: Initialize to an appropriate value

      bool entering = false; // TODO: Initialize to an appropriate value

      Model model = null; // TODO: Initialize to an appropriate value

      Material medium = null; // TODO: Initialize to an appropriate value

      Intersection target = new Intersection(location, distance, entering, model, medium);

      // TODO: Implement code to verify target
      Assert.Inconclusive("TODO: Implement code to verify target");
    }

    /// <summary>
    ///A test case for LessThan
    ///</summary>
    [TestMethod()]
    public void LessThanTest()
    {
      Intersection i = null; // TODO: Initialize to an appropriate value

      Intersection j = null; // TODO: Initialize to an appropriate value

      bool expected = false;
      bool actual;

      actual = i < j;

      Assert.AreEqual(expected, actual, "Aurora.Intersection.operator [ did not return the expected value.");
      Assert.Inconclusive("Verify the correctness of this test method.");
    }

    /// <summary>
    ///A test case for Location
    ///</summary>
    [TestMethod()]
    public void LocationTest()
    {
      Intersection target = new Intersection();

      Point3 val = new Point3(); // TODO: Assign to an appropriate value for the property


      Assert.AreEqual(val, target.Location, "Aurora.Intersection.Location was not set correctly.");
      Assert.Inconclusive("Verify the correctness of this test method.");
    }

    /// <summary>
    ///A test case for Medium
    ///</summary>
    [TestMethod()]
    public void MediumTest()
    {
      Intersection target = new Intersection();

      Material val = null; // TODO: Assign to an appropriate value for the property


      Assert.AreEqual(val, target.Medium, "Aurora.Intersection.Medium was not set correctly.");
      Assert.Inconclusive("Verify the correctness of this test method.");
    }

    /// <summary>
    ///A test case for Model
    ///</summary>
    [TestMethod()]
    public void ModelTest()
    {
      Intersection target = new Intersection();

      Model val = null; // TODO: Assign to an appropriate value for the property

      target.Model = val;


      Assert.AreEqual(val, target.Model, "Aurora.Intersection.Model was not set correctly.");
      Assert.Inconclusive("Verify the correctness of this test method.");
    }

  }
  /// <summary>
  ///This is a test class for Aurora.IntersectionList and is intended
  ///to contain all Aurora.IntersectionList Unit Tests
  ///</summary>
  [TestClass()]
  public class IntersectionListTest
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
    ///A test case for Distance ()
    ///</summary>
    [TestMethod()]
    public void DistanceTest()
    {
      IntersectionList target = new IntersectionList();

      double expected = 0;
      double actual;

      actual = target.Distance();

      Assert.AreEqual(expected, actual, "Aurora.IntersectionList.Distance did not return the expected value.");
      Assert.Inconclusive("Verify the correctness of this test method.");
    }

    /// <summary>
    ///A test case for Drop ()
    ///</summary>
    [TestMethod()]
    public void DropTest()
    {
      IntersectionList target = new IntersectionList();

      target.Drop();

      Assert.Inconclusive("A method that does not return a value cannot be verified.");
    }

    /// <summary>
    ///A test case for Empty ()
    ///</summary>
    [TestMethod()]
    public void EmptyTest()
    {
      IntersectionList target = new IntersectionList();

      bool expected = false;
      bool actual;

      actual = target.Empty();

      Assert.AreEqual(expected, actual, "Aurora.IntersectionList.Empty did not return the expected value.");
      Assert.Inconclusive("Verify the correctness of this test method.");
    }

    /// <summary>
    ///A test case for Entering ()
    ///</summary>
    [TestMethod()]
    public void EnteringTest()
    {
      IntersectionList target = new IntersectionList();

      bool expected = false;
      bool actual;

      actual = target.Entering();

      Assert.AreEqual(expected, actual, "Aurora.IntersectionList.Entering did not return the expected value.");
      Assert.Inconclusive("Verify the correctness of this test method.");
    }

    /// <summary>
    ///A test case for IntersectionList ()
    ///</summary>
    [TestMethod()]
    public void ConstructorTest()
    {
      IntersectionList target = new IntersectionList();

      // TODO: Implement code to verify target
      Assert.Inconclusive("TODO: Implement code to verify target");
    }

  }


}
