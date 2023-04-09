﻿using DataGenerator.Helpers;
using DataGenerator.Models;

// Configurable params: change the connectionString
// and paths to match your environment 

DatabaseConfig dbConfig = new()
{
    ConnectionString = "Server=LAPTOP-G7H787VF;Database=master;Trusted_Connection=True;",
    CatalogName = "AppleStore"
};

string ZipCodesDR_CsvPath = @"C:\maestriacienciadatos\databases\proyecto_final\datasets\ZipCodes_DR.csv";
string AppleProducts_CsvPath = @"C:\maestriacienciadatos\databases\proyecto_final\datasets\AppleProducts.csv";

// Process headers
Console.WriteLine("Apple Store DR Data Generator");
Console.WriteLine($"[{DateTime.Now:MM/dd/yyyy HH:mm:ss}] Starting process...\n");

// Establishing database connection
Console.WriteLine($"[{DateTime.Now:MM/dd/yyyy HH:mm:ss}] Establishing database connection...");

DataAccessHelper.EstablishConnection(dbConfig);

if (!(await DataAccessHelper.ConnectionWasEstablished()))
{
    Console.ForegroundColor = ConsoleColor.White;
    Console.BackgroundColor = ConsoleColor.Red;
    Console.WriteLine($"[{DateTime.Now:MM/dd/yyyy HH:mm:ss}] ERROR! An error has ocurred during 'Establishing database connection'...\n");
    Console.ResetColor();
    return;
}

Console.WriteLine($"[{DateTime.Now:MM/dd/yyyy HH:mm:ss}] Connection has been Established...\n");

// Creating new Database
Console.WriteLine($"[{DateTime.Now:MM/dd/yyyy HH:mm:ss}] Creating new database catalog...");

if (!await DataAccessHelper.InitializeDatabase())
{
    Console.ForegroundColor = ConsoleColor.White;
    Console.BackgroundColor = ConsoleColor.Red;
    Console.WriteLine($"[{DateTime.Now:MM/dd/yyyy HH:mm:ss}] ERROR! An error has ocurred during 'Creating new database catalog'...\n");
    Console.ResetColor();
}

Console.WriteLine($"[{DateTime.Now:MM/dd/yyyy HH:mm:ss}] The new Database catalog has been created...\n");


// Generating the Geographies Table from the CSV
Console.WriteLine($"[{DateTime.Now:MM/dd/yyyy HH:mm:ss}] Generating Geographies Table from CSV...");
int recordsAdded = await GeographiesHelper.GenerateGeographiesTable(ZipCodesDR_CsvPath);

if (recordsAdded <= 0)
{
    Console.ForegroundColor = ConsoleColor.White;
    Console.BackgroundColor = ConsoleColor.Red;
    Console.WriteLine($"[{DateTime.Now:MM/dd/yyyy HH:mm:ss}] ERROR! An error has ocurred during 'Generating Geographies Table from CSV'...\n");
    Console.ResetColor();
}

Console.WriteLine($"[{DateTime.Now:MM/dd/yyyy HH:mm:ss}] Geographies table has been generated ({recordsAdded} records)...\n");


// Generating the Products Table from the CSV
Console.WriteLine($"[{DateTime.Now:MM/dd/yyyy HH:mm:ss}] Generating Products Table from CSV...");
recordsAdded = await ProductsHelper.GenerateProductsTable(AppleProducts_CsvPath);

if (recordsAdded <= 0)
{
    Console.ForegroundColor = ConsoleColor.White;
    Console.BackgroundColor = ConsoleColor.Red;
    Console.WriteLine($"[{DateTime.Now:MM/dd/yyyy HH:mm:ss}] ERROR! An error has ocurred during 'Generating Products Table from CSV'...\n");
    Console.ResetColor();
}

Console.WriteLine($"[{DateTime.Now:MM/dd/yyyy HH:mm:ss}] Products table has been generated ({recordsAdded} records)...\n");


// End of the process
Console.WriteLine($"[{DateTime.Now:MM/dd/yyyy HH:mm:ss}] The process has ended\n");
Console.WriteLine("Press any key to exit...");
Console.ReadKey();

//string currentPath = System.Reflection.Assembly.GetExecutingAssembly().Location;

