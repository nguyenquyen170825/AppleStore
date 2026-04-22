using System.ComponentModel.DataAnnotations;

namespace DUANCUAHANGAPPLE.ViewModels
{
    public class DanhMucVM
    {
        [Required(ErrorMessage = "Vui lòng nh?p tên danh m?c!")]
        [StringLength(100, ErrorMessage = "Tên danh m?c không du?c vu?t quá 100 ký t?")]
        public string TenDanhMuc { get; set; } = string.Empty;

        public string? MoTa { get; set; }

        // M?c d?nh là true (Hi?n th?/Ðang ho?t d?ng)
        public bool TrangThai { get; set; } = true;
    }
}
