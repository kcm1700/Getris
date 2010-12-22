namespace getris.Core
{
    public static class NetworkSingleton
    {
        static private Network instance;
        static private System.Object thisLock = new System.Object();
        static NetworkSingleton()
        {
        }
        static Network Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (thisLock)
                    {
                        if(instance==null)
                            instance = new Network();
                    }
                }
                return instance;
            }
        }

    }
    class Network
    {
    }
}
