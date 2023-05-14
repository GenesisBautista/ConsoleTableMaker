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

namespace ConsoleTableMaker
{
    public class Table : List<Row>
    {
        #region public properties
        public bool HasHeaders { get; set; } = false;
        public int PaddingLeft { get; set; } = 0;
        public int PaddingRight { get; set; } = 0;
        public Color BorderColor { get; set; } = Color.White;
        public bool IntelligibleRows { get; set; } = true;
        public Color HeaderColor { get; set; } = Color.Green;
        public Tuple<Color, Color> DataColor { get; set; } = new Tuple<Color, Color>(Color.White, Color.Gray);
        public bool FullGrid { get; set; } = false;
        #endregion

        #region private properties
        private List<Align> ColumnAlignment = new List<Align>();
        private List<int> ColumnLengths = new List<int>();
        private int ColumnCount = 0;
        #endregion

        #region public methods
        // added validation to existing base.add as well as keeping track of the column length for each column
        public new void Add(Row row)
        {
            ValidateColumnCount(row);
            base.Add(row);
            row.IsAttached = true;
            SetColumnLengths(row);
            if(ColumnAlignment.Count == 0)
            {
                foreach(string s in row)
                {
                    ColumnAlignment.Add(Align.Left);
                }

            }
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

        public void AlignColumnAt(int columnIndex, Align alignment)
        {
            ColumnAlignment[columnIndex] = alignment;
        }

        public void AlignGrid(Align align)
        {
            foreach(int i in ColumnAlignment.Select(index => index))
            {
                ColumnAlignment[i] = align;
            }
        }

        public void DrawGrid()
        {
            Console.Write(ToString());
            //DrawBorder(BorderType.Top);

            //for (int rowIndex = 0; rowIndex < base.Count; rowIndex++)
            //{
            //    Console.Write("│".Pastel(BorderColor));
            //    foreach (var cell in base[rowIndex].Select((data, columnIndex) => (data, columnIndex)))
            //    {
            //        string output = CellBuilder(cell.columnIndex);

            //        Console.Write(output.Pastel(GetCellColor(rowIndex, cell.columnIndex)),
            //            cell.data
            //        );
            //        Console.Write(("│".Pastel(BorderColor)));
            //    }
            //    Console.Write("\n");

            //    if ((HasHeaders && rowIndex == 0) || (FullGrid && rowIndex != base.Count -1))
            //    {
            //        DrawBorder(BorderType.Middle);
            //    }
            //}

            //DrawBorder(BorderType.Bottom);
        }

        public new string ToString()
        {
            string table = "";
            table += DrawBorder(BorderType.Top);

            for (int rowIndex = 0; rowIndex < base.Count; rowIndex++)
            {
                table += "│".Pastel(BorderColor);
                foreach (var cell in base[rowIndex].Select((data, columnIndex) => (data, columnIndex)))
                {
                    string output = CellBuilder(cell.columnIndex);

                    table += string.Format(output, cell.data).Pastel(GetCellColor(rowIndex, cell.columnIndex));
                    table += ("│".Pastel(BorderColor));
                }
                table += "\n";

                if ((HasHeaders && rowIndex == 0) || (FullGrid && rowIndex != base.Count - 1))
                {
                    table += DrawBorder(BorderType.Middle);
                }
            }
            table += DrawBorder(BorderType.Bottom);
            return table;
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
        private string CellBuilder(int columnIndex)
        {
            return CreatePadding(PaddingLeft) + "{0," + AlignCell(columnIndex) + ColumnLengths[columnIndex] + "}" + CreatePadding(PaddingRight);
        }

        private string AlignCell(int columnIndex)
        {
            return ColumnAlignment[columnIndex] == Align.Left ? "-" : "";
        }

        private void SetColumnLengths(Row row)
        {
            foreach (var s in row.Select((data, columnIndex) => (data, columnIndex)))
            {
                if (ColumnLengths.ElementAtOrDefault(s.columnIndex) == 0)
                {
                    ColumnLengths.Add(s.data.ToString().Length);
                }
                else if (s.data.ToString().Length > ColumnLengths[s.columnIndex])
                {
                    ColumnLengths[s.columnIndex] = s.data.ToString().Length;
                }
            }
        }

        private string DrawBorder(BorderType borderType)
        {
            string border = "";
            int sum = (ColumnLengths.Sum() + ColumnLengths.Count + (TotalPadding() * ColumnLengths.Count)) - 1;
            int currentColumn = 0;
            int nextBorder = ColumnLengths[currentColumn] + TotalPadding();

            
            border += DrawBorderEdge(borderType, "┌", "├", "└");           

            for (int i = 0; i < sum; i++)
            {
                if (nextBorder == i)
                {
                    currentColumn++;
                    if (currentColumn < ColumnLengths.Count)
                        nextBorder += ColumnLengths[currentColumn] + 1 + TotalPadding();

                    border += DrawBorderEdge(borderType, "┬", "┼", "┴");
                }
                else
                    border += "─".Pastel(BorderColor);
            }

            border += DrawBorderEdge(borderType, "┐", "┤", "┘");
            border += "\n";

            return border;
        }

        private string DrawBorderEdge(BorderType borderType, string topEdge, string middleEdge, string bottomEdge)
        {
            switch (borderType)
            {
                case BorderType.Top:
                    return topEdge.Pastel(BorderColor);
                case BorderType.Bottom:
                    return bottomEdge.Pastel(BorderColor);
                default:
                    return middleEdge.Pastel(BorderColor);
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

        private Color CurrentDataColor(int rowIndex)
        {
            if (rowIndex == 0 && HasHeaders)
                return HeaderColor;

            if(IntelligibleRows)
            {
                if (rowIndex % 2 == 1)
                    return DataColor.Item1;
                else
                    return DataColor.Item2;
            }
            return DataColor.Item1;
        }

        private Color GetCellColor(int rowIndex, int columnIndex)
        {
            Color? cellColor = base[rowIndex].GetCellColor(columnIndex);
            if (cellColor == null)
                return CurrentDataColor(rowIndex);
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
