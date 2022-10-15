using System;
using System.Collections.Generic;

namespace FFPT_Project.Data.Entity
{
    public partial class Customer
    {
        public Customer()
        {
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? ImageUrl { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
