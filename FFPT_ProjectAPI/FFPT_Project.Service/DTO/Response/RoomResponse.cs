using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFPT_Project.Service.DTO.Response
{
    public class RoomResponse
    {
        public int? Id { get; set; }
        public string? RoomNumber { get; set; }
        public int? FloorId { get; set; }
        public string? FloorNumber { get; set; }
        public int? AreaId { get; set; }
        public string? AreaName { get; set; }
    }
}
