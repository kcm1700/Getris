using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace getris.GameState
{
    class Pile
    {
        //bottom index is 0
        private Cell[,] board;

        public const int ROW_SIZE = 21;
        public const int COL_SIZE = 10;

        // score table for chain removal
        

        // constructor: 1. initialize board with blank cells 
        public Pile()
        {
            board = new Cell[ROW_SIZE+3, COL_SIZE];

            for (int i = 0; i < ROW_SIZE+3; i++)
            {
                for (int j = 0; j < COL_SIZE; j++)
                {
                    board[i, j] = new BlankCell();
                }
            }
        }
        public Cell this[int row, int col]
        {
            get
            {
                return board[row, col];
            }
        }

        public bool IsCellEmpty(int row, int col)
        {
            return board[row, col] is BlankCell;
        }

        private bool isCellCollision(int row, int col, Cell cell)
        {
            if (cell is BlankCell) return false; // always safe
            if (col < 0 || col >= COL_SIZE) return true;
            if (row >= ROW_SIZE) return false;
            if (row < 0) return true;
            if (!IsCellEmpty(row,col)) return true; // if board cell is not empty, it collides
            return false;
        }
        private void PutBlock(int row, int col, Block block)
        {
            for (int i = 0; i < Block.ROW_SIZE; i++)
            {
                for (int j = 0; j < Block.COL_SIZE; j++)
                {
                    if (!(block[i, j] is BlankCell)) // if not empty
                    {
                        board[row + i, col + j] = block[i, j];
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
                    if (isCellCollision(row + i, col + j, block[i, j]))
                        return true;
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

        
        private void FloodFill(int row, int col, bool[,] visit, CellColor par)
        {
            if (row < 0 || row >= ROW_SIZE + 3) return;
            if (col < 0 || col >= COL_SIZE) return;
            if (visit[row, col]) return;
            if (board[row, col] is BlankCell) return;

            if (board[row, col].Color != par) return;

            // in same color
            visit[row, col] = true;
            if (row + 1 < ROW_SIZE + 3)
                FloodFill(row + 1, col, visit, board[row + 1, col].Color); // connect upper
            FloodFill(row - 1, col, visit, par);
            FloodFill(row, col + 1, visit, par);
            FloodFill(row, col - 1, visit, par);
        }

        public ChainResult SimulateChain()
        {
            ChainResult chainResult = new ChainResult();

            bool flgContinue = false;
            do
            {
                List<int> removedLines = new List<int>();
                List<Animation.Drop> dropCells = new List<Animation.Drop>();

                flgContinue = false;
                

                //(1) calculate drops
                bool[,] visit = new bool[ROW_SIZE+3, COL_SIZE];//flood fill color array
                bool flgDown; // flag for gravity down

                do{
                    flgDown = false;
                    for (int i = 0; i < ROW_SIZE + 3; i++)
                    {
                        for (int j = 0; j < COL_SIZE; j++)
                        {
                            visit[i, j] = false;
                        }
                    }

                    for (int j = 0; j < COL_SIZE; j++)
                    {
                        FloodFill(0, j, visit, board[0, j].Color);
                    }
                    for (int i = 1; i < ROW_SIZE+3; i++)
                    {
                        for (int j = 0; j < COL_SIZE; j++)
                        {
                            if (visit[i, j] == false && ((board[i,j] is BlankCell) == false))
                            {
                                int k;
                                flgDown = true;
                                board[i - 1, j] = board[i, j];
                                board[i, j] = new BlankCell();

                                for(k = 0; k < dropCells.Count;k ++){
                                    if (dropCells[k].rowAfter == i && dropCells[k].col == j)
                                    {
                                        Animation.Drop newdrop = new Animation.Drop();
                                        newdrop.rowBefore = dropCells[k].rowBefore;
                                        newdrop.rowAfter = i - 1;
                                        newdrop.col = j;
                                        dropCells[k] = newdrop;
                                        break;
                                    }
                                }
                                if(k >= dropCells.Count){
                                    Animation.Drop newdrop = new Animation.Drop();
                                    newdrop.rowBefore = i;
                                    newdrop.rowAfter = i - 1;
                                    newdrop.col = j;
                                    dropCells.Add(newdrop);
                                }
                            }
                        }
                    }
                    if (flgDown)
                    {
                        flgContinue = true;
                    }
                }while(flgDown);
                //DONE : move all blocks down & add it to drop cells list


                //(2) clear full line
                for (int i = 0; i < ROW_SIZE + 3; i++)
                {
                    int cnt = 0;

                    for (int j = 0; j < COL_SIZE; j++)
                        if (IsCellEmpty(i, j))
                            cnt++;

                    if (cnt == 0)
                    {
                        for (int j = 0; j < COL_SIZE; j++)
                            board[i, j] = new BlankCell();
                        removedLines.Add(i);
                        flgContinue = true;
                    }
                }

                chainResult.Add(new Animation.EraseDropPair(removedLines, dropCells));
                //DONE : combine removedLines & drop cells list to make EraseDropPair
                //DONE : chainResult.animation 에 EraseDropPair 추가하기.

                chainResult.GetScore(removedLines.Count);
                //DONE : calculate score
            } while (flgContinue);

            return chainResult;
        }
    }
}
