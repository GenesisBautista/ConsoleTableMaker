using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGridMaker
{
    public class Row : List<string>
    {
        public int ParentColumnCount { get; set; } = 0;
        public bool IsAttached { get; set; } = false;

        private List<Color?> Colors = new List<Color?>();

        public new void Add(string s)
        {
            if (IsAttached && (base.Count + 1) <= ParentColumnCount)
            {
                Colors.Add(null);
                base.Add(s);
            }
            else if (IsAttached && (base.Count + 1) > ParentColumnCount)
            {
                throw new Exception("Added too many cells in this row");
            }
            else
            {
                Colors.Add(null);
                base.Add(s);
            }
        }

        public void Add(string s, Color color)
        {
            if (IsAttached && (base.Count + 1) <= ParentColumnCount)
            {
                Colors.Add(color);
                base.Add(s);
            }
            else if (IsAttached && (base.Count + 1) > ParentColumnCount)
            {
                throw new Exception("Added too many cells in this row");
            }
            else
            {
                Colors.Add(color);
                base.Add(s);
            }
        }

        public Color? GetColor(int i)
        {
            return Colors[i];
        }

        public void ChangeColorAt(int index, Color color)
        {
            Colors[index] = color;
        }
    }
}
