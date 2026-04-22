
using System.ComponentModel.DataAnnotations;    
using System.ComponentModel.DataAnnotations.Schema;

namespace DUANCUAHANGAPPLE.Models
{
    [Table("ChiTietThanhToan")]
    public class ChiTietThanhToan
    {
        public int Id { get; set; }

        // FK tới ThanhToan
        public int ThanhToanId { get; set; }
        [ForeignKey("ThanhToanId")]
        public ThanhToan ThanhToan { get; set; }

        // FK tới SanPham
        public int SanPhamId { get; set; }
        [ForeignKey("SanPhamId")]
        public SanPham SanPham { get; set; }

        // FK tới BienTheSanPham
        public int? BienTheId { get; set; }
        [ForeignKey("BienTheId")]
        public BienTheSanPham? BienTheSanPham { get; set; }

        // snapshot tại thời điểm mua
        public string TenSanPham { get; set; }
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }

        // tính tiền (không lưu DB nếu không config)
        public decimal ThanhTien => SoLuong * DonGia;
    }
}