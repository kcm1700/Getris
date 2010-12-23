using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace getris.GameState
{
    sealed class BlockList
    {
        static private BlockList instance = null;
        static private readonly System.Object thisLock = new System.Object();
        static private readonly System.Object leftLock = new System.Object();
        static private readonly System.Object rightLock = new System.Object();
        static BlockList()
        {
        }
        static public BlockList Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (thisLock)
                    {
                        if (instance == null)
                            instance = new BlockList();
                    }
                }
                return instance;
            }
        }
        public BlockList(int colorCnt=5)
        {
            leftPivot=0;
            rightPivot=0;
            Random rnd = new Random();
            blocks = new Block[900 + rnd.Next(100)];
            for (int i = 0; i < blocks.Length; i++)
            {
                //TODO : Block 생성자 수정한 다음에 수정.
                blocks[i] = new Block((CellColor)(rnd.Next(1,colorCnt+1)));
                blocks[i].MakeRandomBlock(4, rnd);
            }
        }

        private uint leftPivot;
        private uint rightPivot;
        private readonly Block[] blocks;
        public void NextBlock(bool isLeft)
        {
            if (isLeft)
            {
                lock (leftLock)
                {
                    leftPivot++;
                }
            }
            else
            {
                lock (rightLock)
                {
                    rightPivot++;
                }
            }
        }
        public Block GetBlock(bool isLeft)
        {
            if(isLeft)
            {
                lock (leftLock)
                {
                    return blocks[leftPivot % blocks.Length];
                }
            }
            else
            {
                lock (rightLock)
                {
                    return blocks[rightPivot % blocks.Length];
                }
            }
        }
        public Block Get1st(bool isLeft)
        {
            if(isLeft)
            {
                lock (leftLock)
                {
                    if (rightPivot >= leftPivot + 1)
                        return blocks[(leftPivot + 1) % blocks.Length];
                    else
                        return new Block(blocks[leftPivot + 1].BlockColor);
                }
            }
            else
            {
                lock (rightLock)
                {
                    if (leftPivot >= rightPivot + 1)
                        return blocks[(rightPivot + 1) % blocks.Length];
                    else
                        return new Block(blocks[rightPivot + 1].BlockColor);
                }
            }
        }
        public Block Get2nd(bool isLeft)
        {
            if(isLeft)
            {
                lock (leftLock)
                {
                    if (rightPivot >= leftPivot + 2)
                        return blocks[(leftPivot + 2) % blocks.Length];
                    else
                        return new Block(blocks[leftPivot + 2].BlockColor);
                }
            }
            else
            {
                lock (rightLock)
                {
                    if (leftPivot >= rightPivot + 2)
                        return blocks[(rightPivot + 2) % blocks.Length];
                    else
                        return new Block(blocks[rightPivot + 2].BlockColor);
                }
            }
        }
    }
}
