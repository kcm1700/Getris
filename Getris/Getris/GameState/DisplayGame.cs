using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace getris.GameState
{
    class DisplayGame : Game
    {
        public DisplayGame()
        {
        }
        private void GoTo(int row, int col)
        {
            //TODO : validate check
            block.GoTo(row, col);
        }
    }
}
