using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace getris.GameState
{
    class Pile : IPile
    {
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

        // 0 based row & col
        public Color GetCellColor(int row, int col)
        {
            return board[row, col].maskColor;
        }
        // 0 based row & col
        public void SetCellColor(int row, int col, Color colorValue)
        {
            board[row, col].maskColor = colorValue;
        }
        // 0 based row & col
        public bool IsCellEmpty(int row, int col)
        {
            return board[row, col].IsEmpty();
        }
        // put block
        public void DropBlock(int row, int col, Block block)
        {
            //TODO: just put a block, without chain removal
        }
        public LinkedList<Tuple<List<int>, List<Tuple<Tuple<int, int>, int>>>> SimulateChain()
        {
            //TODO: make wrapper class for simulation result.
            return null;
        }
    }
}
