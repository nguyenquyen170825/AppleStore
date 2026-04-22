using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DUANCUAHANGAPPLE.Models
{
    [Table("BienTheSanPham")]
    public class BienTheSanPham
    {
        [Key]
        public int BienTheId { get; set; }

        public int SanPhamId { get; set; }

        public string Mau { get; set; }

        public string DungLuong { get; set; }

        public string? Ram { get; set; }

        public decimal Gia { get; set; }

        public decimal? GiaCu { get; set; }

        public decimal? GiamGia { get; set; }

        public int SoLuongTon { get; set; }

        public bool TrangThai { get; set; }

        public DateTime NgayTao { get; set; }

        [ForeignKey("SanPhamId")]
        public SanPham? SanPham { get; set; }

        public List<HinhAnh>? HinhAnhs { get; set; }
        public List<ChiTietThanhToan>? ChiTietThanhToans { get; set; }
    }
}