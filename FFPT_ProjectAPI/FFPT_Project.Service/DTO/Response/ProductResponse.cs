using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFPT_Project.Service.DTO.Response
{
    public class ProductResponse
    {
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
    }
}
