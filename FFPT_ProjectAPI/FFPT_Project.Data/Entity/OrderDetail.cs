using System;
using System.Collections.Generic;

namespace FFPT_Project.Data.Entity
{
    public partial class OrderDetail
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string ProductName { get; set; } = null!;
        public int Quantity { get; set; }
        public double FinalAmount { get; set; }
        public int Status { get; set; }
        public int ProductInMenuId { get; set; }

        public virtual Order Order { get; set; } = null!;
        public virtual ProductInMenu ProductInMenu { get; set; } = null!;
    }
}
