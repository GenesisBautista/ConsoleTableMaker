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
        public Align Alignment { get; set; } = Align.Left;
        public bool FullGrid { get; set; } = false;
        #endregion

        #region private properties
        private List<Align?> ColumnAlignment = new List<Align?>();
        private List<Color?> ColumnColor = new List<Color?>();
        private List<string?> ColumnFormatString = new List<string?>();
        private List<int> ColumnLengths = new List<int>();
        private int ColumnCount = 0;
        #endregion

        #region public methods
        // added validation to existing base.add as well as keeping track of the column length for each column
        public new void Add(Row row)
        {
            NecessaryOnRowAdd(row);
            base.Add(row);
            row.IsAttached = true;
        }

        // This adds a header row if none exists and replaces header row if one already exists
        public void AddHeaders(Row row)
        {
            NecessaryOnRowAdd(row);
            if (HasHeaders == false)
            {
                Insert(0, row);
                HasHeaders = true;
            }
            else
            {
                base[0] = row;
            }
        }

        public void AlignColumnAt(int columnIndex, Align alignment)
        {
            ColumnAlignment[columnIndex] = alignment;
        }

        public void SetColumnFormatAt(int columnIndex, string formatString)
        {
            // TODO: recount the column lengths with formatting
            ColumnFormatString[columnIndex] = formatString;
        }

        public void SetColumnColorAt(int columnIndex, Color color)
        {
            ColumnColor[columnIndex] = color;  
        }

        public void DrawTable()
        {
            Console.Write(ToString());
        }

        public new string ToString()
        {
            SetColumnLengths();
            string table = "";
            table += DrawBorder(BorderType.Top);

            for (int rowIndex = 0; rowIndex < base.Count; rowIndex++)
            {
                table += "│".Pastel(BorderColor);
                foreach (var tuple in base[rowIndex].Select((cell, columnIndex) => (cell, columnIndex)))
                {
                    string output = CellBuilder(rowIndex, tuple.columnIndex);

                    table += string.Format(output, tuple.cell.Data).Pastel(GetCellColor(rowIndex, tuple.columnIndex));
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
        #region Column Methods
        private void NecessaryOnRowAdd(Row row)
        {
            ValidateColumnCount(row);
            SetDefaultColumnAlignment(row);
            SetDefaultColumnColor(row);
            SetDefaultColumnFormatString(row);
        }
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
        private void SetColumnLengths()
        {
            for(int i = 0; i < base.Count; i++)
            {
                foreach (var tuple in base[i].Select((cell, columnIndex) => (cell, columnIndex)))
                {
                    string formattedString;
                    if (ColumnFormatString[tuple.columnIndex] != null)
                        formattedString = String.Format("{0:" + ColumnFormatString[tuple.columnIndex] + "}", tuple.cell.Data);
                    else
                        formattedString = tuple.cell.Data.ToString() ?? "";

                    if (ColumnLengths.ElementAtOrDefault(tuple.columnIndex) == 0)
                    {
                        ColumnLengths.Add(formattedString.Length);
                    }
                    else if (formattedString.Length > ColumnLengths[tuple.columnIndex])
                    {
                        ColumnLengths[tuple.columnIndex] = formattedString.Length;
                    }
                }
            }
        }

        private void SetDefaultColumnAlignment(Row row)
        {
            if (ColumnAlignment.Count == 0)
            {
                foreach (object obj in row)
                {
                    ColumnAlignment.Add(null);
                }
            }
        }

        private void SetDefaultColumnColor(Row row)
        {
            if (ColumnColor.Count == 0)
            {
                foreach (object obj in row)
                {
                    ColumnColor.Add(null);
                }
            }
        }

        private void SetDefaultColumnFormatString(Row row)
        {
            if (ColumnFormatString.Count == 0)
            {
                foreach (object obj in row)
                {
                    ColumnFormatString.Add(null);
                }
            }
        }
        #endregion

        #region Grid Drawing
        private string CellBuilder(int rowIndex, int columnIndex)
        {
            string cell = "";
            cell += CreatePadding(PaddingLeft);
            cell += "{0," + AlignCell(rowIndex, columnIndex) + ColumnLengths[columnIndex];
            cell += GetFormatString(rowIndex, columnIndex);
            cell += "}";
            cell += CreatePadding(PaddingRight);
            return  cell;
        }

        private string AlignCell(int rowIndex, int columnIndex)
        {
            Align? cellAlignment = base[rowIndex][columnIndex].Alignment;
            Align? columnAlignment = ColumnAlignment[columnIndex];

            if (cellAlignment != null)
                return cellAlignment == Align.Left ? "-" : "";
            else if (columnAlignment != null)
                return columnAlignment == Align.Left ? "-" : "";
            return Alignment == Align.Left ? "-" : "";
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

        private Color GetCellColor(int rowIndex, int columnIndex)
        {
            Color? cellColor = base[rowIndex][columnIndex].Color;
            Color? columnColor = ColumnColor[columnIndex];

            if (rowIndex == 0 && HasHeaders)
                return HeaderColor;

            if (cellColor != null)
                return (Color)cellColor;
            else if (columnColor != null)
                return (Color)columnColor;

            if (IntelligibleRows)
            {
                if (rowIndex % 2 == 1)
                    return DataColor.Item1;
                else
                    return DataColor.Item2;
            }
            return DataColor.Item1;
        }

        private string GetFormatString(int rowIndex, int columnIndex)
        {
            string? columnFormatString = ColumnFormatString[columnIndex];

            if (columnFormatString != null)
                return ":" + columnFormatString;
            return "";
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
