using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using static System.Console;
using Packt.Shared;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Storage;

namespace WorkingWithEFCore
{
    class Program
    {
        static void Main(string[] args)
        {
            // QueryingCategories();
            FilteredIncludes();
            // QueryingProducts();
            // QueryingWithLike();
            // TEST AddProduct
            // if (AddProduct(6, "Bob's burgers", 500M))
            // {
            //     WriteLine("Add product succesful.");
            // }
            // TEST IncreaseProductPrice
            // if (IncreaseProductPrice("Bob", 20M))
            // {
            //     WriteLine("Update product price succesful.");
            // }
            // TEST DeleteProducts
            // int deleted = DeleteProducts("Bob");
            // WriteLine($"{deleted} product(s) were deleted.");
            // ListProducts();
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
                    WriteLine("ProdID: {0}: {1} costs {2:$#,###0.00} and has {3} in stock.", 
                        item.ProductID, item.ProductName, item.Cost, item.Stock);
                }
            }
        }

        // EF Core supports 'Like' for pattern matching
        static void QueryingWithLike()
        {
            using (var db = new Northwind())
            {
                var loggerFactory = db.GetService<ILoggerFactory>();
                loggerFactory.AddProvider(new ConsoleLoggerProvider());
                Write("Enter a part of a product name: ");
                string input = ReadLine();
                IQueryable<Product> prods = db.Products
                    .Where(p => EF.Functions.Like(p.ProductName, $"%{input}%"));
                foreach (Product item in prods)
                {
                    WriteLine("{0} has {1} units in stock. Discontinued? {2}", 
                        item.ProductName, item.Stock, item.Discontinued);     
                }
            }
        }

        // adding data with EF Core
        static bool AddProduct(int categoryID, string productName, decimal? price)
        {
            using (var db = new Northwind())
            {
                var newProduct = new Product
                {
                    CategoryID = categoryID,
                    ProductName = productName,
                    Cost = price
                };
                // mark product as added in change tracking
                db.Products.Add(newProduct);
                // save tracked change to database
                int affected = db.SaveChanges();
                return (affected == 1);
            }
        }

        static void ListProducts()
        {
            using (var db = new Northwind())
            {
                WriteLine("{0,-3} {1,-35} {2,8} {3,5} {4}", 
                    "ID", "Product Name", "Cost", "Stock", "Disc.");
                foreach (var item in db.Products.OrderByDescending(p => p.Cost))
                {
                    WriteLine("{0:000} {1,-35} {2,8:$#,##0.00} {3,5} {4}", 
                        item.ProductID, item.ProductName, item.Cost, item.Stock, item.Discontinued);
                }
            }
        }

        // updating data with EF Core
        static bool IncreaseProductPrice(string name, decimal amount)
        {
            using (var db = new Northwind())
            {
                // get first product whose name starts with name
                Product updateProduct = db.Products.First(p => p.ProductName.StartsWith(name));
                updateProduct.Cost += amount;
                int affected = db.SaveChanges();
                return (affected == 1);
            }
        }

        // deleting data with EF Core
        static int DeleteProducts(string name)
        {
            // Use explicit transaction
            using (var db = new Northwind())
            {
                using (IDbContextTransaction t = db.Database.BeginTransaction())
                {
                    WriteLine("Transaction isolation level: {0}",
                        t.GetDbTransaction().IsolationLevel);
                    var products = db.Products.Where(p => p.ProductName.StartsWith(name));
                    db.Products.RemoveRange(products);
                    int affected = db.SaveChanges();
                    t.Commit();
                    return affected;
                }
            }

            // Without explicit transactions
            /*
            using (var db = new Northwind())
            {
                IEnumerable<Product> products = db.Products
                    .Where(p => p.ProductName.StartsWith(name));
                db.Products.RemoveRange(products);
                int affected = db.SaveChanges();
                return affected;
            }
            */
        }

    }
}
