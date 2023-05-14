using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTableMaker
{
    public class Row : List<object>
    {
        public int ParentColumnCount { get; set; } = 0;
        public bool IsAttached { get; set; } = false;

        private List<CellProperties> cellProperties = new List<CellProperties>();

        public void Add(object obj, Color? color = null, Align cellAlignment = Align.Left, string cellFormatString = "")
        {
            if (IsAttached && (base.Count + 1) > ParentColumnCount)
            {
                throw new Exception("Added too many cells in this row");
            }
            else
            {
                cellProperties.Add(new CellProperties(color, cellAlignment, cellFormatString));
                base.Add(obj);
            }
        }

        public Color? GetCellColor(int index)
        {
            return cellProperties[index].Color;
        }

        public void ChangeColorAt(int index, Color color)
        {
            cellProperties[index].Color = color;
        }


    }
}
