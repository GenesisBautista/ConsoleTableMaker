using System.Drawing;
using System.Net;
using System.Runtime.ConstrainedExecution;
using System.Runtime.Intrinsics.X86;
using ConsoleTableMaker;
using Pastel;

namespace Examples
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SimpleExample();
            FormattingExample();
            ColorExample();
        }

        /// <summary>
        /// You can add headers and it will seperate the headers from the table data
        /// </summary>
        static private void SimpleExample()
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

            table.DrawTable();
        }

        /// <summary>
        /// you can add formatting to each column by using SetColumnFormatAt()
        /// </summary>
        static void FormattingExample()
        {
            Table table = new Table();

            table.AddHeaders(new Row { 
                "Budget Item",
                "Date",
                "Price"
            });

            table.Add(new Row { "Groceries", new DateTime(2023, 6, 4), -154.32 });
            table.Add(new Row { "Electric bill", new DateTime(2023, 6, 10), -200 });
            table.Add(new Row { "Water bill", new DateTime(2023, 6, 15), -59.87 });
            table.Add(new Row { "Cable bill", new DateTime(2023, 6, 5), -99.99 });
            table.Add(new Row { "Mortgage", new DateTime(2023, 6, 20), -2384.5 });
            table.Add(new Row { "Income", new DateTime(2023, 6, 1), 2555.84 });
            table.Add(new Row { "Income", new DateTime(2023, 6, 15), 2555.84 });

            table.SetColumnFormatAt(2, "C");
            table.SetColumnFormatAt(1, "yyyyMMMdd");

            table.DrawTable();
        }

        /// <summary>
        /// Text colors can be set by headers, table, column, or individual cell. 
        /// </summary>
        static void ColorExample()
        {
            Table table = new Table
            {
                new Row { "id", "Departing Airport", "Departing Date", "Arriving Airport", "Arriving Date", "Seat Type"},
                new Row { Guid.NewGuid(), "JUH", new DateTime(2022, 01, 02), "TQN", new DateTime(2023, 10, 28), "First Class" },
                new Row { Guid.NewGuid(), "MID", new DateTime(2022, 10, 08), "SZH", new DateTime(2023, 08, 14), "Economy" },
                new Row { Guid.NewGuid(), "TCR", new DateTime(2022, 03, 28), "YZD", new DateTime(2023, 08, 03), "Economy" },
                new Row { Guid.NewGuid(), "OBL", new DateTime(2022, 06, 10), "HUW", new DateTime(2023, 09, 15), "Economy" },
                new Row { Guid.NewGuid(), "LAN", new DateTime(2022, 03, 12), "BLU", new DateTime(2023, 09, 15), "Business Class" },
                new Row { Guid.NewGuid(), "BCO", new DateTime(2022, 10, 26), "HEH", new DateTime(2023, 05, 31), "Business Class" },
                new Row { Guid.NewGuid(), "BOC", new DateTime(2022, 12, 17), "WHP", new DateTime(2023, 10, 18), "Economy" },
                new Row { Guid.NewGuid(), "MML", new DateTime(2022, 03, 14), "0", new DateTime(2023, 05, 30), "First Class"   },
                new Row { Guid.NewGuid(), "SRS", new DateTime(2022, 12, 30), "MIB", new DateTime(2023, 03, 17), "Business Class" },
                new Row { Guid.NewGuid(), "VIT", new DateTime(2022, 03, 21), "PUK", new DateTime(2023, 08, 19), "Economy" }
            };

            table.HasHeaders = true;

            table[6][5].Color = Color.SeaGreen;
            table[9][5].Color = Color.SeaGreen;

            table.HeaderColor = Color.Blue;
            table.DataColor = new Tuple<Color, Color> (Color.LightBlue, Color.Aqua);
            table.BorderColor = Color.DodgerBlue;
            table.SetColumnColorAt(5, Color.White);
            table[1][5].Color = Color.LightGreen;
            table[8][5].Color = Color.LightGreen;
            table[5][5].Color = Color.SeaGreen;

            table.DrawTable();
        }
    }
}