using System;
using System.Collections.Generic;

namespace FFPT_Project.Data.Entity
{
    public partial class ComboProduct
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string? Name { get; set; }
        public string? Discount { get; set; }

        public virtual Product Product { get; set; } = null!;
    }
}
