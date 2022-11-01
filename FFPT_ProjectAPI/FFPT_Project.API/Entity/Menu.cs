using System;
using System.Collections.Generic;

namespace FFPT_Project.API.Entity
{
    public partial class Menu
    {
        public Menu()
        {
            ProductInMenus = new HashSet<ProductInMenu>();
        }

        public int Id { get; set; }
        public string? MenuName { get; set; }
        public int? Type { get; set; }
        public int TimeSlotId { get; set; }
        public DateTime? CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }

        public virtual TimeSlot TimeSlot { get; set; } = null!;
        public virtual ICollection<ProductInMenu> ProductInMenus { get; set; }
    }
}
