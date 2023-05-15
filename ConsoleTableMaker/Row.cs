using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTableMaker
{
    public class Row : List<Cell>
    {
        public int ParentColumnCount { get; set; } = 0;
        public bool IsAttached { get; set; } = false;

        public void Add(object obj)
        {
            if (IsAttached && (base.Count + 1) > ParentColumnCount)
            {
                throw new Exception("Added too many cells in this row");
            }
            else
            {
                base.Add(new Cell(obj));
            }
        }
    }
}
