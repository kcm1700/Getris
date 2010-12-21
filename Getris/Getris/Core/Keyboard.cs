using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace getris.Core
{
    static class KeyboardSingleton
    {
        static private Object thisLock = new Object();
        static private Keyboard instance;
        static private KeyboardSingleton()
        {
        }
        static Keyboard Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (thisLock)
                    {
                        instance = new Keyboard();
                    }
                }
                return instance;
            }
        }

    }
    class Keyboard
    {
    }
}
