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
                while (true)
                {
                    //일단 lock 걸고 작업하고,
                    lock (thisLock)
                    {
                        if (!Core.Keyboard.IsEmpty())
                        // is not empty
                        {
                            Core.Action a = Core.Keyboard.Pop();
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
                    }
                    // lock 풀고 Thread 양보하자.
                    Thread.Sleep(1);
                }
            }
            catch(ThreadAbortException e)
            {
            }
        }
    }
}
