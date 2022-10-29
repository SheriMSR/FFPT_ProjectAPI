using System;
using System.Collections.Generic;

namespace FFPT_Project.Data.Entity
{
    public partial class ProductInMenu
    {
        public ProductInMenu()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int Id { get; set; }
        public int ProductId { get; set; }
        public int MenuId { get; set; }
        public double? Price { get; set; }
        public int Active { get; set; }
        public DateTime? CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }

        public virtual Menu Menu { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
