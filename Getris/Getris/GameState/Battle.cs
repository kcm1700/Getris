using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace getris.GameState
{
    public class Battle
    {
        private Game leftGame;
        private Game rightGame;
        private BlockList blocks;

        public Battle()
        {
            leftGame = new RunGame();
            rightGame = new DisplayGame();
            blocks = new BlockList();
        }

        public CellColor GetLeftGamePileCellColor(int row, int col)
        {
            return leftGame.GetPileCellColor(row, col);
        }
    }
}
