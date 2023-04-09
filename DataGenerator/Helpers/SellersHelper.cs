using DataGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataGenerator.Helpers
{
    public static class SellersHelper
    {
        private static async Task<Seller> RandomizeNewSeller()
        {
            Seller seller = new();
            Random random = new Random();

            string name = await Utils.GetRandomName(random.Next());

            seller.FirstName = name.Split("_")[0];
            seller.LastName = name.Split("_")[1];

            seller.AdmissionDate = Utils.GetRandomDateTimeFromYearsRange(2012, 2022);

            if ((random.Next() % 2) == 0)
                seller.EgressDate = Utils.GetRandomDateTimeFromYearsRange(2021, 2021);

            return seller;
        }

        public static async Task<int> GenerateSellersTable(int sellersCount)
        {
            List<Seller> sellers = new();
            Seller newSeller = new();

            for (int i = 0; i < sellersCount; i++)
            {
                newSeller = await RandomizeNewSeller();
                sellers.Add(newSeller);
            }

            if (!sellers.Any())
                throw new Exception("An error has ocurred while trying to generate the Sellers Table.");

            if (await DataAccessHelper.ExecuteQuery(Seller.GetCreateTableQuery()))
            {
                foreach (var seller in sellers)
                    await DataAccessHelper.ExecuteQuery(seller.GetInsertQuery());
            }

            return sellers.Count;
        }
    }
}
