using System;
using System.Collections.Generic;

namespace FFPT_Project.Data.Entity
{
    public partial class TimeSlot
    {
        public TimeSlot()
        {
            Menus = new HashSet<Menu>();
        }

        public int Id { get; set; }
        public string ArriveTime { get; set; } = null!;
        public string CheckoutTime { get; set; } = null!;
        public bool? IsActive { get; set; }
        public bool? IsAvailable { get; set; }

        public virtual ICollection<Menu> Menus { get; set; }
    }
}
