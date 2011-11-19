using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestGetris.Core
{
    /// <summary>
    /// Summary description for ActionTest
    /// </summary>
    [TestClass]
    public class ActionTest
    {
        public ActionTest()
        {
        }

        private TestContext testContextInstance;

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

        [TestMethod]
        public void TestMoveLeft()
        {
            getris.Core.Action action = new getris.Core.Move("left");
            Assert.IsTrue(action.IsValid(), "Left should be valid move.");
        }
        [TestMethod]
        public void TestMoveRight()
        {
            getris.Core.Action action = new getris.Core.Move("right");
            Assert.IsTrue(action.IsValid(), "Right should be valid move.");
        }
        [TestMethod]
        public void TestMoveDrop()
        {
            getris.Core.Action action = new getris.Core.Move("drop");
            Assert.IsTrue(action.IsValid(), "Drop be valid move.");
        }
        [TestMethod]
        public void TestMoveDown()
        {
            getris.Core.Action action = new getris.Core.Move("down");
            Assert.IsTrue(action.IsValid(), "Down should be valid move.");
        }

        [TestMethod]
        public void TestGoto()
        {
            getris.Core.Action action = new getris.Core.GoTo("1:1");
            Assert.IsFalse(action.IsValid());

            action = new getris.Core.GoTo("left:1:1");
            Assert.IsTrue(action.IsValid());

            action = new getris.Core.GoTo("right:2:1");
            Assert.IsTrue(action.IsValid());

            action = new getris.Core.GoTo("1_1");
            Assert.IsFalse(action.IsValid());

            action = new getris.Core.GoTo("12:x");
            Assert.IsFalse(action.IsValid());
        }

        [TestMethod]
        public void TestRotate()
        {
            getris.Core.Action action = new getris.Core.Rotate("cw");
            Assert.IsTrue(action.IsValid());

            action = new getris.Core.Rotate("ccw");
            Assert.IsTrue(action.IsValid());

            action = new getris.Core.Rotate("left:cw");
            Assert.IsTrue(action.IsValid());

            action = new getris.Core.Rotate("left:ccw");
            Assert.IsTrue(action.IsValid());

            action = new getris.Core.Rotate("right:cw");
            Assert.IsTrue(action.IsValid());

            action = new getris.Core.Rotate("right:ccw");
            Assert.IsTrue(action.IsValid());

            action = new getris.Core.Rotate("right");
            Assert.IsFalse(action.IsValid());

            action = new getris.Core.Rotate("left");
            Assert.IsFalse(action.IsValid());
        }

        [TestMethod]
        public void TestChat()
        {
            getris.Core.Action action = new getris.Core.Chat("blah~blah~");
            Assert.IsTrue(action.IsValid());
        }

        [TestMethod]
        public void TestTurn()
        {
            getris.Core.Action action = new getris.Core.Turn("left");
            Assert.IsTrue(action.IsValid());

            action = new getris.Core.Turn("right");
            Assert.IsTrue(action.IsValid());
        }

        [TestMethod]
        public void TestStatus()
        {
        }

        [TestMethod]
        public void TestAttack()
        {
        }

        [TestMethod]
        public void TestNull()
        {
            getris.Core.Action action = new getris.Core.NullAction();
            Assert.IsTrue(action.IsValid());

            action = new getris.Core.NullAction("");
            Assert.IsTrue(action.IsValid());
        }
    }
}
