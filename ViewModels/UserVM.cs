using System.ComponentModel.DataAnnotations;

namespace DUANCUAHANGAPPLE.ViewModels
{
    public class UserVM
    {
        [Required(ErrorMessage = "Vui lòng nhập họ tên hoặc tên đăng nhập!")]
        [StringLength(100, ErrorMessage = "Tên không được vượt quá 100 ký tự")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu!")]
        [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự")]
        public string Password { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Định dạng Email không hợp lệ!")]
        [StringLength(150)]
        public string? Email { get; set; }

        [Phone(ErrorMessage = "Số điện thoại không hợp lệ!")]
        public string? PhoneNumber { get; set; }
    }
}
