using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace getris.GameState
{
    interface IPile
    {
        Color GetCellColor(int row, int col);
        bool IsCellEmpty(int row, int col);
        int NumRows { get; }
        int NumColumns { get; }
    }
}
