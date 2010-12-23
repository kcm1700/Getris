using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace getris.GameState
{
    public class Battle
    {
        private Game leftGame;
        private Game rightGame;

        public Battle()
        {
            leftGame = new RunGame();
            rightGame = new DisplayGame();
        }

        public CellColor GetLeftGamePileCellColor(int row, int col)
        {
            return leftGame.GetPileCellColor(row, col);
        }
        /// <summary>
        /// Row는 블럭의 위치를 나타냄
        /// </summary>
        public int GetLeftGameRow
        {
            get
            {
                return leftGame.Row;
            }
        }
        public int GetLeftGameCol
        {
            get
            {
                return leftGame.Col;
            }
        }

        public CellColor GetLeftGameBlockCellColor(int row, int col)
        {
            return leftGame.GetBlockCellColor(row, col);
        }
    }
}
