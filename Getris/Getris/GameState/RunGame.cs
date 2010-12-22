using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace getris.GameState
{
    class RunGame : Game
    {
        public RunGame()
        {
            base.pile = new Pile();
        }
        protected override void Start()
        {
        }

        public override void MoveDown()
        {
            //TODO: Validate check
            base.MoveDown();
        }
        public override void MoveLeft()
        {
            //TODO: Validate check
            base.MoveLeft();
        }
        public override void MoveRight()
        {
            //TODO: Validate check
            base.MoveRight();
        }
    }
}
