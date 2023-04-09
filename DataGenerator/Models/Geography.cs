using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataGenerator.Models
{
    public class Geography
    {
        public int GeographyId { get; set; }
        public string Region { get; set; }
        public string Province { get; set; }
        public string Municipality { get; set; }
        public string Location { get; set; }
        public string PostalCode { get; set; }

        public string GetInsertQuery()
        {
            StringBuilder query = new();
            query.AppendLine("INSERT INTO Geographies (Region, Province, Municipality, Location, PostalCode)");
            query.AppendLine($"VALUES ('{Region}', '{Province}', '{Municipality}', '{Location}', '{PostalCode}')");
            return query.ToString();
        }

        public static string GetCreateTableQuery()
        {
            StringBuilder query = new();
            query.AppendLine("CREATE TABLE Geographies");
            query.AppendLine("(");

            query.AppendLine("GeographyId INT NOT NULL PRIMARY KEY IDENTITY(1,1),");
            query.AppendLine("Region VARCHAR(255),");
            query.AppendLine("Province VARCHAR(255),");
            query.AppendLine("Municipality VARCHAR(255),");
            query.AppendLine("Location VARCHAR(255),");
            query.AppendLine("PostalCode VARCHAR(10)");

            query.AppendLine(")");

            return query.ToString();
        }
    }
}
