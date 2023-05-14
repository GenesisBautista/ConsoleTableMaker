using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTableMaker
{
    public class CellProperties
    {  
        public Color? Color { get; set; }
        public Align Alignment { get; set; } = Align.Left;
        public string FormatString { get; set; } = "";

        public CellProperties(Color? color, Align alignment, string formatString)
        {
            Color = color;
            Alignment = alignment;
            FormatString = formatString;
        }
    }
}
