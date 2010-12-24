using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace getris.GameState
{
    class RunGame : Game
    {
        public RunGame(bool isLeft = true) : base(isLeft)
        {
            thread = new Thread(new ThreadStart(ThreadWork));
        }
        public override void Start()
        {
            thread.Start();
        }
        void ThreadWork()
        {
            try
            {
                System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
                sw.Start();
                double before = sw.Elapsed.TotalMilliseconds;
                double now;
                while (true)
                {
                    //일단 lock 걸고 작업하고,
                    lock (thisLock)
                    {
                        if (!Core.Keyboard.IsEmpty())
                        // is not empty
                        {
                            switch (Core.Keyboard.Peek().data)
                            {
                                case "down":
                                    if (MoveDown())
                                    {
                                        Core.Keyboard.Pop();
                                    }
                                    break;
                                case "left":
                                    if (MoveLeft())
                                    {
                                        Core.Keyboard.Pop();
                                    }
                                    break;
                                case "right":
                                    if (MoveRight())
                                    {
                                        Core.Keyboard.Pop();
                                    }
                                    break;
                                case "drop":
                                    if (Drop())
                                    {
                                        Core.Keyboard.Pop();
                                    }
                                    break;
                                case "cw":
                                    if (Rotate(true))
                                    {
                                        Core.Keyboard.Pop();
                                    }
                                    break;
                                case "ccw":
                                    if (Rotate(false))
                                    {
                                        Core.Keyboard.Pop();
                                    }
                                    break;
                                default:
                                    throw new Exception("unknown action");
                                //Action(Core.Keyboard.Pop());
                            }
                        }
                        now =sw.Elapsed.TotalMilliseconds;
                        TimeLimit(now, ref before, 700);
                        
                    }
                    // lock 풀고 Thread 양보하자.
                    Thread.Sleep(1);
                }
            }
            catch/*(ThreadAbortException e)*/
            {
            }
        }
        private void TimeLimit(double now, ref double before, double limitTime)
        {
            if (limitTime > now - before)
                return;
            before = now;
            MoveDown();
        }
        private void Action(Core.Move a)
        {
            switch (a.data)
            {
                case "down":
                    MoveDown();
                    break;
                case "left":
                    MoveLeft();
                    break;
                case "right":
                    MoveRight();
                    break;
                case "drop":
                    Drop();
                    break;
                default:
                    throw new Exception("unknown move");
            }
        }
        private void Action(Core.Rotate a)
        {
            switch (a.data)
            {
                case "cw":
                    Rotate(true);
                    break;
                case "ccw":
                    Rotate(false);
                    break;
                default:
                    throw new Exception("unknown action");
            }
        }
        private void Action(Core.Attack a)
        {
        }
        private void Action(Core.Chat a)
        {
        }
        private void Action(Core.GoTo a)
        {
        }
        private void Action(Core.Turn a)
        {
        }

        private bool MoveDown()
        {
            if (animationMode) return false;
            if (gameOver) return false;
            // vaildation 실패시 Drop으로
            this.row--;
            if (pile.IsBlockCollision(row, col, block))
            {
                this.row++;
                Drop();
            }
            else
            {
                //succeeded
            }
            return true;
        }
        private bool MoveLeft()
        {
            if (animationMode) return false;
            if (gameOver) return false;
            this.col--;
            if (pile.IsBlockCollision(row, col, block))
            {
                this.col++;
            }
            else
            {
                pile.CalcGhost(ghostInfoRow, row, col, block);
                //succeeded
            }
            return true;
        }
        private bool MoveRight()
        {
            if (animationMode) return false;
            if (gameOver) return false;
            this.col++;
            if (pile.IsBlockCollision(row, col, block))
            {
                this.col--;
            }
            else
            {
                pile.CalcGhost(ghostInfoRow, row, col, block);
                //succeeded
            }
            return true;
        }
    }
}
