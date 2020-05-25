using System;
using Microsoft.VisualStudio.QualityTools.UnitTesting.Framework;

namespace AuroraUnitTests
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class UnitTest1
  {
    public UnitTest1()
    {
      //
      // TODO: Add constructor logic here
      //
    }

    /// <summary>
    /// Initialize() is called once during test execution before
    /// test methods in this test class are executed.
    /// </summary>
    [TestInitialize()]
    public void Initialize()
    {
      //  TODO: Add test initialization code
    }

    /// <summary>
    /// Cleanup() is called once during test execution after
    /// test methods in this class have executed unless the
    /// corresponding Initialize() call threw an exception.
    /// </summary>
    [TestCleanup()]
    public void Cleanup()
    {
      //  TODO: Add test cleanup code
    }

    [TestMethod]
    public void TestMethod1()
    {
      //
      // TODO: Add test logic	here
      //
    }
  }
}
