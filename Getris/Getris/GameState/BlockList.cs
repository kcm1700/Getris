using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace getris.GameState
{
    sealed class BlockList
    {
        static private BlockList instance = null;
        static private readonly System.Object thisLock, leftLock, rightLock;
        static BlockList()
        {
            thisLock = new System.Object();
            leftLock = new System.Object();
            rightLock = new System.Object();
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
            blocks = new Block[900 + Core.Random.rand(100)];
            for (int i = 0; i < blocks.Length; i++)
            {
                blocks[i] = new Block(4, colorCnt);
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
        /// <summary>
        /// returns new instance of Block
        /// </summary>
        /// <param name="isLeft">좌측인지의 여부</param>
        /// <returns></returns>
        public Block GetBlock(bool isLeft)
        {
            if(isLeft)
            {
                lock (leftLock)
                {
                    return new Block(blocks[leftPivot % blocks.Length]);
                }
            }
            else
            {
                lock (rightLock)
                {
                    return new Block(blocks[rightPivot % blocks.Length]);
                }
            }
        }
        /// <summary>
        /// returns new instance of Block
        /// </summary>
        /// <param name="isLeft">좌측인지의 여부</param>
        /// <returns></returns>
        public Block Get1st(bool isLeft)
        {
            if(isLeft)
            {
                lock (leftLock)
                {
                    if (rightPivot >= leftPivot  || true)
                        return new Block(blocks[(leftPivot + 1) % blocks.Length]);
                    else
                        //TODO: 
                        return new Block();
                }
            }
            else
            {
                lock (rightLock)
                {
                    if (leftPivot >= rightPivot  || true)
                        return new Block(blocks[(rightPivot + 1) % blocks.Length]);
                    else
                        return new Block();
                }
            }
        }
        /// <summary>
        /// returns new instance of Block
        /// </summary>
        /// <param name="isLeft">좌측인지의 여부</param>
        /// <returns></returns>
        public Block Get2nd(bool isLeft)
        {
            if(isLeft)
            {
                lock (leftLock)
                {
                    if (rightPivot >= leftPivot + 2 || true)
                        return new Block(blocks[(leftPivot + 2) % blocks.Length]);
                    else
                        //Block()을 기본적으로 완전 BlankCell로 채워진 놈으로 출력하게 수정했고, 결과적으로 여기서도 BlankCell로 채워진 block을 return할 것임
                        return new Block();
                }
            }
            else
            {
                lock (rightLock)
                {
                    if (leftPivot >= rightPivot + 2 || true)
                        return new Block(blocks[(rightPivot + 2) % blocks.Length]);
                    else
                        //Block()을 기본적으로 완전 BlankCell로 채워진 놈으로 출력하게 수정했고, 결과적으로 여기서도 BlankCell로 채워진 block을 return할 것임
                        return new Block();
                }
            }
        }
    }
}
