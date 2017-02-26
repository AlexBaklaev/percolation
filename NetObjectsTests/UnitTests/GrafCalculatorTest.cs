using NetObjects.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace NetObjectsTests
{


    /// <summary>
    ///This is a test class for GrafCalculatorTest and is intended
    ///to contain all GrafCalculatorTest Unit Tests
    ///</summary>
    [TestClass()]
    public class GrafCalculatorTest
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
        ///A test for HasPath
        ///</summary>
        [TestMethod()]
        public void HasPathTest()
        {
            Node[] net = new[] { 
                                 new Node(0) { NearestNodes = new[] { 9 }, IsActive = true} ,
                                 new Node(1) { NearestNodes = new[] { 5, 4, 2, 3 } , IsActive = true},
                                 new Node(2) { NearestNodes = new[] { 1, 5, 4 } , IsActive = true},
                                 new Node(3) { NearestNodes = new[] { 1, 4, 8 } , IsActive = true},
                                 new Node(4) { NearestNodes = new[] { 2, 3, 1 } , IsActive = true},
                                 new Node(5) { NearestNodes = new[] { 2, 1 }, IsActive = true },
                                 new Node(6) { NearestNodes = new[] { 8 } , IsActive = true},
                                 new Node(7) { NearestNodes = new[] { 8,1 } , IsActive = true},
                                 new Node(8) { NearestNodes = new[] { 3, 6, 7 } , IsActive = true},
                                 new Node(9){ NearestNodes = new[] { 0 }, IsActive = true
                                 }};

            Assert.AreEqual(true, GrafCalculator.HasPath(net, 5, 7));
            Assert.AreEqual(true, GrafCalculator.HasPath(net, 1, 6));
            Assert.AreEqual(true, GrafCalculator.HasPath(net, 6, 1));
            Assert.AreEqual(true, GrafCalculator.HasPath(net, 3, 5));
            Assert.AreEqual(false, GrafCalculator.HasPath(net, 0, 1));
            Assert.AreEqual(false, GrafCalculator.HasPath(net, 7, 9));
            
        }
    }
}
