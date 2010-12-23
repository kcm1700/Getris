using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace getris.Animation
{
    struct Drop{
        public int rowBefore, col;
        public int rowAfter;
    }
    class EraseDropPair
    {
        public List<Drop> dropCellList; // includes all drop cells.
        public List<int> erasedLineList;

        public EraseDropPair()
        {
            erasedLineList = new List<int>();
            dropCellList = new List<Drop>();
        }
        public EraseDropPair(List<int> erasedLineList, List<Drop> dropCellList)
        {
            this.erasedLineList = erasedLineList;
            this.dropCellList = dropCellList;
        }
    }
}
