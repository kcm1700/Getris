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

        private static Keys keyRotateCw1;
        private static Keys? keyRotateCw2;
        private static Keys keyRotateCcw1;
        private static Keys? keyRotateCcw2;
        private static Keys keyMoveLeft;
        private static Keys keyMoveRight;
        private static Keys keyMoveDown;
        private static Keys keyDrop;

        public static Keys KeyRotateCw1
        {
            get
            {
                return keyRotateCw1;
            }
            set
            {
                keyRotateCw1 = value;
                Keyboard.KeyReset();
            }
        }
        public static Keys? KeyRotateCw2
        {
            get
            {
                return keyRotateCw2;
            }
            set
            {
                keyRotateCw2 = value;
                Keyboard.KeyReset();
            }
        }
        public static Keys KeyRotateCcw1
        {
            get
            {
                return keyRotateCcw1;
            }
            set
            {
                keyRotateCcw1 = value;
                Keyboard.KeyReset();
            }
        }
        public static Keys? KeyRotateCcw2
        {
            get
            {
                return keyRotateCcw2;
            }
            set
            {
                keyRotateCcw2 = value;
                Keyboard.KeyReset();
            }
        }
        public static Keys KeyMoveLeft
        {
            get
            {
                return keyMoveLeft;
            }
            set
            {
                keyMoveLeft = value;
                Keyboard.KeyReset();
            }
        }
        public static Keys KeyMoveRight
        {
            get
            {
                return keyMoveRight;
            }
            set
            {
                keyMoveRight = value;
                Keyboard.KeyReset();
            }
        }
        public static Keys KeyMoveDown
        {
            get
            {
                return keyMoveDown;
            }
            set
            {
                keyMoveDown = value;
                Keyboard.KeyReset();
            }
        }
        public static Keys KeyDrop
        {
            get
            {
                return keyDrop;
            }
            set
            {
                keyDrop = value;
                Keyboard.KeyReset();
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
            keyRotateCw1 = Keys.X;
            keyRotateCw2 = Keys.Up;
            keyRotateCcw1 = Keys.Z;
            keyRotateCcw2 = null;
            keyMoveLeft = Keys.Left;
            keyMoveRight = Keys.Right;
            keyMoveDown = Keys.Down;
            keyDrop = Keys.Space;
            initTime = 250;
            repeatPeriod = 150;
        }
    }
}
