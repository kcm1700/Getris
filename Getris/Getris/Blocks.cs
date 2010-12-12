namespace Getris
{
    /*
    interface Block
    {
    }
    class RedBlock:Block
    {
    }
    class BlueBlock:Block
    {
    }
    class GreenBlock : Block
    {
    }
    class YellowBlock : Block
    {
    }
    class EmptyBlock : Block
    {
    }
     */
    enum Color {EMPTY, RED, BLUE, YELLOW, GREEN };
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
    }
    class Blocks
    {
        private Block[,] Blocks = new Block[5,5];
        private Color color;
        public Blocks()
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
                    Blocks[i / 5, i % 5] = new Block(this.color);
                    remain--;
                }
                else
                {
                    Blocks[i / 5, i % 5] = new Block(Color.EMPTY);
                }
            }
        }
    }
}