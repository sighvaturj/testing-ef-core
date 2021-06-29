using Microsoft.EntityFrameworkCore;

namespace Packt.Shared
{
    // connection to the database
    public class Northwind : DbContext
    {
        // properties map to tables in the database
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string path = System.IO.Path.Combine(System.Environment.CurrentDirectory, "Northwind.db");
            optionsBuilder.UseSqlite($"Filename={path}");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // use Fluent API instead of attributes
            // to limit the length of a category name to 15
            modelBuilder.Entity<Category>()
                .Property(category => category.CategoryName)
                .IsRequired() // NOT NULL
                .HasMaxLength(15);
            // added to "fix" the lack of decimal support in SQLite
            modelBuilder.Entity<Product>()
            .Property(product => product.Cost)
            .HasConversion<double>();
            // global filter to remove discontinued products
            modelBuilder.Entity<Product>()
            .HasQueryFilter(p => !p.Discontinued);
        }

    }
}