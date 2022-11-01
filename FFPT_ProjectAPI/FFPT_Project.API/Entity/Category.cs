using System;
using System.Collections.Generic;

namespace FFPT_Project.API.Entity
{
    public partial class Category
    {
        public Category()
        {
            Products = new HashSet<Product>();
        }

        public int Id { get; set; }
        public string CategoryName { get; set; } = null!;
        public int? Status { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public string? Description { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
