using DataGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataGenerator.Helpers
{
    public static class GeographiesHelper
    {
        private static async Task<List<Geography>> ParseGeographiesCsv(string path)
        {
			try
			{
				if (!File.Exists(path))
					return new();

				List<Geography> geographies = new();
				bool header = true;

				foreach (string line in await File.ReadAllLinesAsync(path))
				{
					if (header)
					{
						header = false;
                        continue;
					}

					var items = line.Split(',');

					if (items.Any() && items.Length == 5)
					{
                        geographies.Add(new()
						{
							Region = items[0],
							Province = items[1],
							Municipality = string.IsNullOrWhiteSpace(items[2]) ? items[1] : items[2],
							Location = items[3],
							PostalCode = items[4]
						});
					}
				}

				return geographies;
			}
			catch (Exception)
			{
				return new();
			}
        }

		public static async Task<int> GenerateGeographiesTable(string path)
		{
			List<Geography> geographies = await ParseGeographiesCsv(path);

			if (!geographies.Any())
				throw new Exception("An error has ocurred while trying to generate the Geographies Table.");

			if (await DataAccessHelper.ExecuteQuery(Geography.GetCreateTableQuery()))
			{
				foreach (var geography in geographies)
					await DataAccessHelper.ExecuteQuery(geography.GetInsertQuery());
            }

			return geographies.Count;
		}

        public static async Task<List<Geography>> GetGeographiesFromDB() => await DataAccessHelper.GetGeographiesAsync();
    }
}
