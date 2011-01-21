using System.Net.Sockets;
using System.Net;
using System;
using System.Threading;

namespace getris.Core
{
    enum NetworkMode
    {
        HOST, GUEST
    }
    /// <summary>
    /// <para>chatBuffer</para>for receive chatting message
    /// <para>gameBuffer</para>for receive display game
    /// </summary>
    public sealed class Network
    {
        private Thread networkThread;
        static private Network instance = null;
        static private readonly System.Object thisLock;
        static private readonly System.Object gameLock;
        static private readonly System.Object chatLock;
        private System.Collections.Generic.Queue<Chat> chatBuffer;
        private System.Collections.Generic.Queue<Action> gameBuffer;
        private NetworkMode mode;


        private NetworkStream stream;
        
        private int port;
        private string ip;
        static Network()
        {
            thisLock = new System.Object();
            gameLock = new System.Object();
            chatLock = new System.Object();
            }
        Network()
        {
            networkThread = null;
            mode = NetworkMode.HOST;
            chatBuffer = new System.Collections.Generic.Queue<Chat>();
            gameBuffer = new System.Collections.Generic.Queue<Action>();
            ip = "127.0.0.1";
            port = 10101;
            stream = null;
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
        public void Abort()
        {
            lock (thisLock)
            {
                if (networkThread != null)
                {
                    try
                    {
                        networkThread.Abort();
                    }
                    catch
                    {
                    }
                }
            }
        }
        public bool IsHost
        {
            get
            {
                lock (thisLock)
                {
                    if (this.mode == NetworkMode.HOST)
                        return true;
                    else
                        return false;
                }
            }
            set
            {
                lock (thisLock)
                {
                    if (value)
                        mode = NetworkMode.HOST;
                    else
                        mode = NetworkMode.GUEST;
                }
            }
        }
        public string IP
        {
            get
            {
                return this.ip;
            }
            set
            {
                ip = value;
            }
        }
        public string Port
        {
            get
            {
                return Convert.ToString(this.port);
            }
            set
            {
                port = Convert.ToInt32(value);                
            }
        }

        public void Open()
        {
            if (mode == NetworkMode.HOST)
            {
                Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                //TODO: port 설정하기 추가
                IPEndPoint ipep = new IPEndPoint(IPAddress.Any, port);
                server.Bind(ipep);
                //TODO: 여기 Listen 몇으로 해야 하나?
                server.Listen(1);
                Socket socket = server.Accept();

                stream = new NetworkStream(socket);
                SendBlocks();

                networkThread = new Thread(new ThreadStart(threadLoop));
                networkThread.Name = "NETWORK:SERVER";
            }
            else
            {
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPAddress serverIp = IPAddress.Parse(ip);
                IPEndPoint ipep = new IPEndPoint(serverIp, port);

                try
                {
                    socket.Connect(ipep);
                    stream = new NetworkStream(socket);
                    ReceiveBlocks();
                }
                catch (SocketException e)
                {
                    //TODO 에러나면 어떻게해?
                    Logger.WriteLine(e.Message);
                }

                networkThread = new Thread(new ThreadStart(threadLoop));
                networkThread.Name = "NETWORK:CLIENT";
            }
            networkThread.Start();
        }
        private void SendBlocks()
        {
            int size =GameState.BlockList.Instance.Size;
            byte[] send = BitConverter.GetBytes(size);
            
            stream.Write(send, 0, send.Length);
            for (int i = 0; i < size; i++)
            {
                int x =GameState.BlockList.Instance.Get(i);
                send = BitConverter.GetBytes(GameState.BlockList.Instance.Get(i));
                stream.Write(send,0,send.Length);
            }
            send = new byte[4];
            stream.Read(send, 0, 4);
            Keyboard.Instance.Clear();
        }
        private void ReceiveBlocks()
        {
            byte[] receive = new byte[4];
            
            stream.Read(receive, 0, 4);
            //TODO: 여기 endian에 따라 차이 생기지 않는지 확인하기.
            int size = BitConverter.ToInt32(receive,0);
            GameState.BlockList.Instance.Size =  size;
            for (int i = 0; i < size; i++)
            {
                stream.Read(receive, 0, 4);
                int x = BitConverter.ToInt32(receive, 0);
                GameState.BlockList.Instance.Set(i, x);
            }
            stream.Write(receive, 0, 4);
            Keyboard.Instance.Clear();
        }
        void threadLoop()
        {
            byte[] tmp = new byte[1];
            tmp[0]=0;
            stream.Write(tmp, 0, 1);
            try
            {
                while (true)
                {
                    Thread.Sleep(1);
                    Receive();
                }
            }
            catch (System.IO.IOException e)
            {
            }
        }

        public void ClearGame()
        {
            lock (gameLock)
            {
                gameBuffer.Clear();
            }
        }
        public Action NextGame()
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
        private void AddGame(Action action)
        {
            lock (gameLock)
            {
                gameBuffer.Enqueue(action);
            }
        }

        public bool ChatIsEmpty()
        {
            lock (chatLock)
            {
                return chatBuffer.Count == 0;
            }
        }
        public void ClearChat()
        {
            lock (chatLock)
            {
                chatBuffer.Clear();
            }
        }
        public Action PopChat()
        {
            lock (chatLock)
            {
                if (chatBuffer.Count == 0)
                {
                    return new NullAction();
                }
                else
                {
                    Action a = chatBuffer.Dequeue();
                    if (a.IsValid())
                        return a;
                    else
                        return new NullAction();
                }
            }
        }
        public Action PeekChat()
        {
            lock (chatLock)
            {
                if (chatBuffer.Count == 0)
                {
                    return new NullAction();
                }
                else
                {
                    Action a = chatBuffer.Peek();
                    if (a.IsValid())
                    {
                        return a;
                    }
                    else
                    {
                        chatBuffer.Dequeue();
                        return new NullAction();
                    }
                }
            }
        }
        private void AddChat(Chat action)
        {
            lock (chatLock)
            {
                chatBuffer.Enqueue(action);
            }
        }


        //3byte를 더 보낸다는 의미로 3을 보내고
        //left면 1, right면 0
        //col
        //row
        public bool Send(GoTo action)
        {
            byte[] message = new byte[4];
            string[] user_col_row = action.data.Split(':');
            if (user_col_row.Length != 3)
                return false;

            message[0] = 3;
            message[1] = Convert.ToByte(user_col_row[0] == "left");
            message[2] = Convert.ToByte(Convert.ToInt32(user_col_row[1])+1);
            message[3] = Convert.ToByte(Convert.ToInt32(user_col_row[2]) + 1);
            return Send(message);
        }
        //2byte를 보낸다는 의미로 2를 보내고
        //left면 1, right면 0
        //cw 면 1, ccw면 0
        //이렇게 하기 위해서 rotate의 data를 단순히 cw, ccw로 하는게 아니라 left:cw, left:ccw, right:cw, right:ccw로 해야 할듯.
        //하지만 left, right정보는 network와 display game에서만 필요하니 RunGame에서는 쓰던대로 cw, ccw만 써도 될듯.
        public bool Send(Rotate action)
        {
            byte[] message = new byte[3];
            string[] user_rot = action.data.Split(':');
            if (user_rot.Length != 2)
                return false;
            message[0] = 2;
            message[1] = Convert.ToByte(user_rot[0] == "left");
            message[2] = Convert.ToByte(user_rot[1] == "cw");
            Logger.WriteLine("SEND:" + message[1] + ":" + message[2]);
            return Send(message);
        }
        //2byte를 보낸다는 의미로 2를 보내고
        //left면 3, right면 2
        //공격할 라인수
        public bool Send(Attack action)
        {
            byte[] message = new byte[3];
            string[] user_line = action.data.Split(':');
            if (user_line.Length != 2)
                return false;
            message[0] = 2;
            message[1] = Convert.ToByte(Convert.ToInt32(user_line[0] == "left") + 2);
            message[2] = Convert.ToByte(user_line[1]);
            return Send(message);
        }
        //nick + ':' + 대화내용 + '\00'을 합치면 최소 4byte.
        //전송할 byte수를 보내고
        //전송할 내용을 보낸다.
        //보내는 byte수를 byte로 보내므로, 채팅 내용을 100자 이상 입력 못하게 하는게 좋을듯.
        public bool Send(Chat action)
        {
            byte[] message = new byte[action.data.Length + 1];
            message[0] = Convert.ToByte(action.data.Length);
            //TODO: 이렇게 하면 char가 2byte여서 문제가 생길듯.
            //어쩌피 하직 채팅 구현 안됐으니 나중에 하자.
            char[] data = action.data.ToCharArray();
            for (int i = 0; i < action.data.Length; i++)
            {
                message[i + 1] = Convert.ToByte(data[i]);
            }
            return Send(message);
        }
        //1byte를 보낸다는 의미로 1을 보내고,
        //left면 1, right면 0
        public bool Send(Turn action)
        {
            byte[] message = new byte[2];
            message[0] = 1;
            message[1] = Convert.ToByte(action.data == "left");
            return Send(message);
        }
        //이게 뭘 위해서 있었는지 까먹었다ㅠ}
        public bool Send(Status action)
        {
            byte[] message = new byte[0];
            return Send(message);
        }
        private bool Send(byte[] message)
        {
            if (stream == null)
                return false;
            lock (thisLock)
            {
                
                stream.Write(message, 0, message.Length);
                stream.Flush();
            }
            return true;
        }

        public void Receive()
        {

            byte[] length;
            byte[] data;
            if (stream == null)
                return;
            lock (thisLock)
            {

                length = new byte[1];
                //TODO:여기서 Read할 수 없으면 기다리고 있는것 맞나? 나중에 확인해봐야지
                stream.Read(length, 0, 1);
                data = new byte[length[0]];
                if(length[0]!=0)
                    stream.Read(data, 0, length[0]);
            }
            switch (length[0])
            {
                case 0:
                    stream.Write(length, 0, 1);
                    break;
                case 1:
                    {
                        string str = (data[0] == 1) ? "left" : "right";
                        this.AddGame(new Turn(str));
                        break;
                    }
                case 2:
                    if (data[0] < 2)
                    {
                        string str = (data[0] == 1) ? "left" : "right";
                        str += ":";
                        str += (data[1] == 1) ? "cw" : "ccw";
                        this.AddGame(new Rotate(str));
                    }
                    else
                    {
                        string str = (data[0]==3) ? "left" : "right";
                        str+=":";
                        str+=data[1];
                        this.AddGame(new Attack(str));
                    }
                    break;
                case 3:
                    {
                        string str = (data[0] == 1) ? "left" : "right";
                        str += ":";
                        str += data[1];
                        str += ":";
                        str += data[2];
                        this.AddGame(new GoTo(str));
                    }
                    break;
                default:
                    this.AddChat(new Chat(data.ToString()));
                    break;
            }
        }
    }
}
