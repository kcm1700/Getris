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
                leftGame = new RunGame(true);
            }
            else
            {
                leftGame = new DisplayGame(true);
            }
            if (isRightRun)
            {
                rightGame = new RunGame(false);
            }
            else
            {
                rightGame = new DisplayGame(false);
            }
            LeftThread = leftGame.thread;
            RightThread = rightGame.thread;
            leftGame.Start();
            rightGame.Start();
        }

        public CellColor GetPileCellColor(bool isLeft, int row, int col)
        {
            if (isLeft)
            {
                return leftGame.GetPileCellColor(row, col);
            }
            else
            {
                return rightGame.GetPileCellColor(row, col);
            }
        }
        public int GetRow(bool isLeft)
        {
            if (isLeft)
            {
                return leftGame.Row;
            }
            else
            {
                return rightGame.Row;
            }
        }
        public int GetCol(bool isLeft)
        {
            if (isLeft)
            {
                return leftGame.Col;
            }
            else
            {
                return rightGame.Col;
            }
        }

        public CellColor GetBlockCellColor(bool isLeft, int row, int col)
        {
            if (isLeft)
            {
                return leftGame.GetBlockCellColor(row, col);
            }
            else
            {
                return rightGame.GetBlockCellColor(row, col);
            }
        }
    }
}
