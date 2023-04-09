using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DataGenerator.Models
{
    public class Seller
    {
        public int SellerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime AdmissionDate { get; set; }
        public DateTime? EgressDate { get; set; }

        public string GetInsertQuery()
        {
            StringBuilder query = new();
            query.AppendLine("INSERT INTO Sellers (FirstName, LastName, AdmissionDate, EgressDate)");
            query.AppendLine($"VALUES ('{FirstName}', '{LastName}', '{AdmissionDate}', {(EgressDate.HasValue ? $"'{EgressDate}'" : "NULL")})");
            return query.ToString();
        }

        public static string GetCreateTableQuery()
        {
            StringBuilder query = new();
            query.AppendLine("CREATE TABLE Sellers");
            query.AppendLine("(");

            query.AppendLine("SellerId INT NOT NULL PRIMARY KEY IDENTITY(1,1),");
            query.AppendLine("FirstName VARCHAR(255),");
            query.AppendLine("LastName VARCHAR(255),");
            query.AppendLine("AdmissionDate DATETIME,");
            query.AppendLine("EgressDate DATETIME");

            query.AppendLine(")");

            return query.ToString();
        }
    }
}
