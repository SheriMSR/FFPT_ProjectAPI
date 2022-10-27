using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFPT_Project.Service.DTO.Response
{
    public class BaseResponseMsg
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
    }
}
