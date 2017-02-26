using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetObjects.Processors;
using NetObjects.Core;

namespace NetObjectsTests.UnitTests
{
    /// <summary>
    /// Summary description for TrueProcessorTest
    /// </summary>
    [TestClass]
    public class TrueProcessorTest
    {
        private TrueProcessor _target;
        public TrueProcessorTest()
        {
            _target = new TrueProcessor();
          
        }


        [TestMethod]
        public void TestInitProcess()
        {
            var t = new CalculationTask("Cube", 27);
            _target.InitProcess(t, new NetBuilder(t));
        }
    }
}
