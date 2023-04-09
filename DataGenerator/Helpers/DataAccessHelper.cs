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
    }
}
