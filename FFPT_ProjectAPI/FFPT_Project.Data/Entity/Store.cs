using System;
using System.Collections.Generic;

namespace FFPT_Project.Data.Entity
{
    public partial class Store
    {
        public Store()
        {
            Products = new HashSet<Product>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Phone { get; set; }
        public string StoreCode { get; set; } = null!;
        public DateTime CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public bool Active { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
