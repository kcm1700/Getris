using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace getris.GameState
{
    abstract class Game
    {
        protected Pile pile;
        protected Block block;// 현재 움직이는 블럭
        protected int row;
        protected int col;
        protected bool isLeft;

        protected decimal score;

        public System.Threading.Thread thread;
        public abstract void Start();

        public Game(bool isLeft=true)
        {
            this.isLeft = isLeft;
            score = 0;
            pile = new Pile();
            row = Pile.ROW;
            col = (Pile.COL+1) / 2;
            if (isLeft)
            {
                block = BlockList.Instance.LeftBlock;
            }
            else
            {
                block = BlockList.Instance.RightBlock;
            }
        }

        public virtual CellColor GetPileCellColor(int row, int col)
        {
            return pile.GetCellColor(row, col);
        }

        public virtual CellColor GetBlockCellColor(int row, int col)
        {
            return block.GetCell(row, col).maskColor;
        }

        public virtual decimal Score
        {
            get
            {
                return score;
            }
            set
            {
                score = value;
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

        public virtual void Rotate(bool isCw)
        {
            //TODO: validation
            if (isCw)
            {
                block.RotateCw();
            }
            else
            {
                block.RotateCcw();
            }
        }
        public virtual void Drop()
        {
            pile.DropBlock(row, col, block);
            //TODO: block = BlockList.Instance.NextBlock(isLeft);
        }
        public virtual void MoveDown()
        {
            //TODO: validation
            // vaildation 실패시 Drop으로
            this.row--;
        }
        public virtual void MoveLeft()
        {
            //TODO: validation
            this.col--;
        }
        public virtual void MoveRight()
        {
            //TODO: validation
            this.col++;
        }
        public virtual void GoTo(int row, int col)
        {
            this.row = row;
            this.col = col;
        }
    }
}
