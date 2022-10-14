using FFPT_Project.Service.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FFPT_Project.Service.Helpers.Enum;

namespace FFPT_Project.Service.DTO.Response
{
    public class ProductResponse
    {
        [Int]
        public int Id { get; set; }
        [String]
        public string? Name { get; set; } 
        public string? Image { get; set; }
        public double Price { get; set; }
        public string? Detail { get; set; }
        public ProductStatusEnum Status { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int CategoryId { get; set; }
        public int Quantity { get; set; }
        public int SupplierStoreId { get; set; }
        public int? GeneralProductId { get; set; }
        [String]
        public string? Code { get; set; } 
    }
}
