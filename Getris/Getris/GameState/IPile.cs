using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace getris.GameState
{
    interface IPile
    {
        CellColor GetCellColor(int row, int col);
        bool IsCellEmpty(int row, int col);
    }
}
