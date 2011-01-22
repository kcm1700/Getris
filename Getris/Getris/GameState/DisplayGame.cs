using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace getris.GameState
{
    class DisplayGame : Game
    {
        private int old_row;
        private int old_col;
        public DisplayGame(bool isLeft = false) : base(isLeft)
        {
            thread = new Thread(new ThreadStart(ThreadWork));
            thread.Name = "DISPLAY" + (isLeft ? ":left" : ":right");
            old_row = -1;
            old_col = -1;
        }
        public override void Start()
        {
            thread.Start();
        }
        void ThreadWork()
        {
            try
            {
                while (true)
                {
                    Thread.Sleep(1);
                    //일단 lock 걸고 작업하고,
                    lock (thisLock)
                    {
                        Core.Action a = Core.Network.Instance.NextGame();
                        if (a is Core.NullAction)
                            continue;
                        if (a is Core.Turn)
                        {
                            bool isLeft = a.data == "left";
                            if (isLeft == this.isLeft)
                            {
                                base.Drop();
                            }
                        }
                        else if (a is Core.Rotate)
                        {
                            string[] data = a.data.Split(':');
                            bool isLeft = data[0] == "left";
                            if (isLeft == this.isLeft)
                            {
                                if (!Rotate(data[1] == "cw"))
                                {
                                    //돌릴 수 없는데 돌리라고 한 경우.
                                }
                                else
                                {
                                }
                            }
                        }
                        else if (a is Core.GoTo)
                        {
                            int row, col;
                            if (IsGoTo(a.data, out row, out col))
                            {
                                if (old_row == row && old_col == col)
                                    continue;
                                GoTo(row, col);
                            }
                        }
                    }
                    // lock 풀고 Thread 양보하자.
                    // TODO: 이거 안하면 thread양보 안하나?
                }
            }
            catch/*(ThreadAbortException e)*/
            {
            }
        }
        private bool GoTo(int row, int col)
        {
            if (isAnimationMode) return false;
            if (gameOver) return false;
            this.row = row;
            this.col = col;
            if(pile.IsBlockCollision(row,col,block))
                return false;
            pile.CalcGhost(ghostInfoRow, row, col, block);
            return true;
        }
        protected override bool Rotate(bool isCw)
        {
            bool ret = base.Rotate(isCw);
            return ret;
        }
        private bool IsGoTo(string data, out int row, out int col)
        {
            row=col=-1;
            string[] user_row_col = data.Split(':');
            if (user_row_col.Count() != 3)
                return false;
            bool isLeft = user_row_col[0] == "left";
            if (isLeft != this.isLeft)
                return false;
            row = Convert.ToInt32(user_row_col[1])-1;
            col = Convert.ToInt32(user_row_col[2])-1;
            //TODO: 그래픽 찍기전에 valid check를 하니 여기서 또 할 필요는 없을것 같음.
            return true;
        }
    }
}
