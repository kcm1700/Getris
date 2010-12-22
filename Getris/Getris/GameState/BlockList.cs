using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace getris.GameState
{
    public class BlockList
    {
        private uint leftPivot;
        private uint rightPivot;
        private readonly Block[] blocks;
        public BlockList()
        {
            leftPivot = 0;
            rightPivot = 0;
            Random r = new Random();
            blocks = new Block[900 + r.Next(100)];
            for (int i = 0; i < blocks.Length; i++)
            {
                //TODO : Block 생성자 수정한 다음에 수정.
                blocks[i] = new Block(System.Drawing.Color.Red);
            }
        }
        public void NextBlockLeft()
        {
            leftPivot = leftPivot + 1;
        }
        public void NextBlockRight()
        {
            rightPivot = rightPivot + 1;
        }
        public  Block LeftBlock
        {
            get
            {
                return blocks[leftPivot%blocks.Length];
            }
        }
        public Block RightBlock
        {
            get
            {
                return blocks[rightPivot%blocks.Length];
            }
        }
        public Block Left1st
        {
            get
            {
                if (rightPivot >= leftPivot + 1)
                    return blocks[(leftPivot + 1)%blocks.Length];
                else
                    return new Block(blocks[leftPivot + 1].BlockColor);
            }
        }
        public Block Right1st
        {
            get
            {
                if (leftPivot >= rightPivot + 1)
                    return blocks[(rightPivot + 1) % blocks.Length];
                else
                    return new Block(blocks[rightPivot + 1].BlockColor);
            }
        }
        public Block Left2nd
        {
            get
            {
                if (rightPivot >= leftPivot + 2)
                    return blocks[(leftPivot + 2) % blocks.Length];
                else
                    return new Block(blocks[leftPivot + 2].BlockColor);
            }
        }
        public Block Right2nd
        {
            get
            {
                if (leftPivot >= rightPivot + 2)
                    return blocks[(rightPivot + 2) % blocks.Length];
                else
                    return new Block(blocks[rightPivot + 2].BlockColor);
            }
        }
    }
}
