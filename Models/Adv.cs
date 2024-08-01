using System;
using System.Collections.Generic;

namespace EcommerceProject.Models
{
    public partial class Adv
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Image { get; set; }
        public string? Description { get; set; }
        public string? Link { get; set; }
        public int? Type { get; set; }
    }
}
