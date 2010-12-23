using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace getris.GameState
{
    class ChainResult
    {
        private decimal[] chainScore = { 1, 20, 28, 40, 57, 80, 100, 120, 140, 150, 160, 170, 170, 180, 180, 190, 190, 200, 200, 200, 200, 200, 200, 200, 200 };
        private int chainCnt;
        private List<Animation.EraseDropPair> animation;
        public Decimal score;
        public ChainResult()
        {
            chainCnt = 0;
            score = 0;
            animation = new List<Animation.EraseDropPair>();
        }
        public void Add(Animation.EraseDropPair ed)
        {
            animation.Add(ed);
        }
        public Animation.EraseDropPair this[int index]
        {
            get
            {
                return animation.ElementAt(index);
            }
        }
        public Decimal Score
        {
            get
            {
                return score;
            }
        }
        public void GetScore(int removedLine)
        {
            score += chainScore[chainCnt++] * removedLine;
        }
                
    }
}
