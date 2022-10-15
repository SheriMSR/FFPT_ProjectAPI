using FFPT_Project.Service.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFPT_Project.Service.DTO.Request
{
    public class UpdateMenuRequest
    {
        [String]
        public string? MenuName { get; set; }
        [Int]
        public int? Type { get; set; }
        [Int]
        public int TimeSlotId { get; set; }
    }
}
