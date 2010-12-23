namespace getris.Core
{
    /// <summary>
    /// <para>chatBuffer</para>for receive chatting message
    /// <para>gameBuffer</para>for receive display game
    /// </summary>
    public sealed class Network
    {
        static private Network instance = null;
        static private readonly System.Object thisLock = new System.Object();
        private System.Collections.Generic.Queue<Action> sendBuffer;
        private System.Collections.Generic.Queue<Action> chatBuffer;
        private System.Collections.Generic.Queue<Action> gameBuffer;
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
