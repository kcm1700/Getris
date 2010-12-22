using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace getris.GameState
{
    class RunGame : Game
    {
        public RunGame()
        {
        }
        private void MoveDown()  
        {
            //TODO: Validate check
            block.MoveDown();
        }
        private void MoveLeft()
        {
            //TODO: Validate check
            block.MoveLeft();
        }
        private void MoveRight()
        {
            //TODO: Validate check
            block.MoveRight();
        }
    }
}
