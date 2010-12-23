namespace getris.Core
{
    public sealed class Keyboard
    {
        static private Keyboard instance = null;
        static private readonly System.Object thisLock = new System.Object();
        static Keyboard()
        {
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
    }
}
