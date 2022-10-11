using System;
using System.Collections.Generic;

namespace FFPT_Project.API.Entity
{
    public partial class TimeSlot
    {
        public TimeSlot()
        {
            Menus = new HashSet<Menu>();
        }

        public int Id { get; set; }
        public TimeSpan ArriveTime { get; set; }
        public TimeSpan CheckoutTime { get; set; }
        public bool? IsActive { get; set; }
        public int Status { get; set; }

        public virtual ICollection<Menu> Menus { get; set; }
    }
}
