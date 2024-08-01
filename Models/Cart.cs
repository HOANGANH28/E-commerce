using System;
using System.Collections.Generic;

namespace EcommerceProject.Models
{
    public partial class Cart
    {
        public Cart()
        {
            CheckOuts = new HashSet<CheckOut>();
        }
        public int Id { get; set; }
        public int? CouponCode { get; set; }
        public int? Quantity { get; set; }
        public int ProductId { get; set; }
        public int UserId { get; set; }
        public decimal? Price { get; set; }

        public virtual Product? Product { get; set; }
        public virtual User? User { get; set; }
        public virtual ICollection<CheckOut> CheckOuts { get; set; }
    }
}
