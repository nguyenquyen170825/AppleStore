using System.ComponentModel.DataAnnotations;

namespace DUANCUAHANGAPPLE.ViewModels
{
    public class ProductVM
    {
        [Required(ErrorMessage = "Vui lòng nhập tên sản phẩm!")]
        [StringLength(200, ErrorMessage = "Tên sản phẩm không được vượt quá 200 ký tự")]
        public string TenSanPham { get; set; } = string.Empty;

        // Dùng int? (nullable) để dễ dàng bắt lỗi khi người dùng chưa chọn danh mục (value rỗng)
        [Required(ErrorMessage = "Vui lòng chọn danh mục!")]
        public int? MaDanhMuc { get; set; }

        [StringLength(100, ErrorMessage = "Thương hiệu không được vượt quá 100 ký tự")]
        public string? ThuongHieu { get; set; }

        public string? MoTa { get; set; }

        // Mặc định là true (Đang bán)
        public bool TrangThai { get; set; } = true;
    }
}
