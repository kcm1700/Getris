namespace getris.Core
{
    public static class KeyboardSingleton
    {
        static private System.Object thisLock = new System.Object();
        static private Keyboard instance;
        static KeyboardSingleton()
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
                        if(instance == null)
                            instance = new Keyboard();
                    }
                }
                return instance;
            }
        }
    }
    public class Keyboard
    {
    }
}
