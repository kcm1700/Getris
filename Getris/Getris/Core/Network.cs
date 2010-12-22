namespace getris.Core
{
    public static sealed class NetworkSingleton
    {
        static private Network instance;
        static private System.Object thisLock = new System.Object();
        static private NetworkSingleton()
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
