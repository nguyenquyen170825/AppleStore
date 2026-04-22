using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DUANCUAHANGAPPLE.Models
{
    [Table("PhieuGiamGia")]
    public class PhieuGiamGia
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Ma { get; set; } // Ví dụ: APPLE2024

        [Required]
        public decimal GiaTriGiam { get; set; } // 10 hoặc 100000

        [Required]
        public string LoaiGiam { get; set; } // "PhanTram" hoặc "Tien"

        public decimal? DonHangToiThieu { get; set; } // Điều kiện áp dụng

        public decimal? GiamToiDa { get; set; } // Cho trường hợp giảm %

        public int SoLuong { get; set; } // Tổng số lượng phát hành

        public int DaSuDung { get; set; } // Đã dùng bao nhiêu

        public DateTime NgayBatDau { get; set; }

        public DateTime NgayKetThuc { get; set; }

        public bool TrangThai { get; set; } = true;

        // Navigation
        public ICollection<PhieuGiamGiaNguoiDung>? PhieuNguoiDungs { get; set; }
        public ICollection<ThanhToan>? ThanhToans { get; set; }
    }
}
