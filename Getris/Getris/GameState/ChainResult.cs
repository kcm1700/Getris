using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace getris.GameState
{
    class ChainResult
    {
        static private decimal[] chainScore = { 1, 20, 28, 40, 57, 80, 100, 120, 140, 150, 160, 170, 170, 180, 180, 190, 190, 200, 200, 200, 200, 200, 200, 200, 200 };
        private int chainCnt;
        public List<Animation.EraseDropPair> animation;
        public Cell[,] copiedBoard;
        public Decimal score;

        private Object thisLock = new Object();

        public ChainResult()
        {
            chainCnt = 0;
            score = 0;
            animation = new List<Animation.EraseDropPair>();
        }
        /// <summary>
        /// copies board and stores
        /// </summary>
        /// <param name="board">연쇄 전 pile의 board</param>
        public void SetOriginalBoard(Cell[,] board)
        {
            lock (thisLock)
            {
                copiedBoard = (Cell[,])board.Clone();
            }
        }
        public void Add(Animation.EraseDropPair ed)
        {
            lock (thisLock)
            {
                animation.Add(ed);
            }
        }
        public int Count
        {
            get
            {
                lock (thisLock)
                {
                    return chainCnt;
                }
            }
        }
        public Animation.EraseDropPair this[int index]
        {
            get
            {
                lock (thisLock)
                {
                    return animation.ElementAt(index);
                }
            }
        }
        public Decimal Score
        {
            get
            {
                lock (thisLock)
                {
                    return score;
                }
            }
        }
        public void GetScore(int removedLine)
        {
            lock (thisLock)
            {
                if (chainCnt >= chainScore.Length)
                {
                    score += 210 * removedLine;
                }
                else
                {
                    score += chainScore[chainCnt] * removedLine;
                }
                chainCnt++;
            }
        }



    }
}
