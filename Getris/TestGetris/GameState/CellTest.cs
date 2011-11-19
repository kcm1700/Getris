using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestGetris.GameState
{
    [TestClass]
    public class CellTest
    {
        public CellTest()
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
        public void TestBlankCell()
        {
            getris.GameState.Cell cell = new getris.GameState.BlankCell();
            Assert.AreEqual<getris.GameState.CellColor>(getris.GameState.CellColor.transparent, cell.Color);
        }

        [TestMethod]
        public void TestBlockCell()
        {
            getris.GameState.Cell cell1 = new getris.GameState.BlockCell(getris.GameState.CellColor.color1);
            Assert.AreEqual<getris.GameState.CellColor>(getris.GameState.CellColor.color1, cell1.Color);

            getris.GameState.Cell cell2 = new getris.GameState.BlockCell(getris.GameState.CellColor.color2);
            Assert.AreEqual<getris.GameState.CellColor>(getris.GameState.CellColor.color2, cell2.Color);


            getris.GameState.Cell cell3 = new getris.GameState.BlockCell(getris.GameState.CellColor.color3);
            Assert.AreEqual<getris.GameState.CellColor>(getris.GameState.CellColor.color3, cell3.Color);


            getris.GameState.Cell cell4 = new getris.GameState.BlockCell(getris.GameState.CellColor.color4);
            Assert.AreEqual<getris.GameState.CellColor>(getris.GameState.CellColor.color4, cell4.Color);


            getris.GameState.Cell cell5 = new getris.GameState.BlockCell(getris.GameState.CellColor.color5);
            Assert.AreEqual<getris.GameState.CellColor>(getris.GameState.CellColor.color5, cell5.Color);
        }
    }
}
