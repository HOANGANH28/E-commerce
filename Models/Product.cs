using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceProject.Models
{
    public partial class Product
    {
        public Product()
        {
            Carts = new HashSet<Cart>();
            Comments = new HashSet<Comment>();
        }

        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Image { get; set; }
        public string? Detail { get; set; }
        public string? Description { get; set; }
        public string? Star { get; set; }
        public decimal? Price { get; set; }
        public decimal? PriceSale { get; set; }
        public int? TotalQuantity { get; set; }
        public bool IsSale { get; set; }
        public bool IsFeature { get; set; }
        public bool IsHot { get; set; }
        public bool IsArrive { get; set; }
        public int? CategoryId { get; set; }

        public virtual Category? Category { get; set; } 
        public virtual ICollection<Cart> Carts { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
