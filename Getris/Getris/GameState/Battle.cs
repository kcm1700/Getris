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

        public Battle()
        {
            leftGame = new RunGame();
            rightGame = new DisplayGame();
        }

        public CellColor GetLeftGamePileCellColor(int row, int col)
        {
            return leftGame.GetPileCellColor(row, col);
        }
    }
}
