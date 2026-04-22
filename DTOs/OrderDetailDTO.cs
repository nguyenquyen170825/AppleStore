namespace DUANCUAHANGAPPLE.DTOs
{
    public class OrderDetailDTO
    {
        public int BienTheID { get; set; }

        public string TenSanPham { get; set; }

        public string Mau { get; set; }
        public string DungLuong { get; set; }

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public string ImageUrl { get; set; }
    }
}
