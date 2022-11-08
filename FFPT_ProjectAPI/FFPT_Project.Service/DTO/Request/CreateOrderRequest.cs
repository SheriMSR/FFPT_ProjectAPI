using FFPT_Project.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFPT_Project.Service.DTO.Request
{
    public class CreateOrderRequest
    {
        public double TotalAmount { get; set; }
        public string? DeliveryPhone { get; set; }
        public int OrderType { get; set; }
        public int TimeSlotId { get; set; }
        public int? RoomId { get; set; }
        public int CustomerId { get; set; }
        public virtual ICollection<OrderDetailRequest> OrderDetails { get; set; }
    }
    public class OrderDetailRequest
    {
        public int ProductInMenuId { get; set; }
        public string ProductName { get; set; }
        public int TimeSlotId { get; set; }
        public int Quantity { get; set; }
        public double? FinalAmount { get; set; }
        public int SupplierStoreId { get; set; }
    }
}
