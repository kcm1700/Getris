using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace getris.GameState
{
    public enum CellColor
    {
        transparent,
        color1,
        color2,
        color3,
        color4,
        color5
    }
    abstract class Cell
    {
        private CellColor blockColor;
        
        public virtual CellColor maskColor
        {
            get
            {
                return blockColor;
            }
            set
            {
                blockColor = value;
            }
        }

        public virtual bool IsEmpty()
        {
            return false;
        }
    }

    class BlockCell : Cell
    {
        private BlockCell()
        {
        }
        public BlockCell(CellColor color)
        {
            maskColor = color;
        }

    }

    class BlankCell : Cell
    {
        public BlankCell()
        {
            //default: invisible color mask
            maskColor = CellColor.transparent;
        }
        public BlankCell(CellColor color)
        {
            maskColor = color;
        }

        public override bool IsEmpty()
        {
            return true;
        }
    }
}
