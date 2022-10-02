using System;
using System.Collections.Generic;

namespace FFPT_Project.Data.Entity
{
    public partial class Menu
    {
        public Menu()
        {
            ProductInMenus = new HashSet<ProductInMenu>();
        }

        public int Id { get; set; }
        public int TimeFrom { get; set; }
        public int TimeTo { get; set; }
        public string? MenuName { get; set; }
        public int? Type { get; set; }
        public int StoreId { get; set; }
        public int TimeSlotId { get; set; }

        public virtual Store Store { get; set; } = null!;
        public virtual TimeSlot TimeSlot { get; set; } = null!;
        public virtual ICollection<ProductInMenu> ProductInMenus { get; set; }
    }
}
