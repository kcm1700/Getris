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
        public Block(int block)
        {
            cells = new Cell[ROW_SIZE, COL_SIZE];
            int mask = block;
            for (int j = ROW_SIZE - 1; j >= 0; j--)
            {
                for (int i = COL_SIZE - 1; i >= 0; i--)
                {
                    cells[i, j] = new BlockCell((CellColor)(mask & 7));
                    mask >>= 3;
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

        public int GetMask()
        {
            int mask = 0;
            for (int i = 0; i < Block.ROW_SIZE; i++)
            {
                for (int j = 0; j < Block.COL_SIZE; j++)
                {
                    mask <<= 3;
                    mask |= Convert.ToInt32(cells[i, j].Color);
                }
            }
            return mask;
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
            //TODO: cells = tmp;라고 하면 안되는 이유가 뭐야? 되지 않나? 어쩌피 레퍼런스 가리키고 있는거 아닌가?
            /*
            for (int i = 0; i < ROW_SIZE; i++)
            {
                for (int j = 0; j < COL_SIZE; j++)
                {
                    cells[i, j] = tmp[i, j];
                }
            }
             * */
            cells = tmp;
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
            /*
            for (int i = 0; i < ROW_SIZE; i++)
            {
                for (int j = 0; j < COL_SIZE; j++)
                {
                    cells[i, j] = tmp[i, j];
                }
            }
             * */
            cells = tmp;
        }
        public Cell this[int row, int col]
        {
            get
            {
                return cells[row, col];
            }
        }
        public CellColor GetCellColor(int row, int col)
        {
            return cells[row, col].Color;
        }
        public bool IsEmpty(int row, int col)
        {
            return cells[row, col].IsEmpty;
        }
    }
}