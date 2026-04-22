namespace DUANCUAHANGAPPLE.DTOs
{
    public class OderItemResponseDTO
    {
        public int id { get; set; }

        public string TenKhachHang { get; set; }
        public string SDT { get; set; }

        public string DiaChiGiaoHang { get; set; }

        public DateTime NgayDat { get; set; }
        public decimal TongTien { get; set; }

        public string Status { get; set; }

        public List<OrderDetailDTO> Items { get; set; }
    }
}
