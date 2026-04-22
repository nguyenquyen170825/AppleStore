using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DUANCUAHANGAPPLE.Models
{
    [Table("PhieuGiamGiaNguoiDung")]
    public class PhieuGiamGiaNguoiDung
    {
        [Key]
        public int Id { get; set; }

        public int PhieuGiamGiaId { get; set; }
        [ForeignKey("PhieuGiamGiaId")]
        public PhieuGiamGia? PhieuGiamGia { get; set; }

        public int UserId { get; set; } // Đổi từ NguoiDungId sang UserId để khớp với project
        [ForeignKey("UserId")]
        public User? User { get; set; }

        public DateTime NgayNhan { get; set; } = DateTime.Now;
        public bool DaSuDung { get; set; } = false;
        public DateTime? NgaySuDung { get; set; }
    }
}
