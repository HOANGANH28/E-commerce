using System;
using System.Collections.Generic;

namespace EcommerceProject.Models
{
    public partial class Subscribe
    {
        public int Id { get; set; }
        public string? Email { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
