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
        public const int ROW = 3;
        public const int COL = 3;
        private Cell[,] cells;

        private Color blockColor;
        public Color BlockColor
        {
            get
            {
                return blockColor;
            }
            set
            {
                blockColor = BlockColor;
                //repaint all the block cells
                for (int i = 0; i < ROW; i++)
                {
                    for (int j = 0; j < COL; j++)
                    {
                        if (!cells[i, j].IsEmpty())
                        {
                            cells[i, j].maskColor = blockColor;
                        }
                    }
                }
            }
        }

        private Block()
        {
        }
        public Block(Color color)
        {
            blockColor = color;
            cells = new Cell[ROW, COL];
            for (int i = 0; i < ROW; i++)
            {
                for (int j = 0; j < COL; j++)
                {
                    if (i == 1 && j == 1)
                    {
                        cells[i, j] = new BlockCell(blockColor);
                    }
                    else
                    {
                        cells[i, j] = new BlankCell();
                    }
                }
            }
        }

        public void MakeRandomBlock(int blockCellCnt, Random rnd)
        {
            for (int i = 0; i < ROW; i++)
            {
                for (int j = 0; j < COL; j++)
                {
                    cells[i, j] = new BlankCell();
                }
            }

            if (blockCellCnt > ROW * COL)
            {
                throw new Exception("Can't MakeRandomBlock");
            }

            if (blockCellCnt >= 1)
            {
                cells[1, 1] = new BlockCell(blockColor);
                blockCellCnt--;
            }

            while (blockCellCnt >= 1)
            {
                int row = rnd.Next(ROW);
                int col = rnd.Next(COL);

                if (cells[row, col].IsEmpty())
                {
                    cells[row, col] = new BlockCell(blockColor);
                    blockCellCnt--;
                }
            }
        }

        public void RotateCw()
        {
            Cell[,] tmp = new Cell[ROW, COL];
            for (int i = 0; i < ROW; i++)
            {
                for (int j = 0; j < COL; j++)
                {
                    tmp[j, COL - i - 1] = cells[i, j];
                }
            }
            for (int i = 0; i < ROW; i++)
            {
                for (int j = 0; j < COL; j++)
                {
                    cells[i, j] = tmp[i, j];
                }
            }
        }
        public void RotateCcw()
        {
            Cell[,] tmp = new Cell[ROW, COL];
            for (int i = 0; i < ROW; i++)
            {
                for (int j = 0; j < COL; j++)
                {
                    tmp[i, j] = cells[j, COL - i - 1];
                }
            }
            for (int i = 0; i < ROW; i++)
            {
                for (int j = 0; j < COL; j++)
                {
                    cells[i, j] = tmp[i, j];
                }
            }
        }

        public Cell GetCell(int row, int col)
        {
            return cells[row, col];
        }
        public void SetCell(int row, int col, Cell cell)
        {
            cells[row, col] = cell;
        }
    }
}
