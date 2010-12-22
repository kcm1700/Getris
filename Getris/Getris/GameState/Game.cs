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

        protected decimal score;

        protected System.Threading.Thread thread;
        protected abstract void Start();

        public virtual CellColor GetPileCellColor(int row, int col)
        {
            return pile.GetCellColor(row, col);
        }

        public virtual decimal Score
        {
            get
            {
                return score;
            }
            set
            {
                score = Score;
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

        public virtual void Rotate()
        {
            //TODO: 짜기
        }

        public virtual void MoveDown()
        {
            this.row--;
        }
        public virtual void MoveLeft()
        {
            this.col--;
        }
        public virtual void MoveRight()
        {
            this.col++;
        }
        public virtual void GoTo(int row, int col)
        {
            this.row = row;
            this.col = col;
        }
    }
}
