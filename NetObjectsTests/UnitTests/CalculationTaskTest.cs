using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetObjects.Core;
using NetObjects.Enums;

namespace NetObjectsTests.UnitTests
{
   /// <summary>
   /// Summary description for SerializeTask
   /// </summary>
   [TestClass]
   public class SerializeTaskTest
   {
      /// <summary>
      ///Gets or sets the test context which provides
      ///information about and functionality for the current test run.
      ///</summary>
      public TestContext TestContext { get; set; }

      /// <summary>
      /// 
      /// </summary>
      [TestMethod]
      public void Serialize()
      {
         CalculationTask calc = new CalculationTask(NetType.MultyLink.ToString(), 1000000, 10, 10, 0, 0);
         byte[] len = CalculationTask.GetbytesFromCalculationTask(calc);

         CalculationTask calc2 = CalculationTask.GetCalculationTaskFrombytes(len);
         Assert.IsTrue(calc.NetType == calc2.NetType);
      }
   }
}
