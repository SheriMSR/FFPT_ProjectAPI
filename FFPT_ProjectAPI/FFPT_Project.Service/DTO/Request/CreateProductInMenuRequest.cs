using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFPT_Project.Service.DTO.Request
{
    public class CreateProductInMenuRequest
    {
        public int ProductId { get; set; }
        public List<int> Menu { get; set; }
        public double? Price { get; set; }
    }
}
