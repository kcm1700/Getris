using System;
using System.Threading;
using System.Collections.Generic;
using System.Collections;
using System.Runtime.InteropServices;

namespace getris.Core
{
    public sealed class Keyboard
    {
        [DllImport("User32.dll")]
        private static extern short GetAsyncKeyState(System.Windows.Forms.Keys vKey); 

        static private Keyboard instance = null;
        static private readonly System.Object thisLock = new System.Object();
        static private System.Collections.Generic.Queue<Action> buffer = new Queue<Action>();

        static bool isGame;
        static private System.Object iglock = new System.Object();

        static private Thread keyboardThread = new Thread(new ThreadStart(threadLoop));

        static public bool IsGame
        {
            get
            {
                lock (iglock)
                {
                    return isGame;
                }
            }
            set
            {
                lock (iglock)
                {
                    isGame = value;
                }
            }
        }

        static Keyboard()
        {
            sw.Start();
        }

        static public void Start()
        {
            keyboardThread.Start();
        }

        public const int initTime = 400;
        public const int repeatPeriod = 200;
        static public System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

        struct KeyState{
            public short before;
            public short now;

            public bool isRepeated;
            public double restTime;

            public short newState {
                set{
                    before = now;
                    now = value;
                }
            }

            public bool keyPress (double timeDelta)
            {
                if (now <= 0)
                {
                    isRepeated = false;
                    restTime = 0;
                    return false;
                }
                restTime += timeDelta;
                if (before <= 0){
                    return true;
                }
                if (isRepeated)
                {
                    if (restTime < repeatPeriod)
                    {
                        restTime -= repeatPeriod;
                        return true;
                    }
                }
                else
                {
                    isRepeated = true;
                    if (restTime < initTime)
                    {
                        restTime -= initTime;
                        return true;
                    }
                }
                return false;
            }
        }

        static bool keyUp{

        }

        static void threadLoop()
        {
            KeyState up,down,left,right,x,z;
            double beforeElapsed = sw.Elapsed.TotalMilliseconds;

            up = new KeyState();
            down = new KeyState();
            left = new KeyState();
            right = new KeyState();
            x = new KeyState();
            z = new KeyState();

            while (true)
            {
                if (!isGame)
                {
                    Thread.Sleep(1);
                    continue;
                }
                up.newState = GetAsyncKeyState(System.Windows.Forms.Keys.Up);
                down.newState = GetAsyncKeyState(System.Windows.Forms.Keys.Down);
                left.newState = GetAsyncKeyState(System.Windows.Forms.Keys.Left);
                right.newState = GetAsyncKeyState(System.Windows.Forms.Keys.Right);
                x.newState = GetAsyncKeyState(System.Windows.Forms.Keys.X);
                z.newState = GetAsyncKeyState(System.Windows.Forms.Keys.Z);

                double nowElapsed = sw.Elapsed.TotalMilliseconds;
                double tmspan = nowElapsed - beforeElapsed;
                beforeElapsed = nowElapsed;

                if (up.keyPress(tmspan))
                {
                    //TODO: option based ccw or cw
                    Add(new Rotate("cw"));
                }
                if (down.keyPress(tmspan))
                {
                    Add(new Move("down"));
                }
                if (left.keyPress(tmspan))
                {
                    Add(new Move("left"));
                }
                if (right.keyPress(tmspan))
                {
                    Add(new Move("right"));
                }
                if (x.keyPress(tmspan))
                {
                    Add(new Rotate("cw"));
                }
                if (z.keyPress(tmspan))
                {
                    Add(new Rotate("ccw"));
                }

                Thread.Sleep(1);
            }
        }

        static public Keyboard Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (thisLock)
                    {
                        if (instance == null)
                            instance = new Keyboard();
                    }
                }
                return instance;
            }
        }

        static public void Add(Action action)
        {
            lock (thisLock)
            {
                Logger.Write(action.data);
                Logger.Write("\n");
                buffer.Enqueue(action);
            }
        }
        static public Action Peek()
        {
            lock(thisLock)
            {
                return buffer.Peek();
            }
        }
        static public Action Pop()
        {
            lock(thisLock)
            {
                return buffer.Dequeue();
            }
        }
    }
}
