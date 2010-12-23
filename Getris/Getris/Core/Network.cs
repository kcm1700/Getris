namespace getris.Core
{
    public sealed class Network
    {
        static private Network instance = null;
        static private readonly System.Object thisLock = new System.Object();
        static Network()
        {
        }
        static public Network Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (thisLock)
                    {
                        if (instance == null)
                            instance = new Network();
                    }
                }
                return instance;
            }
        }
    }
}
