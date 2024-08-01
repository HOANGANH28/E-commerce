using System;
using System.Collections.Generic;

namespace EcommerceProject.Models
{
    public partial class Category
    {
        public Category()
        {
            Products = new HashSet<Product>();
        }

        public int Id { get; set; }
        public string? Title { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
