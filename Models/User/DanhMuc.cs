using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DUANCUAHANGAPPLE.Models
{
    [Table("DanhMuc")]
    public class DanhMuc
    {
        [Key]
        public int MaDanhMuc { get; set; }

        public string TenDanhMuc { get; set; }

        public bool TrangThai { get; set; }

        public DateTime NgayTao { get; set; }

        public List<SanPham> SanPhams { get; set; }
    }
}