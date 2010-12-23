using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace getris.GameState
{
    abstract class Game
    {
        public Object thisLock = new Object(); // animation 중 또는 렌더링 중에는 잠길 필요가 있다.

        protected Pile pile;
        protected Block block;// 현재 움직이는 블럭
        protected int row;
        protected int col;
        protected bool isLeft;

        protected decimal score;

        public System.Threading.Thread thread;
        public abstract void Start();

        protected bool gameOver;
        public bool isGameOver
        {
            get
            {
                return gameOver;
            }
        }

        public Game(bool isLeft=true)
        {
            this.isLeft = isLeft;
            score = 0;
            pile = new Pile();
            BlockRegen();
            gameOver = false;
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

        protected virtual void Rotate(bool isCw)
        {
            if (gameOver) return;
            //TODO: validation
            if (isCw)
            {
                block.RotateCw();
                if (pile.IsBlockCollision(row, col, block))
                {
                    block.RotateCcw();
                }
            }
            else
            {
                block.RotateCcw();
                if (pile.IsBlockCollision(row, col, block))
                {
                    block.RotateCw();
                }
            }
        }
        protected virtual void BlockRegen()
        {
            if (gameOver) return;
            BlockList.Instance.NextBlock(isLeft);
            block = BlockList.Instance.GetBlock(isLeft);

            row = Pile.ROW_SIZE - 2;
            col = (Pile.COL_SIZE + 1) / 2;
            if (pile.IsBlockCollision(row, col, block))
            {
                gameOver = true;
            }
        }
        protected virtual void Drop()
        {
            if (gameOver) return;
            pile.DropBlock(row, col, block);
            //TODO: SimulateChain의 결과 처리하기
            ChainResult chainResult = pile.SimulateChain();
            BlockRegen();
        }

        
    }
}
