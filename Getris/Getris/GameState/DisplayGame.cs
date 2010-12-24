using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace getris.GameState
{
    class DisplayGame : Game
    {
        public DisplayGame(bool isLeft = false) : base(isLeft)
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
                while (true)
                {
                    lock (thisLock)
                    {
                    }
                    Thread.Sleep(1);
                }
                //TODO:
                /*
                  while (true)
                  {
                    if (!Core.Keyboard.IsEmpty())
                    {
                        Core.Action a = Core.Network.BufferPop();
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
                                    throw new Exception("unknown move");
                            }
                        }
                        else if (a is Core.GoTo)
                        {
                            String[] arr = a.data.Split(':');
                            if (arr.Length != 2)
                            {
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
                                    throw new Exception("unknown rotation");
                            }
                        }
                    }
                    Thread.Sleep(1);
                }*/
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
            return true;
        }
    }
}
