using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DUANCUAHANGAPPLE.Models
{
    [Table("GioHang")]
    public class GioHang
    {
        [Key]
        public int MaGioHang { get; set; }

        public int MaNguoiDung { get; set; }
        public int BienTheId { get; set; }

        public int SoLuong { get; set; }

        public DateTime NgayThem { get; set; }

        [ForeignKey("MaNguoiDung")]
        public User User { get; set; }

        [ForeignKey("BienTheId")]
        public BienTheSanPham BienTheSanPham { get; set; }
    }
}   