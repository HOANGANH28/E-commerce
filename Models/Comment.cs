using System;
using System.Collections.Generic;

namespace EcommerceProject.Models
{
    public partial class Comment
    {
        public int Id { get; set; }
        public string? Content { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? User { get; set; }
        public int ProductId { get; set; }

        public virtual Product? Product { get; set; }
        
    }
}
