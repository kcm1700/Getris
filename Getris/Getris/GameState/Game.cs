using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace getris.GameState
{
    abstract class Game
    {
        protected Object thisLock = new Object(); // animation 중 또는 렌더링 중에는 잠길 필요가 있다.

        protected Pile pile;
        protected Block block;// 현재 움직이는 블럭
        protected int row;
        protected int col;
        protected bool isLeft;

        protected decimal score;

        public ChainResult chainResult;

        public System.Threading.Thread thread;
        public abstract void Start();

        protected bool gameOver;
        protected bool animationMode;

        protected int[,] ghostInfoRow;

        //모니터 lock 관련 함수. 그릴 때는 통제해야한다.
        public void Enter()
        {
            System.Threading.Monitor.Enter(thisLock);
        }
        public void Exit()
        {
            System.Threading.Monitor.Exit(thisLock);
        }

        public bool isGameOver
        {
            get
            {
                return gameOver;
            }
        }
        public bool isAnimationMode
        {
            get
            {
                return animationMode;
            }
            set
            {
                //TODO: keyboard로 쌓인 것들 냅둘 것인가 결정하기.
                animationMode = value;
                if (animationMode == false)
                {
                    // animation ended. regen block
                    BlockRegen();
                }
            }
        }
        public Game(bool isLeft=true)
        {
            this.isLeft = isLeft;
            animationMode = false;
            score = 0;
            pile = new Pile();
            ghostInfoRow = new int[Block.ROW_SIZE, Block.COL_SIZE];
            gameOver = false;
            BlockRegen();
        }
        ~Game()
        {
            if (this.thread.IsAlive)
            {
                this.thread.Abort();
            }
        }

        public virtual CellColor GetPileCellColor(int row, int col)
        {
            return pile.GetCellColor(row,col);
        }
        public Pile Pile
        {
            get
            {
                return pile;
            }
        }
        public Block Block
        {
            get
            {
                return block;
            }
        }

        private System.Diagnostics.Stopwatch swDrop = new System.Diagnostics.Stopwatch();
        

        public virtual CellColor GetBlockCellColor(int row, int col)
        {
            return block[row, col].Color;
        }

        public virtual decimal Score
        {
            get
            {
                return score;
            }
        }

        public virtual int Row
        {
            get
            {
                return this.row;
            }
        }
        public virtual int Col
        {
            get
            {
                return this.col;
            }
        }
        
        protected virtual bool Rotate(bool isCw)
        {
            if (isAnimationMode) return false;
            if (gameOver) return false;
            if (isCw)
            {
                block.RotateCw();
            }
            else
            {
                block.RotateCcw();
            }
            //SRS
            int[,] trylist = { { 0, 0 }, { 1, 0 }, { -1, 0 }, { 0, -1 }, { 0, 1 }, { 2, 0 }, { 0, 2 }, { 0, -2 } };
            bool flgOk = false;
            for (int i = 0; i < trylist.Length; i++)
            {
                if (!pile.IsBlockCollision(row + trylist[i, 0], col + trylist[i, 1], block))
                {
                    row += trylist[i, 0];
                    col += trylist[i, 1];
                    flgOk = true;
                    break;
                }
            }
            if (flgOk == false)
            {
                if (isCw)
                {
                    block.RotateCcw();
                }
                else
                {
                    block.RotateCw();
                }
            }

            pile.CalcGhost(ghostInfoRow, row, col, block);
            return flgOk;
        }
        protected virtual bool BlockRegen()
        {
            if (isAnimationMode) return false;
            if (gameOver) return false;
            BlockList.Instance.NextBlock(isLeft);
            block = BlockList.Instance.GetBlock(isLeft);

            row = Pile.ROW_SIZE - 2;
            col = (Pile.COL_SIZE + 1) / 2;
            if (pile.IsBlockCollision(row, col, block))
            {
                gameOver = true;
            }
            else
            {
                //regen succeeded
                pile.CalcGhost(ghostInfoRow, row, col, block);
            }
            return true;
        }


        protected virtual bool Drop()
        {
            if (isAnimationMode) return false;
            if (gameOver) return false;
            try
            {
                pile.DropBlock(row, col, block);
                //TODO: SimulateChain의 결과 처리하기
                chainResult = pile.SimulateChain();
                score += chainResult.Score;
            }
            finally
            {
//                swDrop.Restart();
                animationMode = true;
            }
            return true;
        }

        

        public int GetGhostRow(int rowInBlock, int colInBlock)
        {
            return ghostInfoRow[rowInBlock, colInBlock];
        }

        public int GetGhostCol(int rowInBlock, int colInBlock)
        {
            return this.col + colInBlock;
        }
    }
}
