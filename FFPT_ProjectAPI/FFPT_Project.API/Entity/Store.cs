using System;
using System.Collections.Generic;

namespace FFPT_Project.API.Entity
{
    public partial class Store
    {
        public Store()
        {
            Orders = new HashSet<Order>();
            ProductInMenus = new HashSet<ProductInMenu>();
            Products = new HashSet<Product>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Phone { get; set; }
        public string StoreCode { get; set; } = null!;
        public DateTime CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public bool Active { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<ProductInMenu> ProductInMenus { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
