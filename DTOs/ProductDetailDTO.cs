namespace DUANCUAHANGAPPLE.DTOs
{
    public class ProductDetailDTO
    {
        public int SanPhamId { get; set; }
        public string TenSanPham { get; set; }
        public string MoTa { get; set; }

        public string ThuongHieu { get; set; }
        public int? MaDanhMuc { get; set; }
        public string TenDanhMuc { get; set; }
        public bool? TrangThai { get; set; }

        // Danh sách biến thể (Màu, Dung lượng...)
        public List<VariantDTO> BienThes { get; set; } = new List<VariantDTO>();

        // THÊM MỚI: 1 object chứa bộ thông số kỹ thuật của sản phẩm
        public ThongSoKyThuatDTO ThongSo { get; set; } 
    }

    // Class ThongSoKyThuatDTO bê nguyên các thuộc tính từ model của bạn sang
    public class ThongSoKyThuatDTO
    {
        public int ThongSoId { get; set; }
        public string ManHinh { get; set; }
        public string CongNgheManHinh { get; set; }
        public string DoPhanGiai { get; set; }
        public string TanSoQuet { get; set; }
        public string CameraSau { get; set; }
        public string CameraTruoc { get; set; }
        public string Chipset { get; set; }
        public string Gpu { get; set; }
        public string Ram { get; set; }
        public string BoNhoTrong { get; set; }
        public string Pin { get; set; }
        public string CongSac { get; set; }
        public string WiFi { get; set; }
        public string Bluetooth { get; set; }
        public string ChongNuoc { get; set; }
    }

    public class VariantDTO
    {
        public int BienTheId { get; set; }
        public string Mau { get; set; }
        public string DungLuong { get; set; }
        public string Ram { get; set; }
        public decimal Gia { get; set; }
        public string Sku { get; set; }
        public decimal? GiaCu { get; set; }
        public int? SoLuongTon { get; set; }
        public List<string> HinhAnhs { get; set; } = new List<string>();
    }
}
