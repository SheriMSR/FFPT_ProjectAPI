using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFPT_Project.Service.DTO.Response
{
    public class ProductInMenuResponse
    {
        public int ProductMenuId { get; set; }
        public int StoreId { get; set; }
        public String? StoreName { get; set; }
        public String? ProductName { get; set; }
        public string? Image { get; set; }
        public string? Detail { get; set; }
        public int? MenuId { get; set; }
        public String? MenuName { get; set; }
        public double? Price { get; set; }
        public int? Status { get; set; }
        public DateTime? CreateAt { get; set; }         
        public DateTime? UpdateAt { get; set; }
    }
}
