using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DUANCUAHANGAPPLE.Models
{
    [Table("HinhAnh")]
    public class HinhAnh
    {
        [Key]
        public int HinhAnhId { get; set; }

        public int BienTheId { get; set; }

        public string UrlHinhAnh { get; set; }

        public bool LaAnhChinh { get; set; }

        public int ThuTu { get; set; }

        public DateTime NgayTao { get; set; }

        [ForeignKey("BienTheId")]
        public BienTheSanPham BienTheSanPham { get; set; }
    }
}