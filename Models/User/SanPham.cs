using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DUANCUAHANGAPPLE.Models
{
    [Table("SanPham")]
    public class SanPham
    {
        [Key]
        public int SanPhamId { get; set; }

        public string TenSanPham { get; set; }

        public string ThuongHieu { get; set; }

        public string MoTa { get; set; }

        public bool TrangThai { get; set; }

        public DateTime NgayTao { get; set; }

        public int MaDanhMuc { get; set; }

        [ForeignKey("MaDanhMuc")]
        public DanhMuc DanhMuc { get; set; }

        public List<BienTheSanPham> BienThes { get; set; }

        public List<ThongSoKyThuat>? ThongSoKyThuats { get; set; }

         public List<ChiTietThanhToan> ChiTietThanhToans { get; set; }
    }
}