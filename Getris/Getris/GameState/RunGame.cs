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
                        if (!Core.Keyboard.Instance.IsEmpty())
                        // is not empty
                        {
                            Core.Action a = Core.Keyboard.Instance.Peek();
                            if (a is Core.Move)
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
                                        throw new Exception("unknown action");
                                }
                            }
                            else if (a is Core.Rotate)
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
                            Core.Keyboard.Instance.Pop();
                        }
                        now =sw.Elapsed.TotalMilliseconds;
                        TimeLimit(now, ref before, 700);
                        LockTimeLimit(5000);
                    }
                    // lock 풀고 Thread 양보하자.
                    Thread.Sleep(1);
                }
            }
            catch/*(ThreadAbortException e)*/
            {
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="limitTime">in milli second</param>
        private void LockTimeLimit(double limitTime)
        {
            if (swLock.Elapsed.TotalMilliseconds > limitTime)
            {
                swLock.Reset();
                Drop();
            }
        }
        private void TimeLimit(double now, ref double before, double limitTime)
        {
            if (limitTime > now - before)
                return;
            before = now;
            MoveDown();
        }

        int lockCount = 25;
        System.Diagnostics.Stopwatch swLock = new System.Diagnostics.Stopwatch();

        private bool isLocked()
        {
            bool ret = false;
            this.row--;
            if(pile.IsBlockCollision(row,col,block))
            {
                ret = true;
            }
            this.row++;
            return ret;
        }

        private void CheckLockCnt()
        {
            if (isLocked() == false)
            {
                lockCount = 25;
                swLock.Stop();
            }
            if (swLock.IsRunning == true)
            {
                lockCount--;
                if (lockCount < 0)
                {
                    lockCount = 25;
                    Drop();
                }
            }
            else
            {
                if (isLocked())
                {
                    swLock.Start();
                    lockCount = 25;
                }
            }
        }

        protected override bool Drop()
        {
            //stop lock stopwatch
            swLock.Reset();
            lockCount = 25;
            string str = isLeft ? "left" : "right";
            Core.Network.Instance.Send(new Core.Turn(str));
            return base.Drop();
        }

        protected override bool Rotate(bool isCw)
        {
            bool ret = base.Rotate(isCw);
            if (ret == true)
            {
                CheckLockCnt();
                string str = isLeft ? "left:" : "right:";
                str += isCw ? "cw" : "ccw";
                Core.Network.Instance.Send(new Core.Rotate(str));
            }
            
            return ret;
        }

        private bool MoveDown()
        {
            if (animationMode) return false;
            if (gameOver) return false;
            // vaildation 실패시 Drop이 아니라 lock count decrease
            this.row--;
            if (pile.IsBlockCollision(row, col, block))
            {
                this.row++;
            }
            else
            {
                //succeeded
            }
            CheckLockCnt();
            this.SendMove();
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
            CheckLockCnt();
            this.SendMove();
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
            CheckLockCnt();
            this.SendMove();
            return true;
        }
        private void SendMove()
        {
            string str = isLeft ? "left:" : "right:";
            str += row;
            str += ":";
            str += col;
            Core.Network.Instance.Send(new Core.GoTo(str));
        }
    }
}
