using System;
using System.Threading;
using System.Collections.Generic;
using System.Collections;
using System.Runtime.InteropServices;

namespace getris.Core
{
    public sealed class Keyboard
    {
        static private Keyboard instance;
        static private readonly System.Object thisLock;
        static private System.Collections.Generic.Queue<Action> buffer;

        static bool isGame;
        static private System.Object iglock;

        static public Thread keyboardThread;

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
            instance = null;
            thisLock = new Object();
            keymapLock = new Object();
            buffer = new Queue<Action>();
            iglock = new System.Object();
            sw = new System.Diagnostics.Stopwatch();
            keyboardThread = new Thread(new ThreadStart(threadLoop));
            isPressed = new Dictionary<System.Windows.Forms.Keys, bool>();
            lock(keymapLock){
                isPressed.Add(System.Windows.Forms.Keys.Up, false);
                isPressed.Add(System.Windows.Forms.Keys.Down, false);
                isPressed.Add(System.Windows.Forms.Keys.Left, false);
                isPressed.Add(System.Windows.Forms.Keys.Right, false);
                isPressed.Add(System.Windows.Forms.Keys.X, false);
                isPressed.Add(System.Windows.Forms.Keys.Z, false);
                isPressed.Add(System.Windows.Forms.Keys.Space, false);
            }
        }

        static public void Start()
        {
            //stopwatch
            sw.Start();
            //thread
            keyboardThread.Start();
        }

        public const int initTime = 250;
        public const int repeatPeriod = 100;
        static public System.Diagnostics.Stopwatch sw;

        struct KeyState{
            public bool before;
            public bool now;

            public bool isRepeated;
            public double restTime;

            public bool newState {
                set{
                    before = now;
                    now = value;
                }
            }

            public bool keyPress (double timeDelta)
            {
                if (now == false)
                {
                    isRepeated = false;
                    restTime = 0;
                    return false;
                }
                restTime += timeDelta;
                if (before == false){
                    return true;
                }
                if (isRepeated)
                {
                    if (restTime > repeatPeriod)
                    {
                        restTime -= repeatPeriod;
                        return true;
                    }
                }
                else
                {
                    isRepeated = true;
                    if (restTime > initTime)
                    {
                        restTime -= initTime;
                        return true;
                    }
                }
                return false;
            }
        }

        static Object keymapLock;
        static System.Collections.Generic.Dictionary<System.Windows.Forms.Keys,bool> isPressed;

        static public void KeyDown(System.Windows.Forms.Keys key)
        {
            lock (keymapLock)
            {
                isPressed[key] = true;
            }
        }
        static public void KeyUp(System.Windows.Forms.Keys key)
        {
            lock (keymapLock)
            {
                isPressed[key] = false;
            }
        }

        static void threadLoop()
        {
            try
            {
                KeyState up, down, left, right, x, z, space;
                double beforeElapsed = sw.Elapsed.TotalMilliseconds;

                space = new KeyState();
                up = new KeyState();
                down = new KeyState();
                left = new KeyState();
                right = new KeyState();
                x = new KeyState();
                z = new KeyState();
                space.restTime = 0;
                up.restTime = 0;
                down.restTime = 0;
                left.restTime = 0;
                right.restTime = 0;
                x.restTime = 0;
                z.restTime = 0;

                while (true)
                {
                    if (!isGame)
                    {
                        Thread.Sleep(1);
                        continue;
                    }

                    lock (keymapLock)
                    {
                        up.newState = isPressed[System.Windows.Forms.Keys.Up];
                        down.newState = isPressed[System.Windows.Forms.Keys.Down];
                        left.newState = isPressed[System.Windows.Forms.Keys.Left];
                        right.newState = isPressed[System.Windows.Forms.Keys.Right];
                        x.newState = isPressed[System.Windows.Forms.Keys.X];
                        z.newState = isPressed[System.Windows.Forms.Keys.Z];
                        space.newState = isPressed[System.Windows.Forms.Keys.Space];
                    }

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
                    if (space.keyPress(tmspan))
                    {
                        Add(new Move("drop"));
                    }
                    Thread.Sleep(1);
                }
            }
            catch(ThreadAbortException e)
            {
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
                buffer.Enqueue(action);
            }
        }

        static public bool IsEmpty()
        {
            lock (thisLock)
            {
                return buffer.Count == 0;
            }
        }
        static public Action Peek()
        {
            lock (thisLock)
            {
                return buffer.Peek();
            }
        }
        static public Action Pop()
        {
            lock (thisLock)
            {
                return buffer.Dequeue();
            }
        }
    }
}
