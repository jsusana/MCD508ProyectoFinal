using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataGenerator.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public int SellerId { get; set; }
        public int GeographyId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public int PurcharseType { get; set; } //1 Online-Delivery, 2 InStore-Delivery, 3 InStore-InstantPickUp
        public DateTime OrderDate { get; set; }
        public DateTime ShipDate { get; set; }

        public string GetInsertQuery()
        {
            StringBuilder query = new();
            query.AppendLine("INSERT INTO Orders (SellerId, GeographyId, ProductId, Quantity, PurcharseType, OrderDate, ShipDate)");
            query.AppendLine($"VALUES ({SellerId}, {GeographyId}, {ProductId}, {Quantity}, {PurcharseType}, '{OrderDate}', '{ShipDate}')");
            return query.ToString();
        }

        public static string GetCreateTableQuery()
        {
            StringBuilder query = new();
            query.AppendLine("CREATE TABLE Orders");
            query.AppendLine("(");

            query.AppendLine("OrderId INT NOT NULL PRIMARY KEY IDENTITY(1,1),");
            query.AppendLine("SellerId INT NOT NULL,");
            query.AppendLine("GeographyId INT NOT NULL,");
            query.AppendLine("ProductId INT NOT NULL,");
            query.AppendLine("Quantity INT NOT NULL,");
            query.AppendLine("PurcharseType INT NOT NULL,");
            query.AppendLine("OrderDate DATETIME,");
            query.AppendLine("ShipDate DATETIME,");
            query.AppendLine("");
            query.AppendLine("FOREIGN KEY (SellerId) REFERENCES Sellers(SellerId),");
            query.AppendLine("FOREIGN KEY (GeographyId) REFERENCES Geographies(GeographyId),");
            query.AppendLine("FOREIGN KEY (ProductId) REFERENCES Products(ProductId)");

            query.AppendLine(")");

            return query.ToString();
        }
    }
}
