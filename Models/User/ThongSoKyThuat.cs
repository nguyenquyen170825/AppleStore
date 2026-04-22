using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DUANCUAHANGAPPLE.Models
{
    [Table("ThongSoKyThuat")]
    public class ThongSoKyThuat
    {
        [Key]
        public int Id { get; set; }

        public int SanPhamId { get; set; }

        public int LoaiThongSoId { get; set; }

        public string TenThongSo { get; set; }

        public string GiaTri { get; set; }
        [ForeignKey("SanPhamId")]
        public SanPham? SanPham { get; set; }

        [ForeignKey("LoaiThongSoId")]
        public LoaiThongSo? LoaiThongSo { get; set; }
    }
}