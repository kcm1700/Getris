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

        public System.Threading.Thread LeftThread;
        public System.Threading.Thread RightThread;

        public readonly static int colorCnt = 5;

        public Battle(bool isLeftRun=true, bool isRightRun=false)
        {
            if (isLeftRun && isRightRun)
                throw new Exception("둘다 run일리가 없다.");
            if (isLeftRun)
            {
                leftGame = new RunGame();
            }
            else
            {
                leftGame = new DisplayGame();
            }
            if (isRightRun)
            {
                rightGame = new RunGame();
            }
            else
            {
                rightGame = new DisplayGame();
            }
            LeftThread = leftGame.thread;
            RightThread = rightGame.thread;
            leftGame.Start();
            rightGame.Start();
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
