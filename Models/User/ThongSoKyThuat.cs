using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DUANCUAHANGAPPLE.Models
{
    [Table("ThongSoKyThuat")]
    public class ThongSoKyThuat
    {
        [Key]
        public int ThongSoId { get; set; }

        public int SanPhamId { get; set; }

        public string ManHinh { get; set; }

        public string Chipset { get; set; }

        public string CameraSau { get; set; }

        public string CameraTruoc { get; set; }

        public string Pin { get; set; }

        public string WiFi { get; set; }

        public string Bluetooth { get; set; }

        [ForeignKey("SanPhamId")]
        public SanPham SanPham { get; set; }
    }
}