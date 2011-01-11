namespace getris.Core
{
    /// <summary>
    /// <para>chatBuffer</para>for receive chatting message
    /// <para>gameBuffer</para>for receive display game
    /// </summary>
    public sealed class Network
    {
        static private Network instance = null;
        static private readonly System.Object thisLock;
        static private System.Object gameLock;
        private System.Collections.Generic.Queue<Action> sendBuffer;
        private System.Collections.Generic.Queue<Action> chatBuffer;
        private System.Collections.Generic.Queue<Action> gameBuffer;
        static Network()
        {
            thisLock = new System.Object();
            gameLock = new System.Object();
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
        public bool GameIsEmpty()
        {
            lock (gameLock)
            {
                return gameBuffer.Count == 0;
            }
        }
        public void ClearGame()
        {
            lock (gameLock)
            {
                gameBuffer.Clear();
            }
        }
        public Action PopGame()
        {
            lock (gameLock)
            {
                if (gameBuffer.Count == 0)
                {
                    return new NullAction();
                }
                else
                {
                    Action a = gameBuffer.Dequeue();
                    if (a.IsValid())
                        return a;
                    else
                        return new NullAction();
                }
            }
        }
        public Action PeekGame()
        {
            lock (gameLock)
            {
                if (gameBuffer.Count == 0)
                {
                    return new NullAction();
                }
                else
                {
                    Action a = gameBuffer.Peek();
                    if (a.IsValid())
                    {
                        return a;
                    }
                    else
                    {
                        gameBuffer.Dequeue();
                        return new NullAction();
                    }
                }
            }
        }
        public void AddGame(Action action)
        {
            lock (gameLock)
            {
                gameBuffer.Enqueue(action);
            }
        }

        public bool SendIsEmpty()
        {
            lock (thisLock)
            {
                return sendBuffer.Count == 0;
            }
        }
        public void ClearSend()
        {
            lock (thisLock)
            {
                sendBuffer.Clear();
            }
        }
        public Action PopSend()
        {
            lock (thisLock)
            {
                if (sendBuffer.Count == 0)
                {
                    return new NullAction();
                }
                else
                {
                    Action a = sendBuffer.Dequeue();
                    if (a.IsValid())
                        return a;
                    else
                        return new NullAction();
                }
            }
        }
        public Action PeekSend()
        {
            lock (thisLock)
            {
                if (sendBuffer.Count == 0)
                {
                    return new NullAction();
                }
                else
                {
                    Action a = sendBuffer.Peek();
                    if (a.IsValid())
                    {
                        return a;
                    }
                    else
                    {
                        sendBuffer.Dequeue();
                        return new NullAction();
                    }
                }
            }
        }
        public void AddSend(Action action)
        {
            lock (thisLock)
            {
                sendBuffer.Enqueue(action);
            }
        }
    }
}
