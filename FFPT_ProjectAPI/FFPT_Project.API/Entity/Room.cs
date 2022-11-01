using System;
using System.Collections.Generic;

namespace FFPT_Project.API.Entity
{
    public partial class Room
    {
        public Room()
        {
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string RoomNumber { get; set; } = null!;
        public int FloorId { get; set; }
        public int AreaId { get; set; }

        public virtual Area Area { get; set; } = null!;
        public virtual Floor Floor { get; set; } = null!;
        public virtual ICollection<Order> Orders { get; set; }
    }
}
