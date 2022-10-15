using FFPT_Project.Service.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFPT_Project.Service.DTO.Response
{
    public class MenuResponse
    {
        [Int]
        public int Id { get; set; }
        [String]
        public string? MenuName { get; set; }
        [Int]
        public int? Type { get; set; }
        [Int]
        public int TimeSlotId { get; set; }
        public DateTime? CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
    }
}
