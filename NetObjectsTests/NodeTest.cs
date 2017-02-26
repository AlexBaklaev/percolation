using NetObjects.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace NetObjectsTests
{
    
    
    /// <summary>
    ///This is a test class for NodeTest and is intended
    ///to contain all NodeTest Unit Tests
    ///</summary>
    [TestClass()]
    public class NodeTest
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

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for RemoveNearestNode
        ///</summary>
        [TestMethod()]
        public void RemoveNearestNodeTest()
        {
            Node target = new Node(100);
            target.AddNearestNode(200);
            target.AddNearestNode(300);
            target.AddNearestNode(400);
            target.AddNearestNode(500);

            Assert.AreEqual(4, target.NearestNodes.Length);
          
            target.RemoveNearestNode(400);
            target.RemoveNearestNode(600);
            Assert.AreEqual(3, target.NearestNodes.Length);
        }
    }
}
