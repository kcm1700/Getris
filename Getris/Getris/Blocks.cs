namespace Getris
{
    enum Color {RED, BLUE, YELLOW, GREEN, EMPTY};
    class Block
    {
        private Color color;
        public Block(Color color = Color.EMPTY)
        {
            this.color = color;
        }
        public Color Color
        {
            get
            {
                return this.color;
            }
        }
        public bool IsRed()
        {
            return this.color == Color.RED;
        }
        public bool IsBlue()
        {
            return this.color == Color.BLUE;
        }
        public bool IsYellow()
        {
            return this.color == Color.YELLOW;
        }
        public bool IsGreen()
        {
            return this.color == Color.GREEN;
        }
        public bool IsEmpty()
        {
            return this.color == Color.EMPTY;
        }
    }

    class Brick
    {
        private Block[,] blocks = new Block[5,5];
        private int x;
        private int y;
        private Color color;
        public Brick()
        {
            System.Random rand = new System.Random();
            this.color = (Color)rand.Next(4);
            int cnt = 24;
            int remain = 4;
            for (int i = 0; i < 25; i++)
            {
                if (i == 2*5 + 2)
                    continue;
                if (rand.Next(cnt--) < remain)
                {
                    this.blocks[i / 5, i % 5] = new Block(this.color);
                    remain--;
                }
                else
                {
                    this.blocks[i / 5, i % 5] = new Block(Color.EMPTY);
                }
            }
            this.x = Game.row/2;
            this.y = 0;
        }
        private bool IsValid(ref Pile pile)
        {
            // TODO: implement
            return false;
        }
        public void MoveLeft()
        {
            this.x--;
        }
        public void MoveRight()
        {
            this.x++;
        }
        public void MoveDown()
        {
            this.y++;
        }
        public void GotoXY(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public void Rotate()
        {
            Block[,] changed = new Block[5, 5];
            for (int i = 0; i < 5; i++)
                for (int j = 0; j < 5; j++)
                    changed[4 - j, i] = this.blocks[i, j];
            this.blocks = changed;
        }
        public void RotateReverse()
        {
            Block[,] changed = new Block[5, 5];
            for (int i = 0; i < 5; i++)
                for (int j = 0; j < 5; j++)
                    changed[j, 4 - i] = this.blocks[i, j];
            this.blocks = changed;
        }
        public void Drop()
        {
        }
        public int X
        {
            get { return this.x; }
        }
        public int Y
        {
            get { return this.y; }
        }
    }
}