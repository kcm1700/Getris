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

        /// <summary>
        /// Copy Constructor
        /// </summary>
        /// <param name="from"></param>
        public Block(Block from)
        {
            this.cells = new Cell[ROW_SIZE, COL_SIZE];
            for (int i = 0; i < ROW_SIZE; i++)
            {
                for (int j = 0; j < COL_SIZE; j++)
                {
                    this.cells[i, j] = from.cells[i, j];
                }
            }
        }
        /// <summary>
        /// Blank Block Constructor
        /// </summary>
        /// <param name="isRandom"></param>
        public Block()
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

        public Block(int blockCellCnt, int colorCnt)
        {
            if (blockCellCnt < 1 || ROW_SIZE * COL_SIZE < blockCellCnt)
            {
                throw new Exception("Can't MakeRandomBlock");
            }

            cells = new Cell[ROW_SIZE, COL_SIZE];

            int cnt = COL_SIZE * ROW_SIZE - 1;
            int remain = blockCellCnt - 1;

            int numberOfColors = 2;
            CellColor[] blockColorCand = new CellColor[numberOfColors];

            for (int i = 0; i < numberOfColors; i++)
            {
                bool dupFlg;
                do
                {
                    dupFlg = false;
                    blockColorCand[i] = (CellColor)Core.Random.rand(1, colorCnt + 1);
                    for (int j = 0; j < i; j++)
                    {
                        if (blockColorCand[i] == blockColorCand[j])
                        {
                            dupFlg = true;
                        }
                    }
                } while (dupFlg);
            }
            for (int i = 0; i < ROW_SIZE; i++)
            {
                for (int j = 0; j < COL_SIZE; j++)
                {
                    if (i == ROW_SIZE / 2 && j == COL_SIZE / 2)
                    {
                        cells[i, j] = new BlockCell(blockColorCand[Core.Random.rand(0, numberOfColors)]);
                        continue;
                    }
                    if (Core.Random.rand(cnt) < remain)
                    {
                        cells[i, j] = new BlockCell(blockColorCand[Core.Random.rand(0, numberOfColors)]);
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

        public void RotateCcw()
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
        public void RotateCw()
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
        }
    }
}