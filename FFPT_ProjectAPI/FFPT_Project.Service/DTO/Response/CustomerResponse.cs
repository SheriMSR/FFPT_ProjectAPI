using FFPT_Project.Service.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFPT_Project.Service.DTO.Response
{
    public class CustomerResponse
    {
        [Int]
        public int Id { get; set; }
        [String]
        public string Name { get; set; } 
        [String]
        public string Phone { get; set; }
        [String]
        public string Email { get; set; }
        public string ImageUrl { get; set; }
    }
}
