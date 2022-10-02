using System;
using System.Collections.Generic;

namespace FFPT_Project.Data.Entity
{
    public partial class ProductInMenu
    {
        public ProductInMenu()
        {
            OrderDetails = new HashSet<OrderDetail>();
            Products = new HashSet<Product>();
        }

        public int Id { get; set; }
        public int StoreId { get; set; }
        public int MenuId { get; set; }
        public double Price { get; set; }
        public int Active { get; set; }
        public DateTime CreateAt { get; set; }

        public virtual Menu Menu { get; set; } = null!;
        public virtual Store Store { get; set; } = null!;
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
