using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Packt.Shared
{
    public class Category
    {
        // properties mapped to columns in the database
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        [Column(TypeName = "ntext")]
        public string Description { get; set; }
        // navigation property for related rows
        public virtual ICollection<Product> Products { get; set; }

        // constructs
        public Category()
        {
            // initialize navigation property to an empty collection
            this.Products = new HashSet<Product>();
        }
    }
}