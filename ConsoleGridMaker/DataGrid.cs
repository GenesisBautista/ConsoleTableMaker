using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Pastel;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;

namespace ConsoleGridMaker
{
    public class DataGrid : List<Row>
    {
        #region public properties
        public int ColumnCount { get; set; } = 0;
        public bool HasHeaders { get; set; } = false;
        public int PaddingLeft { get; set; } = 0;
        public int PaddingRight { get; set; } = 0;
        public Align Align { get; set; } = Align.Left;
        public Color BorderColor { get; set; } = Color.White;
        public bool IntelligibleRows { get; set; } = true;
        public Color HeaderColor { get; set; } = Color.Green;
        public Tuple<Color, Color> DataColor { get; set; } = new Tuple<Color, Color>(Color.White, Color.Gray);
        public bool FullGrid { get; set; } = false;
        #endregion

        #region private properties
        private List<int> ColumnLengths = new List<int>();
        #endregion

        #region public methods
        // added validation to existing base.add as well as keeping track of the column length for each column
        public new void Add(Row row)
        {
            ValidateColumnCount(row);
            base.Add(row);
            row.IsAttached = true;
            SetColumnLengths(row);
        }

        // This adds a header row if none exists and replaces header row if one already exists
        public void AddHeaders(Row row)
        {
            ValidateColumnCount(row);
            if (HasHeaders == false)
            {
                Insert(0, row);
                HasHeaders = true;
            }
            else
            {
                base[0] = row;
            }
            SetColumnLengths(row);
        }

        public void DrawGrid()
        {
            DrawBorder(BorderType.Top);

            for (int i = 0; i < base.Count; i++)
            {
                Console.Write("│".Pastel(BorderColor));
                foreach (var cell in base[i].Select((data, index) => (data, index)))
                {
                    string output = CreatePadding(PaddingLeft) + "{0," + (Align == Align.Left ? "-" : "") + ColumnLengths[cell.index] + "}" + CreatePadding(PaddingRight);

                    Console.Write(output.Pastel(GetCellColor(base[i].GetColor(cell.index), i)),
                        cell.data
                    );
                    Console.Write(("│".Pastel(BorderColor)));
                }
                Console.Write("\n");

                if ((HasHeaders && i == 0) || (FullGrid && i != base.Count -1))
                {
                    DrawBorder(BorderType.Middle);
                }
            }

            DrawBorder(BorderType.Bottom);
        }
        #endregion

        #region private methods
        private void ValidateColumnCount(Row row)
        {
            if (ColumnCount == 0)
            {
                ColumnCount = row.Count;
            }
            if (ColumnCount != row.Count)
            {
                throw new Exception($"New row added at Index {Count - 1} does not match the rest of the grid column size");
            }
        }

        #region Grid Drawing
        private void SetColumnLengths(Row row)
        {
            int index = 0;

            foreach (string s in row)
            {
                if (ColumnLengths.ElementAtOrDefault(index) == 0)
                {
                    ColumnLengths.Add(s.Length);
                }
                else if (s.Length > ColumnLengths[index])
                {
                    ColumnLengths[index] = s.Length;
                }
                index++;
            }
        }

        private void DrawBorder(BorderType borderType)
        {
            int sum = (ColumnLengths.Sum() + ColumnLengths.Count + (TotalPadding() * ColumnLengths.Count)) - 1;
            int currentColumn = 0;
            int nextBorder = ColumnLengths[currentColumn] + TotalPadding();

            
            DrawBorderEdge(borderType, "┌", "├", "└");           

            for (int i = 0; i < sum; i++)
            {
                if (nextBorder == i)
                {
                    currentColumn++;
                    if (currentColumn < ColumnLengths.Count)
                        nextBorder += ColumnLengths[currentColumn] + 1 + TotalPadding();

                    DrawBorderEdge(borderType, "┬", "┼", "┴");
                }
                else
                    Console.Write("─".Pastel(BorderColor));
            }

            DrawBorderEdge(borderType, "┐", "┤", "┘");
            Console.Write("\n");
        }

        private void DrawBorderEdge(BorderType borderType, string topEdge, string middleEdge, string bottomEdge)
        {
            switch (borderType)
            {
                case BorderType.Top:
                    Console.Write(topEdge.Pastel(BorderColor));
                    break;
                case BorderType.Bottom:
                    Console.Write(bottomEdge.Pastel(BorderColor));
                    break;
                case BorderType.Middle:
                    Console.Write(middleEdge.Pastel(BorderColor));
                    break;
            }
        }
        #endregion

        #region Grid Formatting
        private string CreatePadding(int paddingCount)
        {
            string padding = "";

            for (int i = 0; i < paddingCount; i++)
            {
                padding += " ";
            }

            return padding;
        }

        private int TotalPadding()
        {
            return PaddingRight + PaddingLeft;
        }

        private Color CurrentDataColor(int index)
        {
            if (index == 0 && HasHeaders)
                return HeaderColor;

            if(IntelligibleRows)
            {
                if (index % 2 == 1)
                    return DataColor.Item1;
                else
                    return DataColor.Item2;
            }
            return DataColor.Item1;
        }

        private Color GetCellColor(Color? cellColor, int i)
        {
            if (cellColor == null)
                return CurrentDataColor(i);
            else
                return (Color)cellColor;
        }
        #endregion
        #endregion
    }

    public enum BorderType
    {
        Top,
        Middle,
        Bottom
    }

    public enum Align
    {
        Left,
        Right
    }
}
