using System;
using System.Collections.Generic;

namespace FFPT_Project.API.Entity
{
    public partial class Floor
    {
        public Floor()
        {
            Rooms = new HashSet<Room>();
        }

        public int Id { get; set; }
        public string FloorNumber { get; set; } = null!;

        public virtual ICollection<Room> Rooms { get; set; }
    }
}
