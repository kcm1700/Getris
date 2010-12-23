namespace getris.Core
{
    public sealed class Random
    {
        static private Random instance=null;
        static private System.Random rnd;
        static private readonly System.Object thisLock;
        static Random()
        {
            thisLock = new System.Object();
            rnd = new System.Random();
        }
        static public int rand()
        {
            if(instance==null)
            {
                lock(thisLock)
                {
                    if(instance==null)
                    {
                        instance=new Random();
                    }
                }
            }
            lock (thisLock)
            {
                return rnd.Next();
            }
        }
        static public int rand(int maxValue)
        {
            if (instance == null)
            {
                lock (thisLock)
                {
                    if (instance == null)
                    {
                        instance = new Random();
                    }
                }
            }
            lock (thisLock)
            {
                return rnd.Next(maxValue);
            }
        }
        static public int rand(int minValue, int maxValue)
        {
            if (instance == null)
            {
                lock (thisLock)
                {
                    if (instance == null)
                    {
                        instance = new Random();
                    }
                }
            }
            lock (thisLock)
            {
                return rnd.Next(minValue, maxValue);
            }
        }
    }
}
