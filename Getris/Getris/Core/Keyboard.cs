using System;
using System.Threading;
using System.Collections.Generic;
using System.Collections;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;

namespace getris.Core
{
    public sealed class Keyboard
    {

        static private Keyboard instance;
        static private readonly System.Object thisLock;
        static private System.Collections.Generic.Queue<Action> buffer;

        static bool isEnabled;
        public enum InputModes{
            Game,
            Menu
        }

        static private InputModes inputMode;

        static public InputModes InputMode {
            get
            {
                lock (iglock)
                {
                    return inputMode;
                }
            }
            set
            {
                lock (iglock)
                {
                    inputMode = value;
                }
            }
        }

        static private System.Object iglock;

        static public Thread keyboardThread;

        static public bool Enabled
        {
            get
            {
                lock (iglock)
                {
                    return isEnabled;
                }
            }
            set
            {
                lock (iglock)
                {
                    isEnabled = value;
                }
            }
        }


        static public void KeyReset()
        {
            lock (keymapLock)
            {
                lock (KeySettings.thisLock)
                {
                    keyDrop = new KeyState(KeySettings.InitTimeDrop, KeySettings.RepeatPeriodDrop);
                    keyMoveDown = new KeyState(KeySettings.InitTimeMoveDown, KeySettings.RepeatPeriodMoveDown);
                    keyMoveLeft = new KeyState(KeySettings.InitTimeMoveLeft, KeySettings.RepeatPeriodMoveLeft);
                    keyMoveRight = new KeyState(KeySettings.InitTimeMoveRight, KeySettings.RepeatPeriodMoveRight);
                    keyRotateCw1 = new KeyState(KeySettings.InitTimeRotateCw1, KeySettings.RepeatPeriodRotateCw1);
                    keyRotateCcw1 = new KeyState(KeySettings.InitTimeRotateCcw1, KeySettings.RepeatPeriodRotateCcw1);
                    keyRotateCw2 = new KeyState(KeySettings.InitTimeRotateCw2, KeySettings.RepeatPeriodRotateCw2);
                    keyRotateCcw2 = new KeyState(KeySettings.InitTimeRotateCcw2, KeySettings.RepeatPeriodRotateCcw2);

                    isPressed.Clear();
                    isPressed[KeySettings.KeyDrop] = false;
                    isPressed[KeySettings.KeyMoveDown] = false;
                    isPressed[KeySettings.KeyMoveLeft] = false;
                    isPressed[KeySettings.KeyMoveRight] = false;
                    isPressed[KeySettings.KeyRotateCcw1] = false;
                    isPressed[KeySettings.KeyRotateCw1] = false;

                    //secondary key check
                    if (KeySettings.KeyRotateCcw2.HasValue)
                        isPressed[KeySettings.KeyRotateCcw2.Value] = false;
                    if (KeySettings.KeyRotateCw2.HasValue)
                        isPressed[KeySettings.KeyRotateCw2.Value] = false;

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
            KeyReset();
        }

        static public void Start()
        {
            //stopwatch
            sw.Start();
            //thread
            keyboardThread.Start();
        }

        static public System.Diagnostics.Stopwatch sw;

        struct KeyState{
            public int initTime;
            public int repeatPeriod;

            public KeyState(int initTime, int repeatPeriod)
            {
                this.initTime = initTime;
                this.repeatPeriod = repeatPeriod;
                this.before = false;
                this.now = false;
                this.isRepeated = false;
                this.restTime = 0;
            }

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
                    before = true;
                    return true;
                }
                if (isRepeated)
                {
                    if (restTime > this.repeatPeriod)
                    {
                        restTime -= this.repeatPeriod;
                        return true;
                    }
                }
                else
                {
                    if (restTime > this.initTime)
                    {
                        isRepeated = true;
                        restTime -= this.initTime;
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

        static private KeyState keyDrop, keyMoveDown, keyMoveLeft, keyMoveRight, keyRotateCw1, keyRotateCcw1, keyRotateCw2, keyRotateCcw2;
        static void threadLoop()
        {
            try
            {
                double beforeElapsed = sw.Elapsed.TotalMilliseconds;
                KeyReset();

                while (true)
                {
                    if (!isEnabled)
                    {
                        Thread.Sleep(1);
                        continue;
                    }

                    switch (InputMode)
                    {
                        case InputModes.Menu:
                            DoMenuInput(ref beforeElapsed);
                            break;
                        case InputModes.Game:
                            DoGameInput(ref beforeElapsed);
                            break;
                    }

                    Thread.Sleep(1);
                }
            }
            catch/*(ThreadAbortException e)*/
            {
            }
        }

        static void DoMenuInput(ref double beforeElapsed)
        {
            lock (keymapLock)
            {
                lock (KeySettings.thisLock)
                {
                    keyDrop.newState = isPressed[KeySettings.KeyDrop];
                    keyMoveDown.newState = isPressed[KeySettings.KeyMoveDown];
                    keyMoveLeft.newState = isPressed[KeySettings.KeyMoveLeft];
                    keyMoveRight.newState = isPressed[KeySettings.KeyMoveRight];
                    keyRotateCw1.newState = isPressed[KeySettings.KeyRotateCw1];
                    keyRotateCcw1.newState = isPressed[KeySettings.KeyRotateCcw1];
                    if (KeySettings.KeyRotateCw2.HasValue)
                        keyRotateCw2.newState = isPressed[KeySettings.KeyRotateCw2.Value];
                    if (KeySettings.KeyRotateCcw2.HasValue)
                        keyRotateCcw2.newState = isPressed[KeySettings.KeyRotateCcw2.Value];
                }
            }
            double nowElapsed = sw.Elapsed.TotalMilliseconds;
            double tmspan = nowElapsed - beforeElapsed;
            beforeElapsed = nowElapsed;

            while (keyDrop.keyPress(tmspan))
            {
                Add(new Move("drop"));
            }
            while (keyMoveDown.keyPress(tmspan))
            {
                Add(new Move("down"));
            }
            while (keyMoveLeft.keyPress(tmspan))
            {
                Add(new Move("left"));
            }
            while (keyMoveRight.keyPress(tmspan))
            {
                Add(new Move("right"));
            }
            while (keyRotateCw1.keyPress(tmspan))
            {
                Add(new Rotate("cw"));
            }
            while (keyRotateCcw1.keyPress(tmspan))
            {
                Add(new Rotate("ccw"));
            }
            while (keyRotateCw2.keyPress(tmspan))
            {
                Add(new Rotate("cw"));
            }
            while (keyRotateCcw2.keyPress(tmspan))
            {
                Add(new Rotate("ccw"));
            }
        }

        static void DoGameInput(ref double beforeElapsed)
        {
            lock (keymapLock)
            {
                lock (KeySettings.thisLock)
                {
                    keyDrop.newState = isPressed[KeySettings.KeyDrop];
                    keyMoveDown.newState = isPressed[KeySettings.KeyMoveDown];
                    keyMoveLeft.newState = isPressed[KeySettings.KeyMoveLeft];
                    keyMoveRight.newState = isPressed[KeySettings.KeyMoveRight];
                    keyRotateCw1.newState = isPressed[KeySettings.KeyRotateCw1];
                    keyRotateCcw1.newState = isPressed[KeySettings.KeyRotateCcw1];
                    if (KeySettings.KeyRotateCw2.HasValue)
                        keyRotateCw2.newState = isPressed[KeySettings.KeyRotateCw2.Value];
                    if (KeySettings.KeyRotateCcw2.HasValue)
                        keyRotateCcw2.newState = isPressed[KeySettings.KeyRotateCcw2.Value];
                }
            }

            double nowElapsed = sw.Elapsed.TotalMilliseconds;
            double tmspan = nowElapsed - beforeElapsed;
            beforeElapsed = nowElapsed;

            while (keyDrop.keyPress(tmspan))
            {
                Add(new Move("drop"));
            }
            while (keyMoveDown.keyPress(tmspan))
            {
                Add(new Move("down"));
            }
            while (keyMoveLeft.keyPress(tmspan))
            {
                Add(new Move("left"));
            }
            while (keyMoveRight.keyPress(tmspan))
            {
                Add(new Move("right"));
            }
            while (keyRotateCw1.keyPress(tmspan))
            {
                Add(new Rotate("cw"));
            }
            while (keyRotateCcw1.keyPress(tmspan))
            {
                Add(new Rotate("ccw"));
            }
            while (keyRotateCw2.keyPress(tmspan))
            {
                Add(new Rotate("cw"));
            }
            while (keyRotateCcw2.keyPress(tmspan))
            {
                Add(new Rotate("ccw"));
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
//                Logger.Write(action.data);
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
