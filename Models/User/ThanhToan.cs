using System.ComponentModel.DataAnnotations;    
using System.ComponentModel.DataAnnotations.Schema;

namespace DUANCUAHANGAPPLE.Models
{
    [Table("ThanhToan")]
    public class ThanhToan
    {
        public int Id { get; set; }

        public string MaDonHang { get; set; }
        public DateTime NgayThanhToan { get; set; } = DateTime.Now;
        public decimal TongTien { get; set; }

        public string TrangThai { get; set; } // Chờ xác nhận, Đang giao, Hoàn tất...
        public string PhuongThucThanhToan { get; set; } // COD, Banking...

        // Thông tin Voucher (Nếu có)
        public int? PhieuGiamGiaId { get; set; }
        [ForeignKey("PhieuGiamGiaId")]
        public PhieuGiamGia? PhieuGiamGia { get; set; }
        
        public decimal SoTienGiam { get; set; } = 0; // Lưu số tiền đã giảm được

        // Thông tin người nhận hàng
        public string HoTen { get; set; }
        public string SoDienThoai { get; set; }
        public string Email { get; set; }

        // Địa chỉ giao hàng
        public string TinhThanh { get; set; }
        public string QuanHuyen { get; set; }
        public string PhuongXa { get; set; }
        public string DiaChiChiTiet { get; set; }
        public string? GhiChu { get; set; }

        // FK tới User
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }

        // 1 đơn có nhiều chi tiết
        public List<ChiTietThanhToan> ChiTietThanhToans { get; set; }
    }
}