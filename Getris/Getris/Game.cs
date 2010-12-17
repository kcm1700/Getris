namespace Getris
{
    abstract class Game
    {
        public static const int COLUMN = 10;
        public static const int ROW = 21;
        protected Brick brick;
        protected Pile pile;
        private void Start();
        protected System.Threading.Thread thread;
    }
    class RunGame : Game
    {
        public void RunGame()
        {
            this.thread = new System.Threading.Thread(Start);
        }
        private void Start()
        {
            // TODO: keyboard를 받아서 게임을 하고, 네트워크로 보낸다.
        }
        // TODO: move전에 validation 검사를 하도록 하자.
        private void MoveDown()
        {
            this.brick.MoveDown();
        }
        private void MoveLeft()
        {
            this.brick.MoveLeft();
        }
        private void MoveRight()
        {
            this.brick.MoveRight();
        }
    }
    class DisplayGame : Game
    {
        public void DisplayGame()
        {
            this.thread = new System.Threading.Thread(Start);
        }
        private void Start()
        {
            //TODO: network으로 입력을 받아서 결과를 출력하는 일을 한다.
        }
        // TODO: move전에 validation 검사를 하도록 하자.
        private void GotoMatrix(int x, int y)
        {
            this.brick.GotoMatrix(x, y);
        }
    }
}