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
        protected override void Start()
        {
            //TODO: start ?
        }
        public override void GoTo(int row, int col)
        {
            //TODO : validate check
            base.GoTo(row, col);
        }
    }
}
