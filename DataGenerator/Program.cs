using DataGenerator.Helpers;
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
int sellersCount = 50;

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


// Generating the Seller Table from random generated data
Console.WriteLine($"[{DateTime.Now:MM/dd/yyyy HH:mm:ss}] Generating Seller Table from random generated data...");
recordsAdded = await SellersHelper.GenerateSellersTable(20);

if (recordsAdded <= 0)
{
    Console.ForegroundColor = ConsoleColor.White;
    Console.BackgroundColor = ConsoleColor.Red;
    Console.WriteLine($"[{DateTime.Now:MM/dd/yyyy HH:mm:ss}] ERROR! An error has ocurred during 'Generating Seller Table from random generated data'...\n");
    Console.ResetColor();
}

Console.WriteLine($"[{DateTime.Now:MM/dd/yyyy HH:mm:ss}] Sellers table has been generated ({recordsAdded} records)...\n");

// Retrieving Geographies, Products and Sellers data from database
Console.WriteLine($"[{DateTime.Now:MM/dd/yyyy HH:mm:ss}] Retrieving Geographies, Products and Sellers data from database...");

List<Geography> geographies = await GeographiesHelper.GetGeographiesFromDB();
List<Product> products = await ProductsHelper.GetProductsFromDB();
List<Seller> sellers = await SellersHelper.GetSellersFromDB();

if (!geographies.Any() || !products.Any() || !sellers.Any())
{
    Console.ForegroundColor = ConsoleColor.White;
    Console.BackgroundColor = ConsoleColor.Red;
    Console.WriteLine($"[{DateTime.Now:MM/dd/yyyy HH:mm:ss}] ERROR! An error has ocurred during 'Retrieving Geographies, Products and Sellers data from database'...\n");
    Console.ResetColor();
}

Console.WriteLine($"[{DateTime.Now:MM/dd/yyyy HH:mm:ss}] Retrieved data from the database {{Geographies: {geographies.Count}, Products: {products.Count}, Sellers: {sellers.Count} }}...\n");

// End of the process
Console.WriteLine($"[{DateTime.Now:MM/dd/yyyy HH:mm:ss}] The process has ended\n");
Console.WriteLine("Press any key to exit...");
Console.ReadKey();

//string currentPath = System.Reflection.Assembly.GetExecutingAssembly().Location;

