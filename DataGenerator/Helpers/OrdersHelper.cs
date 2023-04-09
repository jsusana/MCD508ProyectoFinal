using DataGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataGenerator.Helpers
{
    public static class OrdersHelper
    {
        private static List<Geography> geographies;
        private static List<Product> products;
        private static List<Seller> sellers;

        private static async Task<Order> RandomizeNewOrder()
        {
            Order newOrder = new();

            Random random = new();

            Geography geography = geographies[random.Next(0, geographies.Count - 1)];
            Product product = products[random.Next(0, products.Count - 1)];
            Seller seller = sellers[random.Next(0, sellers.Count - 1)];

            newOrder.SellerId = seller.SellerId;
            newOrder.ProductId = product.ProductId;
            newOrder.GeographyId = geography.GeographyId;

            newOrder.Quantity = random.Next(1, 3);
            newOrder.PurcharseType = random.Next(1, 3);
            newOrder.PurcharseType = random.Next(1, 3);
            newOrder.OrderDate = Utils.GetRandomDateTimeFromYearsRange(seller.AdmissionDate.Year, seller.EgressDate.HasValue ? seller.EgressDate.Value.Year : 2022);
            newOrder.ShipDate = newOrder.PurcharseType == 3 ? newOrder.OrderDate : newOrder.OrderDate.AddDays(random.Next(1, 25));

            return newOrder;
        }

        public static async Task<int> GenerateOrdersTable(int ordersCount, List<Geography> _geographies, List<Product> _products, List<Seller> _sellers)
        {
            geographies = _geographies;
            products = _products;
            sellers = _sellers;

            if (!await DataAccessHelper.ExecuteQuery(Order.GetCreateTableQuery()))
                return 0;

            List<Order> orders = new();
            Order newOrder;

            for (int i = 0; i < ordersCount; i++)
            {
                newOrder = await RandomizeNewOrder();
                orders.Add(newOrder);

                if (orders.Count >= 1000)
                {
                    foreach (var order in orders)
                        await DataAccessHelper.ExecuteQuery(order.GetInsertQuery());

                    orders.Clear();
                    orders = new();
                }
            }

            if (orders.Any())
                foreach (var order in orders)
                    await DataAccessHelper.ExecuteQuery(order.GetInsertQuery());

            return ordersCount;
        }
    }
}
