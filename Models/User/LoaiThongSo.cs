using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DUANCUAHANGAPPLE.Models
{
    [Table("LoaiThongSo")]
    public class LoaiThongSo
    {
        [Key]
        public int Id { get; set; }

        public string TenLoai { get; set; }

        // 🔗 Navigation
        public List<ThongSoKyThuat>? ThongSoKyThuats { get; set; }
    }
}
