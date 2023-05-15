# Console Table Maker
Create tables on console.


## Quick Start

To start creating tables you can call the Table class and start adding rows. The table class takes in rows and the row class takes in objects. 

```cs
static void Main(string[] args)
{
    Table table = new Table();
    table.Add(new Row { "1st cell", "2nd cell" });
    table.Add(new Row { "2nd row", "2nd cell of 2nd row" });
    table.Add(new Row { 3, 4 });
    table.DrawTable();
}
```
> put sample01 here

To access each cell you can put its coordinates from the table and change the "data" property. We can add this line to the code above and get the following result:
```cs
static void Main(string[] args)
{
    int columnIndex = 1;
    int rowIndex = 2;
    Table table = new Table();
    table.Add(new Row { "1st cell", "2nd cell" });
    table.Add(new Row { "2nd row", "2nd cell of 2nd row" });
    table.Add(new Row { 3, 4 });

    table[rowIndex][columnIndex].Data = 1;

    table.DrawTable();
}
```
>put sample02 here

## Headers
You can add headers using AddHeaders(row) method this takes in a Row.
```cs
table.AddHeaders(new Row { "Column One", "Column Two" });
```
you can also add headers to the top of your table and set the HasHeaders flag to true and it will net the same result.
```cs
table.Insert(0, (new Row { "Column One", "Column Two" }));
table.HasHeaders = true;
```
>put sample03 here
>
## Table Formatting
### Padding
Padding can be added to create space to the left or right of cells this can be done using the PaddingLeft and PaddingRight properties.
```cs
table.PaddingLeft = 1;
table.PaddingRight = 5;
```
>sample04
### Full Grid
The FullGrid property can be set to true to draw a grid in between each cell.
```cs
table.FullGrid = true;
```
>sample05

### Text Formatting
You can format texts by column using SetColumnFormatAt(index, formatString)
```cs
static void Main(string[] args)
{
    Table table = new Table();
    table.Add(new Row { "Christmas", new DateTime(2023, 12, 25) });
    table.Add(new Row { "New Years", new DateTime(2023, 1, 1) });
    table.AddHeaders(new Row { "Holiday", "Date" });

    table.SetColumnFormatAt(1, "MMMM d");

    table.DrawTable();
}
```
>sample06

There is currently no way to format by each cell or by the whole table. 
### Alignment
#### Table Alignment
You can set table alignment by setting the table property Alignment.
```cs
table.Alignment = Align.Left;
```
#### Column Alignment
You can set column alignment by using the method SetColumnAlignmentAt(columnIndex, alignment)
```cs
table.SetColumnAlignmentAt(0, Align.Right);
```
#### Cell Alignment
You can set cell alignment by setting the cell property Alignment.
```cs
table[rowIndex][columnIndex].Alignment = Align.Left;
```
#### Additional Alignment Notes
There is a hierarchy which the table follows that goes Cell > Column > Table. This means that if you set a cell alignment will overwrite column alignment and column alignment will overwrite table alignment. 
```cs
static void Main(string[] args)
{
    Table table = new Table();
    table.Add(new Row { "1st Cell", "Cell" });
    table.Add(new Row { "2nd Cell", "Also a Cell" });
    table.Add(new Row { "3rd Cell", "Fake Data" });
    table.AddHeaders(new Row { "Super Long Header", "Another Super Long Header" });

    table[2][0].Alignment = Align.Left;
    table.Alignment = Align.Left;
    table.SetColumnAlignmentAt(0, Align.Right);

    table.DrawTable();
}
```
>sample07

### Color
#### Border Color
Border color can be set using BorderColor property on the table.
```cs
table.BorderColor = Color.Red;
```
> sample 08

#### Header Color
Header color can be set using HeaderColor property on the table.
```cs
table.HeaderColor = Color.Blue;
```
> sample09

#### Alternating Color
You can turn alternating color off by setting the AlternatingColor property to false.
```cs
table.AlternatingColor = false;
```
> sample10

You can set the colors that alternate by the table property DataColor which is a tuple of two colors, if alternating colors is turned off the first of these colors will be chosen for the whole table.
```cs
table.DataColor = new Tuple<Color, Color>(Color.LightGreen, Color.GreenYellow);
```
> sample11
#### Column Color
Column color can be changed using SetColumnColorAt(columnIndex, color) method.
```cs
table.SetColumnColorAt(1, Color.Purple);
```
> sample12
#### Cell Color
Cell color can be change by setting the color property of a cell.
```cs
table[rowIndex][columnIndex].Color = Color.Yellow;
```
>sample13
#### Additional Color Notes
You can use RGB values for color by using Color.Argb(int, int, int).
```cs
table[rowIndex][columnIndex].Color = Color.FromArgb(255, 255, 255);
```
The colors follow the same hierarchy as the alignment property. It goes Cell > Column > Table. 