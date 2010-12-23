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
    }
}
