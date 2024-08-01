using System;
using System.Collections.Generic;

namespace EcommerceProject.Models
{
    public partial class CheckOut
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Company { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public int? Phone { get; set; }
        public string? Email { get; set; }
        public string? Note { get; set; }
        public decimal? TotalShip { get; set; }
        public decimal? TotalPrice { get; set; }
        public int CartId { get; set; }
        public virtual Cart? Cart { get; set; }

    }
}
