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

        private bool isFinished = false;
        public bool Finished
        {
            get
            {
                if (leftGame.isGameOver || rightGame.isGameOver)
                {
                    isFinished = true;
                }
                return isFinished;
            }
            set
            {
                isFinished = value;
            }
        }

        public void MonEnter(bool isLeft){
            if(isLeft){
                leftGame.Enter();
            }else{
                rightGame.Enter();
            }
        }
        public void MonExit(bool isLeft)
        {
            if (isLeft)
            {
                leftGame.Exit();
            }
            else
            {
                rightGame.Exit();
            }
        }

        public bool isAnimationMode(bool isLeft)
        {
            if (isLeft)
            {
                return leftGame.isAnimationMode;
            }
            else
            {
                return rightGame.isAnimationMode;
            }
        }
        public void finishedAnimationMode(bool isLeft)
        {
            if (isLeft)
            {
                leftGame.isAnimationMode = false;
            }
            else
            {
                rightGame.isAnimationMode = false;
            }
        }

        public Animation.Animator GetAnimator(bool isLeft)
        {
            //TODO: ChainResult에 대한 copy constructor 만들어서 적용하기.
            if (isLeft)
            {
                return new Animation.Animator(leftGame.chainResult.copiedBoard, leftGame.chainResult.animation);
            }
            else
            {
                return new Animation.Animator(rightGame.chainResult.copiedBoard, rightGame.chainResult.animation);
            }
        }

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
                return leftGame.Pile[row, col].Color;
            }
            else
            {
                return rightGame.Pile[row, col].Color;
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


        public bool UseGhost = true;

        public int GetGhostRow(bool isLeft, int rowInBlock, int colInBlock)
        {
            if (isLeft)
            {
                return leftGame.GetGhostRow(rowInBlock, colInBlock);
            }
            else
            {
                return rightGame.GetGhostRow(rowInBlock, colInBlock);
            }
        }

        public int GetGhostCol(bool isLeft, int rowInBlock, int colInBlock)
        {
            if (isLeft)
            {
                return leftGame.GetGhostCol(rowInBlock, colInBlock);
            }
            else
            {
                return rightGame.GetGhostCol(rowInBlock, colInBlock);
            }
        }

        public CellColor GetNextBlockCellColor(bool isLeft, int p, int i, int j)
        {
            Block nextBlock;
            if (p == 0)
            {
                nextBlock = BlockList.Instance.Get1st(isLeft);
            }
            else
            {
                nextBlock = BlockList.Instance.Get2nd(isLeft);
            }
            return nextBlock[i, j].Color;
        }

        public bool isOver(bool isLeft)
        {
            if (isLeft)
            {
                return leftGame.isGameOver;
            }
            else
            {
                return rightGame.isGameOver;
            }
        }

        public Decimal GetScore(bool isLeft)
        {
            if (isLeft)
            {
                return leftGame.Score;
            }
            else
            {
                return rightGame.Score;
            }
        }
    }
}
