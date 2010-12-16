namespace Getris
{
    interface Game
    {
        public static const int col = 21;
        public static const int row = 10;
        private void Start();
    }
    class RunGame : Game
    {
        private System.Threading.Thread thread;
        public void RunGame()
        {
            this.thread = new System.Threading.Thread(Start);
        }
        private void Start()
        {
            // TODO: keyboard를 받아서 게임을 하고, 네트워크로 보낸다.
        }
    }
    class DisplayGame : Game
    {
        private System.Threading.Thread thread;
        public void DisplayGame()
        {
            this.thread = new System.Threading.Thread(Start);
        }
        private void Start()
        {
            //TODO: network으로 입력을 받아서 결과를 출력하는 일을 한다.
        }
    }
    class Pile
    {
        private Block[,] blocks = new Block[Game.row, Game.col];
        public Pile()
        {
            for (int i = 0; i < Game.row; i++)
                for (int j = 0; j < Game.col; j++)
                    blocks[i, j] = new Block(Color.EMPTY);
        }
    }
}