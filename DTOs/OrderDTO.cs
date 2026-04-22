using System;

namespace DUANCUAHANGAPPLE.DTOs
{
    public class OrderDTO
    {
        public int OrderId { get; set; }
        public string Status { get; set; }
        public string FullName { get; set; }
        public DateTime OrderDate { get; set; }
        public string ShippingAddress { get; set; }
        public string PhoneNumber { get; set; }
        public decimal TotalAmount { get; set; }
        public UserDTO User { get; set; }
    }

    public class UserDTO
    {
        public string FullName { get; set; }
    }
}
