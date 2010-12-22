using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace getris.GameState
{
    class Pile : IPile
    {
        //bottom index is 0
        private Cell[,] board;

        public static const int ROW = 21;
        public static const int COL = 10;

        // constructor: 1. initialize board with blank cells 
        public Pile()
        {
            board = new Cell[ROW, COL];

            for (int i = 0; i < ROW; i++)
            {
                for (int j = 0; j < COL; j++)
                {
                    board[i, j] = new BlankCell();
                }
            }
        }

        public Cell GetCell(int row, int col)
        {
            return board[row, col];
        }

        public Color GetCellColor(int row, int col)
        {
            return board[row, col].maskColor;
        }

        public bool IsCellEmpty(int row, int col)
        {
            return board[row, col].IsEmpty();
        }

        private bool isCellCollision(int row, int col, Cell cell)
        {
            if (cell.IsEmpty()) return false; // always safe
            if (row < 0 || row >= ROW) return true;
            if (col < 0 || col >= COL) return true;
            if (!IsCellEmpty(row,col)) return true; // if board cell is not empty, it collides
            return false;
        }
        private void PutBlock(int row, int col, Block block)
        {
            for (int i = 0; i < Block.ROW_SIZE; i++)
            {
                for (int j = 0; j < Block.COL_SIZE; j++)
                {
                    if (!block.GetCell(i, j).IsEmpty()) // if not empty
                    {
                        board[row + i, col + j] = block.GetCell(i, j);
                    }
                }
            }
        }

        public bool IsBlockCollision(int row, int col, Block block)
        {
            for (int i = 0; i < Block.ROW_SIZE; i++)
            {
                for (int j = 0; j < Block.COL_SIZE; j++)
                {
                    if (isCellCollision(row + i, col + j, block.GetCell(i, j))) return true;
                }
            }
            return false;
        }

        /// <summary> Drop <param name="block">block</param> at somewhere. It doesn't simulate any removal of lines.
        /// <param name="row">row</param>(bottom of the field: 0) &amp; <param name="col">col</param>(left of the field: 0)
        /// </summary>
        public void DropBlock(int row, int col, Block block)
        {
            if (IsBlockCollision(row, col, block))
            {
                throw new Exception("블럭이 올바른 위치에서 놓이지 않았음. 놓일 수 없는 위치임. DropBlock");
            }
            int lastRow = row;
            for (int i = row-1; i+Block.ROW_SIZE >= 0; i--)
            {
                if (IsBlockCollision(i, col, block)) break;
                lastRow = i;
            }
            PutBlock(lastRow, col, block);
        }
        public ChainResult SimulateChain()
        {
            ChainResult chainResult = new ChainResult();
            //initialize 
            chainResult.animation = new List<Animation.EraseDropPair>();
            chainResult.score = 0;

            bool flgRemoved = false;
            do
            {
                List<int> removedLines = new List<int>();
                for (int i = 0; i < ROW; i++)
                {
                    int cnt = 0;

                    for (int j = 0; j < COL; j++)
                        if (IsCellEmpty(i, j))
                            cnt++;

                    if (cnt == 0)
                    {
                        for (int j = 0; j < COL; j++)
                            board[i, j] = new BlankCell();
                        removedLines.Add(i);
                        flgRemoved = true;
                    }
                }

                //TODO : move all blocks down & add it to drop cells list

                //TODO : combine removedLines & drop cells list to make EraseDropPair
                //TODO : chainResult.animation 에 EraseDropPair 추가하기.

                //TODO : calculate score
            } while (flgRemoved);

            return chainResult;
        }
    }
}
