using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit.Abstractions;

namespace EcommerceProject.Models
{
    public partial class User
    {
        public User()
        {
            Carts = new HashSet<Cart>();
            Contacts = new HashSet<Contact>();
            Testimonials = new HashSet<Testimonial>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }

        public virtual ICollection<Contact> Contacts { get; set; }
        public virtual ICollection<Testimonial> Testimonials { get; set; }
        public virtual ICollection<Cart> Carts { get; set; }
    }
}
