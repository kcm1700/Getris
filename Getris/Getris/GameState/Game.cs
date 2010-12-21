using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace getris.GameState
{
    abstract class Game
    {
        protected Pile pile;
        protected Block block;// 현재 움직이는 블럭
    }
}
