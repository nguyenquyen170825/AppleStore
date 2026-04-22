using System.Collections.Generic;

namespace DUANCUAHANGAPPLE.DTOs
{
    public class CreateOrderDTO
    {
        public string PhoneNumber { get; set; }
        public string ShippingAddress { get; set; }
        public int? UserId { get; set; }
        public List<CreateOrderItemDTO> Items { get; set; } = new List<CreateOrderItemDTO>();
    }

    public class CreateOrderItemDTO
    {
        public int BienTheId { get; set; }
        public int Quantity { get; set; }
        public int UnitPrice { get; set; }
    }
}
