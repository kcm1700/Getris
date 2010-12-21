using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace getris.Core
{
    static class NetworkSingleton
    {
        static private Network instance;
        static private Object thisLock = new Object();
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
