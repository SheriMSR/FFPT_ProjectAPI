using System;
using System.Collections.Generic;

namespace FFPT_Project.Data.Entity
{
    public partial class Product
    {
        public Product()
        {
            ComboProducts = new HashSet<ComboProduct>();
            InverseGeneralProduct = new HashSet<Product>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Image { get; set; }
        public double Price { get; set; }
        public string? Detail { get; set; }
        public int Status { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int CategoryId { get; set; }
        public int Quantity { get; set; }
        public int SupplierStoreId { get; set; }
        public int? GeneralProductId { get; set; }
        public string Code { get; set; } = null!;
        public int ProductInMenuId { get; set; }

        public virtual Category Category { get; set; } = null!;
        public virtual Product? GeneralProduct { get; set; }
        public virtual ProductInMenu ProductInMenu { get; set; } = null!;
        public virtual Store SupplierStore { get; set; } = null!;
        public virtual ICollection<ComboProduct> ComboProducts { get; set; }
        public virtual ICollection<Product> InverseGeneralProduct { get; set; }
    }
}
