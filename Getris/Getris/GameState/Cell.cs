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
                blockColor = maskColor;
            }
        }

        public virtual bool IsEmpty()
        {
            return false;
        }
    }

    class BlockCell : Cell
    {
        public BlockCell()
        {
            maskColor = Color.Red;
        }

    }

    class BlankCell : Cell
    {
        public BlankCell()
        {
            maskColor = Color.Black;
        }

        public override bool IsEmpty()
        {
            return true;
        }
    }
}
