using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FFPT_Project.Service.Helpers.Enum;

namespace FFPT_Project.Service.DTO.Request
{
    public class ProductRequest : PagingRequest
    {
        public ProductStatusEmun Status { get; set; }
    }
}
