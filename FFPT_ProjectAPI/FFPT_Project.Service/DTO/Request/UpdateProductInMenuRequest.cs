using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFPT_Project.Service.DTO.Request
{
    public class UpdateProductInMenuRequest
    {
        public int ProductId { get; set; }
        public int MenuId { get; set; }
        public double? Price { get; set; }
        public int Active { get; set; }
    }
}
