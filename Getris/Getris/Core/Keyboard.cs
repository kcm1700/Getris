namespace getris.Core
{
    public static sealed class KeyboardSingleton
    {
        static private System.Object thisLock = new System.Object();
        static private Keyboard instance;
        static private KeyboardSingleton()
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
                        if(instance==null)
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
