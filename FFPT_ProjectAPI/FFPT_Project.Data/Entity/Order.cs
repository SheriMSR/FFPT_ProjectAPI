using System;
using System.Collections.Generic;

namespace FFPT_Project.Data.Entity
{
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int Id { get; set; }
        public string? OrderName { get; set; }
        public DateTime CheckInDate { get; set; }
        public double TotalAmount { get; set; }
        public double? ShippingFee { get; set; }
        public double FinalAmount { get; set; }
        public int OrderStatus { get; set; }
        public int CustomerId { get; set; }
        public string? DeliveryPhone { get; set; }
        public int OrderType { get; set; }
        public int TimeSlotId { get; set; }
        public int RoomId { get; set; }

        public virtual Customer Customer { get; set; } = null!;
        public virtual Room Room { get; set; } = null!;
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
