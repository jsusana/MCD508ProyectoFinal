using DataGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataGenerator.Helpers
{
    public static class ProductsHelper
    {
        const double UsdToDop = 54.55;
        private static async Task<List<Product>> ParseProductsCsv(string path)
        {
            try
            {
                if (!File.Exists(path))
                    return new();

                List<Product> products = new();
                bool header = true;
                double price = 0.00;

                foreach (string line in await File.ReadAllLinesAsync(path))
                {
                    if (header)
                    {
                        header = false;
                        continue;
                    }

                    var items = line.Split(',');

                    if (items.Any() && items.Length == 2)
                    {
                        if (!double.TryParse(items[1], out price))
                            price = GetRandomPrice(100, 1200);
                    }

                    products.Add(new()
                    {
                        Name = items[0],
                        Price = Math.Round(price * UsdToDop, 2)
                    });
                }

                return products;
            }
            catch (Exception)
            {
                return new();
            }
        }

        private static double GetRandomPrice(double lowerBound, double upperBound)
        {
            var random = new Random();
            var rDouble = random.NextDouble();
            var rRangeDouble = rDouble * (upperBound - lowerBound) + lowerBound;
            return rRangeDouble;
        }

        public static async Task<int> GenerateProductsTable(string path)
        {
            List <Product> products = await ParseProductsCsv(path);

            if (!products.Any())
                throw new Exception("An error has ocurred while trying to generate the Geographies Table.");

            if (await DataAccessHelper.ExecuteQuery(Product.GetCreateTableQuery()))
            {
                foreach (var product in products)
                    await DataAccessHelper.ExecuteQuery(product.GetInsertQuery());
            }

            return products.Count;
        }
    }
}
