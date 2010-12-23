using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace getris.GameState
{
    /// <summary>
    /// Block의 밑부분은 0번이고, 윗부분은 (ROW-1)번이다.
    /// </summary>
    class Block
    {
        public const int ROW_SIZE = 3;
        public const int COL_SIZE = 3;
        private Cell[,] cells;
        /*
        //TODO:어쩌피 색을 완전 random으로 할거라면 이게 필요없을듯
        public CellColor BlockColor
        {
            get
            {
                return cells[1,1].maskColor;
            }
            set
            {
                //repaint all the block cells
                for (int i = 0; i < ROW_SIZE; i++)
                {
                    for (int j = 0; j < COL_SIZE; j++)
                    {
                        if (!cells[i, j].IsEmpty())
                        {
                            cells[i, j].maskColor = value;
                        }
                    }
                }
            }
        }
        private Block()
        {
        }
        public Block(CellColor color= CellColor.transparent)
        {
            cells = new Cell[ROW_SIZE, COL_SIZE];
            for (int i = 0; i < ROW_SIZE; i++)
            {
                for (int j = 0; j < COL_SIZE; j++)
                {
                    if (i == 1 && j == 1)
                    {
                        cells[i, j] = new BlockCell(color);
                    }
                    else
                    {
                        cells[i, j] = new BlankCell();
                    }
                }
            }
        }
         * */
        public Block(bool isRandom = false)
        {
            cells = new Cell[ROW_SIZE, COL_SIZE];
            for (int i = 0; i < ROW_SIZE; i++)
            {
                for (int j = 0; j < COL_SIZE; j++)
                {
                    cells[i, j] = new BlankCell();
                }
            }
        }
        public void MakeRandomBlock(int blockCellCnt, int colorCnt)
        {
            if (blockCellCnt < 1 || ROW_SIZE * COL_SIZE < blockCellCnt)
            {
                throw new Exception("Can't MakeRandomBlock");
            }
            int cnt = COL_SIZE * ROW_SIZE - 1;
            int remain = blockCellCnt - 1;
            for (int i = 0; i < ROW_SIZE; i++)
            {
                for (int j = 0; j < COL_SIZE; j++)
                {
                    if (i == ROW_SIZE / 2 && j == COL_SIZE / 2)
                    {
                        cells[i, j] = new BlockCell((CellColor)Core.Random.rand(1, colorCnt + 1));
                        continue;
                    }
                    if (Core.Random.rand(cnt) < remain)
                    {
                        cells[i, j] = new BlockCell((CellColor)Core.Random.rand(1, colorCnt + 1));
                        remain--;
                    }
                    else
                    {
                        cells[i, j] = new BlankCell();
                    }
                    cnt--;
                }
            }
        }

        public void RotateCw()
        {
            Cell[,] tmp = new Cell[ROW_SIZE, COL_SIZE];
            for (int i = 0; i < ROW_SIZE; i++)
            {
                for (int j = 0; j < COL_SIZE; j++)
                {
                    tmp[j, COL_SIZE - i - 1] = cells[i, j];
                }
            }
            for (int i = 0; i < ROW_SIZE; i++)
            {
                for (int j = 0; j < COL_SIZE; j++)
                {
                    cells[i, j] = tmp[i, j];
                }
            }
        }
        public void RotateCcw()
        {
            Cell[,] tmp = new Cell[ROW_SIZE, COL_SIZE];
            for (int i = 0; i < ROW_SIZE; i++)
            {
                for (int j = 0; j < COL_SIZE; j++)
                {
                    tmp[i, j] = cells[j, COL_SIZE - i - 1];
                }
            }
            for (int i = 0; i < ROW_SIZE; i++)
            {
                for (int j = 0; j < COL_SIZE; j++)
                {
                    cells[i, j] = tmp[i, j];
                }
            }
        }
        public Cell this[int row, int col]
        {
            get
            {
                return cells[row, col];
            }
            set
            {
                cells[row, col]=value;
            }
        }
    }
}