using DataGenerator.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataGenerator.Helpers
{
    public static class DataAccessHelper
    {
        private static SqlConnection dbConnection;
        private static DatabaseConfig config;
        private static bool DatabaseHasBeenCreated = false;

        public static async void EstablishConnection(DatabaseConfig databaseConfig)
        {
            config = databaseConfig;
            dbConnection = new SqlConnection(databaseConfig.ConnectionString);
        }

        public static async Task<bool> ConnectionWasEstablished()
        {
            try
            {
                if (dbConnection == null)
                    return false;

                await dbConnection.OpenAsync();
                await dbConnection.CloseAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public static async Task<bool> InitializeDatabase()
        {
            try
            {
                await dbConnection.OpenAsync();

                string query = @$"IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = '{config.CatalogName}')
                                    CREATE DATABASE [{config.CatalogName}]";
                SqlCommand command = new(query, dbConnection);
                await command.ExecuteNonQueryAsync();
                await dbConnection.ChangeDatabaseAsync(config.CatalogName);
                DatabaseHasBeenCreated = true;
            }
            catch (Exception)
            {
                DatabaseHasBeenCreated = false;
            }
            finally
            {
                await dbConnection.CloseAsync();
            }

            return DatabaseHasBeenCreated;
        }

        public static async Task<bool> ExecuteQuery(string query)
        {
            try
            {
                if (await ConnectionWasEstablished())
                {
                    await dbConnection.OpenAsync();
                    await dbConnection.ChangeDatabaseAsync(config.CatalogName);
                    SqlCommand command = new(query, dbConnection);
                    await command.ExecuteNonQueryAsync();
                    return true;
                }
                else
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                await dbConnection.CloseAsync();
            }
        }

        public static async Task<List<Seller>> GetSellersAsync()
        {
            List<Seller> sellers = new();

            if (await ConnectionWasEstablished())
            {
                await dbConnection.OpenAsync();
                await dbConnection.ChangeDatabaseAsync(config.CatalogName);
                string query = "SELECT * FROM Sellers";
                SqlCommand command = new SqlCommand(query, dbConnection);
                SqlDataReader reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    sellers.Add(new()
                    {
                        SellerId = reader.GetInt32(0),
                        FirstName = reader.GetString(1),
                        LastName = reader.GetString(2),
                        AdmissionDate = reader.GetDateTime(3),
                        EgressDate = reader.IsDBNull(4) ? null : reader.GetDateTime(4),
                    });
                }
            }
            else
                return new();

            return sellers;
        }

        public static async Task<List<Product>> GetProductsAsync()
        {
            List<Product> products = new();

            if (await ConnectionWasEstablished())
            {
                await dbConnection.OpenAsync();
                await dbConnection.ChangeDatabaseAsync(config.CatalogName);
                string query = "SELECT * FROM Products";
                SqlCommand command = new SqlCommand(query, dbConnection);
                SqlDataReader reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    products.Add(new()
                    {
                        ProductId = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Price = reader.GetDouble(2)
                    });
                }
            }
            else
                return new();

            return products;
        }

        public static async Task<List<Geography>> GetGeographiesAsync()
        {
            List<Geography> geographies = new();

            if (await ConnectionWasEstablished())
            {
                await dbConnection.OpenAsync();
                await dbConnection.ChangeDatabaseAsync(config.CatalogName);
                string query = "SELECT * FROM Geographies";
                SqlCommand command = new SqlCommand(query, dbConnection);
                SqlDataReader reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    geographies.Add(new()
                    {
                        GeographyId = reader.GetInt32(0),
                        Region = reader.GetString(1),
                        Province = reader.GetString(2),
                        Municipality = reader.GetString(3),
                        Location = reader.GetString(4),
                        PostalCode = reader.GetString(5)
                    });
                }
            }
            else
                return new();

            return geographies;
        }
    }
}
