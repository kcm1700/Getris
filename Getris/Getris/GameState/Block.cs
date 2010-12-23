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
        //TODO : 아무 인자를 안넣으면 random한 색을 생성하는 생성자로 만들고 싶은데 방법이 없나? 그냥 Color를 enum으로 정의해서 쓰면 안될까?
        // 답: 게임에서 몇 색으로 게임을 할 것인지 설정가능하게 할 것이므로, 무작정 Block에 랜덤 색상을 요구하는 것은 무리가 있을 것 같습니다.
        // 랜덤 색상은 역시 그 윗단에서 처리를 해줘야하지 않을까요?
        public Block(CellColor color)
        {
/*            row = Pile.ROW;
            col = (Pile.COL + 1) / 2;*/
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
        // TODO : Random을 인자로 넘기는 이유는? 매번 새로운 Random을 만들면 random성이 보장이 안 됩니다.
        public void MakeRandomBlock(int blockCellCnt, Random rnd)
        {
            if (ROW_SIZE * COL_SIZE < blockCellCnt || blockCellCnt < 1)
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
                        cells[i, j] = new BlockCell(cells[1,1].maskColor);
                        continue;
                    }
                    if (rnd.Next(cnt) < remain)
                    {
                        cells[i, j] = new BlockCell(cells[1,1].maskColor);
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