using FFPT_Project.Service.Commons;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFPT_Project.Service.DTO.Response
{
    public class TimeslotResponse
    {
        [Int]
        public int Id { get; set; }
        public TimeSpan ArriveTime { get; set; }
        public TimeSpan CheckoutTime { get; set; }
        [Boolean]
        public bool? IsActive { get; set; }
        [Int]
        public int Status { get; set; }
    }
}
