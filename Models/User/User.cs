using System.ComponentModel.DataAnnotations;

namespace DUANCUAHANGAPPLE.Models
{
    public enum Sex
    {
        Nam,
        Nu,
        Khac
    }

    public enum HangThanhVien
    {
        Dong,
        Bac,
        Vang,
        KimCuong
    }

    public class User
    {
        public int Id { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

        public string? FullName { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }

        public string? Provider { get; set; }
        public string? ProviderId { get; set; }

        public bool IsProfileCompleted { get; set; }
        public bool IsLocked { get; set; } = false;

        public DateTime? NgaySinh { get; set; }

        public Sex? Sex { get; set; }

        public HangThanhVien Hang { get; set; } = HangThanhVien.Dong;

        public decimal TongTienDaMua { get; set; } = 0;
        public DateTime? PasswordLastUpdated { get; set; }

        public List<ThanhToan> ThanhToans { get; set; }
        public ICollection<PhieuGiamGiaNguoiDung>? PhieuNguoiDungs { get; set; }
        public ICollection<EmailOtp>? EmailOtps { get; set; }

        // tính hạng khách hàng.
        public void CapNhatHang()
        {
            if (TongTienDaMua >= 20000000)
                Hang = HangThanhVien.KimCuong;
            else if (TongTienDaMua >= 10000000)
                Hang = HangThanhVien.Vang;
            else if (TongTienDaMua >= 5000000)
                Hang = HangThanhVien.Bac;
            else
                Hang = HangThanhVien.Dong;
        }

        public string Role { get; set; } = "Customer"; // Customer, Admin
         
    }
}