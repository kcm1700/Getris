using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace getris.GameState
{
    abstract class Cell
    {
        private Color blockColor;
        
        public virtual Color maskColor
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
        public BlockCell(Color color)
        {
            maskColor = color;
        }

    }

    class BlankCell : Cell
    {
        public BlankCell()
        {
            //default: invisible color mask
            maskColor = Color.Transparent;
        }
        public BlankCell(Color color)
        {
            maskColor = color;
        }

        public override bool IsEmpty()
        {
            return true;
        }
    }
}
