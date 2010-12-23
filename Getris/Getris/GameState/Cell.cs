using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace getris.GameState
{
    public enum CellColor
    {
        transparent = 0,
        color1 = 1,
        color2 = 2,
        color3 = 3,
        color4 = 4,
        color5 = 5
    }
    abstract class Cell
    {
        protected CellColor blockColor;
        
        public virtual CellColor Color
        {
            get
            {
                return blockColor;
            }
        }
    }

    class BlockCell : Cell
    {
        private BlockCell()
        {
        }
        public BlockCell(CellColor color)
        {
            blockColor = color;
        }

    }

    class BlankCell : Cell
    {
        public BlankCell()
        {
            //default: invisible color mask
            blockColor = CellColor.transparent;
        }
        public BlankCell(CellColor color)
        {
            blockColor = color;
        }

    }
}
