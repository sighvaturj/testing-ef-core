using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using static System.Console;
using Packt.Shared;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace WorkingWithEFCore
{
    class Program
    {
        static void Main(string[] args)
        {
            // QueryingCategories();
            FilteredIncludes();
            // QueryingProducts();
        }

        static void QueryingCategories()
        {
            using (var db = new Northwind())
            {
                // adding logger
                var loggerFactory = db.GetService<ILoggerFactory>();
                loggerFactory.AddProvider(new ConsoleLoggerProvider());
                
                WriteLine("Categories and how many products they have:");
                // fetch data
                IQueryable<Category> cats = db.Categories
                    .Include(c => c.Products);
                foreach (Category c in cats)
                {
                    WriteLine($"{c.CategoryName} has {c.Products.Count} products.");
                }
            }
        }

        static void FilteredIncludes()
        {
            using (var db = new Northwind())
            {
                Write("Enter a minimum for units in stock: ");
                string unitsInStock = ReadLine();
                int stock = int.Parse(unitsInStock);
                // fetch data
                IQueryable<Category> cats = db.Categories
                    .Include(c => c.Products
                    .Where(p => p.Stock >= stock));
                // WriteLine($"ToQueryString: {cats.ToQueryString()}"); // output generated SQL
                foreach (Category c in cats)
                {
                    WriteLine($"{c.CategoryName} has {c.Products.Count} products with a minimum of {stock} units in stock.");
                    foreach (Product p in c.Products)
                    {
                        WriteLine($"-> {p.ProductName} has {p.Stock} units in stock.");                        
                    }
                }
            }
        }

        static void QueryingProducts()
        {
            using (var db = new Northwind())
            {
                // adding logger
                var loggerFactory = db.GetService<ILoggerFactory>();
                loggerFactory.AddProvider(new ConsoleLoggerProvider());
                
                WriteLine("Products that cost more than a price, highest at top.");
                string input;
                decimal price;
                do
                {
                    WriteLine("Enter a product price: ");
                    input = ReadLine();
                }
                while(!decimal.TryParse(input, out price));
                // fetch data
                IQueryable<Product> prods = db.Products
                    .TagWith("Products filtered by price and sorted.")
                    .Where(product => product.Cost > price)
                    .OrderByDescending(product => product.Cost);
                foreach (Product item in prods)
                {
                    WriteLine("ProdID: {0}: {1} costs {2:$#,###0.00} and has {3} in stock.", item.ProductID, item.ProductName, item.Cost, item.Stock);
                }
            }
        }

    }
}
