using FFPT_Project.Data.Entity;
using FFPT_Project.Service.DTO.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFPT_Project.Service.DTO.Response
{
    public class OrderResponse
    {
        public int Id { get; set; }
        public string? OrderName { get; set; }
        public DateTime CheckInDate { get; set; }
        public double TotalAmount { get; set; }
        public double? ShippingFee { get; set; }
        public double FinalAmount { get; set; }
        public int OrderStatus { get; set; }
        public string? DeliveryPhone { get; set; }
        public int OrderType { get; set; }
        public int TimeSlotId { get; set; }
        public int RoomId { get; set; }
        public int SupplierStoreId { get; set; }
        public CustomerResponse CustomerInfo { get; set; }
        public virtual ICollection<OrderDetailResponse> OrderDetails { get; set; }
    }
    public class OrderDetailResponse
    {
        public string ProductName { get; set; } = null!;
        public int Quantity { get; set; }
        public double FinalAmount { get; set; }
        public int Status { get; set; }
        public int ProductInMenuId { get; set; }
    }
}
