using NetObjects.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetObjects.Enums;

namespace NetObjectsTests
{

   /// <summary>
   ///This is a test class for NetSaveLoadTest and is intended
   ///to contain all NetSaveLoadTest Unit Tests
   ///</summary>
   [TestClass]
   public class NetSaveLoadTest
   {
      private readonly string _path = Properties.Settings.Default.PathToTestResults;
      public TestContext TestContext { get; set; }

      /// <summary>
      ///A test for NetSaveLoad Constructor
      ///</summary>
      [TestMethod]
      public void NetSaveTest()
      {
         Assert.IsTrue(
             new NetSaveLoad().NetSave
             (new NetBuilder
                 (new CalculationTask
                     (NetType.Quadro.ToString(), 10000)).CreateNet(), _path));
      }

      /// <summary>
      ///A test for NetLoad
      ///</summary>
      [TestMethod]
      public void NetLoadTest()
      {
         Node[] actual = new NetSaveLoad(_path + @"\Result.xml").CreateNet();
         Assert.IsNotNull(actual);
      }
   }
}
