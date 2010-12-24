using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using getris.GameState;

namespace getris.Animation
{
    public struct Drop{
        public int rowBefore, col;
        public int rowAfter;
    }
    public class EraseDropPair
    {
        public List<Drop> dropCellList; // includes all drop cells.
        public List<int> erasedLineList;

        public EraseDropPair()
        {
            erasedLineList = new List<int>();
            dropCellList = new List<Drop>();
        }
        public EraseDropPair(List<int> erasedLineList, List<Drop> dropCellList)
        {
            this.erasedLineList = erasedLineList;
            this.dropCellList = dropCellList;
        }
    }

    public class Animator
    {
        private List<EraseDropPair> animation;
        private Cell[,] copiedBoard;
        private Object thisLock = new Object();
        const double g = 3;

        Dictionary<Tuple<int, int>, double> rowDict;
        Dictionary<Tuple<int, int>, CellColor> colorDict;

        public Animator(Cell[,] copiedBoard,List<EraseDropPair> animation)
        {
            this.copiedBoard = copiedBoard;
            this.animation = animation;
        }

        public double getAniRow(int row, int col)
        {
            lock (thisLock)
            {
                Tuple<int, int> from = new Tuple<int, int>(row, col);
                if (rowDict.ContainsKey(from))
                {
                    return rowDict[from];
                }
                else
                {
                    return row;
                }
            }
        }

        public double getAniCol(int row, int col)
        {
            return col;
        }

        public CellColor getAniCellColor(int row, int col)
        {
            lock (thisLock)
            {
                Tuple<int, int> from = new Tuple<int, int>(row, col);
                if (colorDict.ContainsKey(from))
                {
                    return colorDict[from];
                }
                else
                {
                    return CellColor.transparent;
                }
            }
        }

        double v0 = 3;
        private double GetTimeDown(double dist, double g = 55)
        {
            return (-v0 + Math.Sqrt(2 * dist *g + v0*v0)) / g;
        }
        private double GetDistDown(double time, double g = 55)
        {
            return time * time * g / 2 + v0*time;
        }

        /// <summary>
        /// 애니메이션 세팅.
        /// </summary>
        /// <param name="timeElapsed">애니메이션 시작 후 경과 시간(단위: 초)</param>
        /// <returns>returns true if animation ended</returns>
        public bool SetElapsedTime(double timeElapsed)
        {
            lock (thisLock)
            {
                rowDict = new Dictionary<Tuple<int, int>, double>();
                colorDict = new Dictionary<Tuple<int,int>,CellColor>();
                Cell[,] aniBoard = (Cell[,])copiedBoard.Clone();
                Cell[,] tmpBoard = (Cell[,])copiedBoard.Clone();
                foreach (Animation.EraseDropPair edp in animation)
                {
                    double largestGap = 0;
                    foreach (Animation.Drop drop in edp.dropCellList)
                    {
                        if (drop.rowBefore - drop.rowAfter > largestGap)
                        {
                            largestGap = drop.rowBefore - drop.rowAfter;
                        }
                    }
                    double timeSpan = GetTimeDown(largestGap);
                    if (timeSpan >= timeElapsed)
                    {
                        //drop 이전의 상태로 세팅
                        for (int i = 0; i < aniBoard.GetLength(0); i++)
                        {
                            for (int j = 0; j < aniBoard.GetLength(1); j++)
                            {
                                Tuple<int, int> pos = new Tuple<int, int>(i, j);
                                rowDict[pos] = i;
                                colorDict[pos] = aniBoard[i, j].Color;
                            }
                        }
                        //drop하는 도중의 위치를 시뮬레이트
                        foreach (Animation.Drop drop in edp.dropCellList)
                        {
                            Tuple<int, int> pos = new Tuple<int, int>(drop.rowBefore, drop.col);
                            double dist = drop.rowBefore - drop.rowAfter;
                            if (GetTimeDown(dist) >= timeElapsed)
                            {
                                // 아직 떨어지는 중
                                rowDict[pos] = drop.rowBefore - GetDistDown(timeElapsed);
                            }
                            else
                            {
                                // 완전히 떨어짐
                                rowDict[pos] = drop.rowAfter;
                            }
                        }
                        return false;
                    }
                    timeElapsed -= timeSpan;


                    //initialize temporary board
                    for (int i = 0; i < aniBoard.GetLength(0); i++)
                    {
                        for (int j = 0; j < aniBoard.GetLength(1); j++)
                        {
                            tmpBoard[i, j] = new BlankCell();
                        }
                    }
                    //moved cell to temp board
                    foreach (Animation.Drop drop in edp.dropCellList)
                    {
                        tmpBoard[drop.rowAfter, drop.col] = aniBoard[drop.rowBefore, drop.col];
                        aniBoard[drop.rowBefore, drop.col] = new BlankCell();
                    }
                    // store moved cell into original board
                    for (int i = 0; i < aniBoard.GetLength(0); i++)
                    {
                        for (int j = 0; j < aniBoard.GetLength(1); j++)
                        {
                            if (!(tmpBoard[i, j] is BlankCell))
                            {
                                //not blank cell
                                aniBoard[i, j] = tmpBoard[i, j];
                            }
                        }
                    }
                    //TODO: calculate bomb time & remove lines
                    if (edp.erasedLineList.Count > 0)
                    {
                        timeSpan = 0.5;
                    }
                    else
                    {
                        timeSpan = 0;
                    }
                    if (timeSpan >= timeElapsed)
                    {
                        for (int i = 0; i < aniBoard.GetLength(0); i++)
                        {
                            for (int j = 0; j < aniBoard.GetLength(1); j++)
                            {
                                Tuple<int, int> pos = new Tuple<int, int>(i, j);
                                rowDict[pos] = i;
                                colorDict[pos] = aniBoard[i, j].Color;
                            }
                        }
                        foreach (int i in edp.erasedLineList)
                        {
                            for (int j = 0; j < aniBoard.GetLength(1); j++)
                            {
                                Tuple<int, int> pos = new Tuple<int, int>(i, j);
                                if (((int)(timeElapsed / 0.06)) % 2 == 1)
                                {
                                    colorDict[pos] = aniBoard[i, j].Color;
                                }
                                else
                                {
                                    colorDict[pos] = CellColor.transparent;
                                }
                            }
                        }
                        return false;
                    }
                    timeElapsed -= timeSpan;
                    foreach (int i in edp.erasedLineList)
                    {
                        for (int j = 0; j < aniBoard.GetLength(1); j++)
                        {
                            aniBoard[i,j] = new BlankCell();
                        }
                    }
                }
                for (int i = 0; i < aniBoard.GetLength(0); i++)
                {
                    for (int j = 0; j < aniBoard.GetLength(1); j++)
                    {
                        Tuple<int, int> pos = new Tuple<int, int>(i, j);
                        rowDict[pos] = i;
                        colorDict[pos] = aniBoard[i, j].Color;
                    }
                }
                return true;
            }
        }
    }
}
