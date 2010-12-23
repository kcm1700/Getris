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
        private List<Animation.EraseDropPair> animation;
        private Cell[,] copiedBoard;
        public Decimal score;
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
            copiedBoard = (Cell[,])board.Clone();
        }
        public void Add(Animation.EraseDropPair ed)
        {
            animation.Add(ed);
        }
        public int Count
        {
            get
            {
                return chainCnt;
            }
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
            if(chainCnt >= chainScore.Length){
                score += 210 * removedLine;
            }else{
                score += chainScore[chainCnt] * removedLine;
            }
            chainCnt++;
        }



        const double g = 3;

        Dictionary<Tuple<int,int>, double> rowDict;

        public double getAniRow(int row, int col)
        {
            Tuple<int,int> from = new Tuple<int, int>(row, col);
            if (rowDict.ContainsKey(from))
            {
                return rowDict[from];
            }
            else
            {
                return row;
            }
        }

        public double getAniCol(int row, int col)
        {
            return col;
        }

        /// <summary>
        /// 애니메이션 세팅.
        /// </summary>
        /// <param name="timeElapsed">애니메이션 시작 후 경과 시간(단위: 초)</param>
        /// <returns>returns true if animation ended</returns>
        public bool SetElapsedTime(double timeElapsed){
            throw new NotImplementedException();
            const double g = 3;
            rowDict = new Dictionary<Tuple<int, int>, double>();
            Cell[,] tmpBoard = (Cell[,])copiedBoard.Clone();
            foreach (Animation.EraseDropPair edp in animation)
            {
                double largestGap = 0;
                foreach (Animation.Drop drop in edp.dropCellList){
                    if(drop.rowBefore-drop.rowAfter > largestGap){
                        largestGap = drop.rowBefore-drop.rowAfter;
                    }
                }
                double timeSpan = Math.Sqrt(2*largestGap/g);
                if (timeSpan >= timeElapsed)
                {

                    return false;
                }
                foreach (Animation.Drop drop in edp.dropCellList)
                {
                    tmpBoard[drop.rowAfter,drop.col] = copiedBoard[drop.rowBefore,drop.col];
                }
                //TODO: calculate bomb time & remove lines
                timeElapsed -= timeSpan;
            }
            return true;
        }
    }
}
