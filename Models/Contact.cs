using System;
using System.Collections.Generic;

namespace EcommerceProject.Models
{
    public partial class Contact
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? Message { get; set; }

        public virtual User User { get; set; } = null!;
    }
}
