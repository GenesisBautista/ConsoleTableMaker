using System.Drawing;
using ConsoleTableMaker;
using Pastel;

namespace Examples
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Example1();
            Example2();
            Example3();
        }

        static private void Example1()
        {
            Table table = new Table {
                new Row { "Freemon","Cranmore","fcranmore1@webs.com","560-975-1941","8977 Northland Hill" },
                new Row { "Dion","Feldman","dfeldman2@yellowbook.com","172-904-6740","652 Everett Avenue" },
                new Row { "Giffie", "Attawell", "gattawell3@bravesites.com", "386-317-5262", "96964 Randy Circle" },
                new Row { "Viv", "Wilman", "vwilman4@skype.com", "118-344-7212", "57 Bunker Hill Pass" },
                new Row { "Stephanus", "Crumby", "scrumby5@sciencedaily.com", "459-297-4008", "45735 Ronald Regan Road" },
                new Row { "Robenia", "Silmon", "rsilmon6@delicious.com", "396-793-7999", "34 Gale Pass" },
                new Row { "Alicia", "Wenman", "awenman7@nature.com", "678-381-8855", "245 Upham Street" },
                new Row { "Carlin", "Darlington", "cdarlington8@state.gov", "198-588-9032", "84 Sachs Junction" },
                new Row { "Rivalee","Dodgshon","rdodgshon9@examiner.com","606-575-3779","230 Golf Circle"}
            };

            table.AddHeaders(new Row()
            {
                "First Name",
                "Last Name",
                "Email",
                "Phone Number",
                "Street Address"
            });

            table.HeaderColor = Color.Aquamarine;
            table.BorderColor = Color.Salmon;
            table.DataColor = new Tuple<Color, Color>(Color.Pink, Color.PaleVioletRed);
            table.PaddingLeft = 1;
            table.PaddingRight = 1;
            table.AlignColumnAt(4, Align.Right);

            table.DrawGrid();
        }

        static void Example2()
        {
            Row row1 = new Row { "Spending", "-$500" };
            Row row2 = new Row { "Earnings", "$1000" };

            Table table = new Table();

            table.Add(row1);
            table.Add(row2);

            table.AddHeaders(new Row()
            {
                "Budget Item",
                "Amount"
            });

            table[1].SetCellColorAt(1, Color.Red);
            table[2].SetCellColorAt(1, Color.Green);

            table.IntelligibleRows = false;

            table.DrawGrid();
        }

        static void Example3()
        {
            Table table = new Table();

            table.AddHeaders(new Row { 
                "Test"
            });

            table.DrawGrid();
        }
    }
}