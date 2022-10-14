using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFPT_Project.Service.DTO.Response
{
    public class AuthResponse
    {
        public bool IsNewCustomer { get; set; }
        public bool IsAuthSuccessful { get; set; }
        public CustomerResponse customer { get; set; }
    }
}
