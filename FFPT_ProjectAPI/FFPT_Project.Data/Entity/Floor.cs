using System;
using System.Collections.Generic;

namespace FFPT_Project.Data.Entity
{
    public partial class Floor
    {
        public Floor()
        {
            Rooms = new HashSet<Room>();
        }

        public int Id { get; set; }
        public int FloorNumber { get; set; }

        public virtual ICollection<Room> Rooms { get; set; }
    }
}
