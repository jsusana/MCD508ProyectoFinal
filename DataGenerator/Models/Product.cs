using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataGenerator.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }

        public string GetInsertQuery()
        {
            StringBuilder query = new();
            query.AppendLine("INSERT INTO Products (Name, Price)");
            query.AppendLine($"VALUES ('{Name}', {Price})");
            return query.ToString();
        }

        public static string GetCreateTableQuery()
        {
            StringBuilder query = new();
            query.AppendLine("CREATE TABLE Products");
            query.AppendLine("(");

            query.AppendLine("ProductId INT NOT NULL PRIMARY KEY IDENTITY(1,1),");
            query.AppendLine("Name VARCHAR(255),");
            query.AppendLine("Price DECIMAL(12,2)");

            query.AppendLine(")");

            return query.ToString();
        }
    }
}
