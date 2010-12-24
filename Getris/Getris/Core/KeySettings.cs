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

        private static Keys keyRotateCw1 = Keys.X;
        private static Keys? keyRotateCw2 = Keys.Up;
        private static Keys keyRotateCcw1 = Keys.Z;
        private static Keys? keyRotateCcw2 = null;
        private static Keys keyMoveLeft = Keys.Left;
        private static Keys keyMoveRight = Keys.Right;
        private static Keys keyMoveDown = Keys.Down;
        private static Keys keyDrop = Keys.Space;

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

        static KeySettings()
        {
        }

        /*
        private class KeySet {
            private static Keys? key;
            private 
        }*/
    }
}
