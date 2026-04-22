namespace DUANCUAHANGAPPLE.DTOs
{
    public class ProductDTO
    {
        public int SanPhamId { get; set; }
        
        public string TenSanPham { get; set; }
        
        public string TenDanhMuc { get; set; }
        
        // Có thể lấy giá nhỏ nhất từ các biến thể để làm giá đại diện
        public decimal GiaGoc { get; set; } 
        
        // Chỉ lấy 1 URL ảnh chính để load danh sách cho nhanh
        public string HinhAnhDaiDien { get; set; } 
        
        // Tổng số lượng tồn của tất cả biến thể cộng lại
        public int TongSoLuongTon { get; set; } 
        
        public bool? TrangThai { get; set; }
    }
}
