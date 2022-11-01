using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFPT_Project.Service.DTO.Request
{
    public class CreateProductInMenuRequest
    {
        public int MenuId { get; set; }
        public List<ProductInMenuDetail> Products { get; set; }
    }
    public class ProductInMenuDetail
    {
        public int? ProductId { get; set; }
        public double? Price { get; set; }
    }
}
