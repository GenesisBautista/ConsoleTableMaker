using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTableMaker
{
    public class Cell
    {
        public object Data { get; set; }
        public Color? Color { get; set; } = null;
        public Align? Alignment { get; set; } = null;

        public Cell(object obj) 
        { 
            Data = obj;
        }
    }
}
