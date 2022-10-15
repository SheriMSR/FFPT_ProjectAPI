using FFPT_Project.Service.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFPT_Project.Service.DTO.Request
{
    public class UpdateProductRequest
    {
        [String]
        public string? Name { get; set; }
        [String]
        public string? Image { get; set; }
        public double Price { get; set; }
        [String]
        public string? Detail { get; set; }
        [Int]
        public int CategoryId { get; set; }
        [Int]
        public int Quantity { get; set; }
        [Int]
        public int SupplierStoreId { get; set; }
        [String]
        public string? Code { get; set; } 
    }
}
