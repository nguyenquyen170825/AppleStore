using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace DUANCUAHANGAPPLE.ViewModels
{
    public class BienTheVM
    {
        public int BienTheId { get; set; } // Dùng khi muốn cập nhật biến thể

        [Required(ErrorMessage = "Vui lòng chọn sản phẩm")]
        public int SanPhamId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập màu sắc")]
        [StringLength(50, ErrorMessage = "Màu sắc không được vượt quá 50 ký tự")]
        public string Mau { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập dung lượng")]
        [StringLength(50, ErrorMessage = "Dung lượng không được vượt quá 50 ký tự")]
        public string DungLuong { get; set; } = string.Empty;

        [StringLength(50, ErrorMessage = "RAM không được vượt quá 50 ký tự")]
        public string? Ram { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập giá bán")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá bán không hợp lệ")]
        public decimal Gia { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Giá cũ không hợp lệ")]
        public decimal? GiaCu { get; set; }

        [Range(0, 100, ErrorMessage = "Mức giảm giá không hợp lệ")]
        public decimal? GiamGia { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số lượng tồn")]
        [Range(0, int.MaxValue, ErrorMessage = "Số lượng tồn không hợp lệ")]
        public int SoLuongTon { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập SKU")]
        public string Sku { get; set; } = string.Empty;

        public bool TrangThai { get; set; } = true;

        // Dùng để nhận file upload từ form
        public IFormFile? HinhAnhFile { get; set; }
    }
}
