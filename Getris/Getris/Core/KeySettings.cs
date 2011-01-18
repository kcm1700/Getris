using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace getris.Core
{
    /// <summary>
    /// thread unsafe key settings class
    /// </summary>

    public static class KeySettings
    {
        public static Object thisLock = new Object();
        
        private static int initTime;
        private static int repeatPeriod;

        private static KeySet keyRotateCw1;
        private static KeySet keyRotateCw2;
        private static KeySet keyRotateCcw1;
        private static KeySet keyRotateCcw2;
        private static KeySet keyMoveLeft;
        private static KeySet keyMoveRight;
        private static KeySet keyMoveDown;
        private static KeySet keyDrop;

        public static int InitTimeRotateCw1
        {
            get
            {
                return keyRotateCw1.initTime;
            }
            set
            {
                keyRotateCw1.initTime = value;
            }
        }
        public static int InitTimeRotateCw2
        {
            get
            {
                return keyRotateCw2.initTime;
            }
            set
            {
                keyRotateCw2.initTime = value;
            }
        }

        public static int InitTimeRotateCcw1
        {
            get
            {
                return keyRotateCcw1.initTime;
            }
            set
            {
                keyRotateCcw1.initTime = value;
            }
        }
        public static int InitTimeRotateCcw2
        {
            get
            {
                return keyRotateCcw2.initTime;
            }
            set
            {
                keyRotateCcw2.initTime = value;
            }
        }

        public static int InitTimeMoveLeft
        {
            get
            {
                return keyMoveLeft.initTime;
            }
            set
            {
                keyMoveLeft.initTime = value;
            }
        }
        public static int InitTimeMoveRight
        {
            get
            {
                return keyMoveRight.initTime;
            }
            set
            {
                keyMoveRight.initTime = value;
            }
        }

        public static int InitTimeMoveDown
        {
            get
            {
                return keyMoveDown.initTime;
            }
            set
            {
                keyMoveDown.initTime = value;
            }
        }
        public static int InitTimeDrop
        {
            get
            {
                return keyDrop.initTime;
            }
            set
            {
                keyDrop.initTime = value;
            }
        }


        public static int RepeatPeriodRotateCw1
        {
            get
            {
                return keyRotateCw1.repeatPeriod;
            }
            set
            {
                keyRotateCw1.repeatPeriod = value;
            }
        }
        public static int RepeatPeriodRotateCw2
        {
            get
            {
                return keyRotateCw2.repeatPeriod;
            }
            set
            {
                keyRotateCw2.repeatPeriod = value;
            }
        }

        public static int RepeatPeriodRotateCcw1
        {
            get
            {
                return keyRotateCcw1.repeatPeriod;
            }
            set
            {
                keyRotateCcw1.repeatPeriod = value;
            }
        }
        public static int RepeatPeriodRotateCcw2
        {
            get
            {
                return keyRotateCcw2.repeatPeriod;
            }
            set
            {
                keyRotateCcw2.repeatPeriod = value;
            }
        }

        public static int RepeatPeriodMoveLeft
        {
            get
            {
                return keyMoveLeft.repeatPeriod;
            }
            set
            {
                keyMoveLeft.repeatPeriod = value;
            }
        }
        public static int RepeatPeriodMoveRight
        {
            get
            {
                return keyMoveRight.repeatPeriod;
            }
            set
            {
                keyMoveRight.repeatPeriod = value;
            }
        }

        public static int RepeatPeriodMoveDown
        {
            get
            {
                return keyMoveDown.repeatPeriod;
            }
            set
            {
                keyMoveDown.repeatPeriod = value;
            }
        }
        public static int RepeatPeriodDrop
        {
            get
            {
                return keyDrop.repeatPeriod;
            }
            set
            {
                keyDrop.repeatPeriod = value;
            }
        }


        public static Keys KeyRotateCw1
        {
            get
            {
                return keyRotateCw1.assigned.Value;
            }
            set
            {
                keyRotateCw1.assigned = value;
                Keyboard.Instance.KeyReset();
            }
        }
        public static Keys? KeyRotateCw2
        {
            get
            {
                return keyRotateCw2.assigned;
            }
            set
            {
                keyRotateCw2.assigned = value;
                Keyboard.Instance.KeyReset();
            }
        }
        public static Keys KeyRotateCcw1
        {
            get
            {
                return keyRotateCcw1.assigned.Value;
            }
            set
            {
                keyRotateCcw1.assigned = value;
                Keyboard.Instance.KeyReset();
            }
        }
        public static Keys? KeyRotateCcw2
        {
            get
            {
                return keyRotateCcw2.assigned;
            }
            set
            {
                keyRotateCcw2.assigned = value;
                Keyboard.Instance.KeyReset();
            }
        }
        public static Keys KeyMoveLeft
        {
            get
            {
                return keyMoveLeft.assigned.Value;
            }
            set
            {
                keyMoveLeft.assigned = value;
                Keyboard.Instance.KeyReset();
            }
        }
        public static Keys KeyMoveRight
        {
            get
            {
                return keyMoveRight.assigned.Value;
            }
            set
            {
                keyMoveRight.assigned = value;
                Keyboard.Instance.KeyReset();
            }
        }
        public static Keys KeyMoveDown
        {
            get
            {
                return keyMoveDown.assigned.Value;
            }
            set
            {
                keyMoveDown.assigned = value;
                Keyboard.Instance.KeyReset();
            }
        }
        public static Keys KeyDrop
        {
            get
            {
                return keyDrop.assigned.Value;
            }
            set
            {
                keyDrop.assigned = value;
                Keyboard.Instance.KeyReset();
            }
        }

        public static int InitTime
        {
            get
            {
                return initTime;
            }
            set
            {
                initTime = value;
            }
        }
        public static int RepeatPeriod
        {
            get
            {
                return repeatPeriod;
            }
            set
            {
                repeatPeriod = value;
            }
        }
        static KeySettings()
        {
            keyRotateCw1 = new KeySet(Keys.X,500,100);
            keyRotateCw2 = new KeySet(Keys.Up,500,100);
            keyRotateCcw1 = new KeySet(Keys.Z,500,100);
            keyRotateCcw2 = new KeySet(Keys.Enter, 500, 100);
            keyMoveLeft = new KeySet(Keys.Left,200,50);
            keyMoveRight = new KeySet(Keys.Right, 200, 50);
            keyMoveDown = new KeySet(Keys.Down, 200, 50);
            keyDrop = new KeySet(Keys.Space, 600, 100);
            initTime = 250;
            repeatPeriod = 150;
        }
    }

    public struct KeySet
    {
        public Keys? assigned;
        public int initTime;
        public int repeatPeriod;

        public KeySet(Keys? key, int initTime, int repeatPeriod)
        {
            this.assigned = key;
            this.initTime = initTime;
            this.repeatPeriod = repeatPeriod;
        }
    }
}
