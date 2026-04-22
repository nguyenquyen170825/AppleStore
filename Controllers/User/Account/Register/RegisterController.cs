using Microsoft.AspNetCore.Mvc;
using DUANCUAHANGAPPLE.Data;
using DUANCUAHANGAPPLE.Models;

namespace DUANCUAHANGAPPLE.Controllers
{
    public class RegisterController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly DUANCUAHANGAPPLE.Services.OtpService _otpService;
        private readonly DUANCUAHANGAPPLE.Services.SendEmailService _emailService;

        public RegisterController(ApplicationDbContext context, 
            DUANCUAHANGAPPLE.Services.OtpService otpService,
            DUANCUAHANGAPPLE.Services.SendEmailService emailService)
        {
            _context = context;
            _otpService = otpService;
            _emailService = emailService;
        }
        // ================== REGISTER ==================
        [HttpPost]
        public async Task<IActionResult> Register(string fullname, string email, string password, string confirmPassword)
        {
            if (string.IsNullOrEmpty(fullname) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
            {
                TempData["ErrorMessage"] = "Vui lòng nhập đầy đủ thông tin.";
                return RedirectToAction("Register", "Home");
            }

            if (password != confirmPassword)
            {
                TempData["ErrorMessage"] = "Mật khẩu xác nhận không khớp.";
                return RedirectToAction("Register", "Home");
            }

            if (_context.Users.Any(u => u.Email == email))
            {
                TempData["ErrorMessage"] = "Email đã tồn tại.";
                return RedirectToAction("Register", "Home");
            }

            // 🔥 Tạo OTP
            var otp = _otpService.GenerateOtp();

            // 🔥 Xóa OTP cũ nếu có
            var oldOtp = _context.EmailOtps.FirstOrDefault(x => x.Email == email);
            if (oldOtp != null) _context.EmailOtps.Remove(oldOtp);

            // 🔥 Lưu OTP
            _context.EmailOtps.Add(new EmailOtp
            {      
                Email = email,
                Code = otp,
                ExpireAt = DateTime.Now.AddMinutes(5)
            });

            // 🔥 Lưu user tạm
            var oldPending = _context.PendingUsers.FirstOrDefault(x => x.Email == email);
            if (oldPending != null) _context.PendingUsers.Remove(oldPending);

            _context.PendingUsers.Add(new PendingUser
            {
                FullName = fullname,
                Email = email,
                Password = password
            });

            await _context.SaveChangesAsync();

            // 🔥 Gửi mail
            await _emailService.SendOtpEmail(email, otp);

            // 👉 chuyển sang trang nhập OTP
            return RedirectToAction("VerifyOtpPage", new { email = email });
        }

        [HttpPost]
        public async Task<IActionResult> VerifyOtp(string email, string otp)
        {
            var record = _context.EmailOtps
                .FirstOrDefault(x => x.Email == email && x.Code == otp);

            if (record == null)
            {
                TempData["ErrorMessage"] = "OTP sai";
                return RedirectToAction("VerifyOtpPage", new { email });
            }

            if (record.ExpireAt < DateTime.Now)
            {
                TempData["ErrorMessage"] = "OTP hết hạn";
                return RedirectToAction("VerifyOtpPage", new { email });
            }
            //luồng xủ lý User quên mật khẩu.
            if (_context.Users.FirstOrDefault(u => u.Email == email) != null){
                _context.EmailOtps.Remove(record);
                await _context.SaveChangesAsync();
                return RedirectToAction("ResetPasswordNew", "Login", new { email = email });
            }
            //luồng xủ lý User đăng ký.
            var pending = _context.PendingUsers.FirstOrDefault(x => x.Email == email);

            if (pending == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy user";
                return RedirectToAction("Register", "Home");
            }

            // 🔥 Tạo user thật
            var user = new User
            {
                FullName = pending.FullName,
                Email = pending.Email,
                Password = pending.Password,
                IsProfileCompleted = false,
                PasswordLastUpdated = DateTime.Now,
                Role = "Customer"
            };

            _context.Users.Add(user);

            // 🔥 Xóa dữ liệu tạm
            _context.PendingUsers.Remove(pending);
            _context.EmailOtps.Remove(record);

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Đăng ký thành công!";
            return RedirectToAction("RegistrationSuccess", new { name = user.FullName });
        }

        [HttpGet]
        public IActionResult RegistrationSuccess(string name)
        {
            ViewBag.FullName = name;
            return View("~/Views/Home/User/Account/RegistrationSuccess.cshtml");
        }

        [HttpGet]
        public IActionResult VerifyOtpPage(string email)
        {
            ViewBag.Email = email;
            return View("~/Views/Home/User/Account/VerifyOtpPage.cshtml");
        }

        [HttpGet]
        public async Task<IActionResult> ResendOtp(string email)
        {
            var otp = _otpService.GenerateOtp();
            var oldOtp = _context.EmailOtps.FirstOrDefault(x => x.Email == email);
            if (oldOtp != null) _context.EmailOtps.Remove(oldOtp);

            _context.EmailOtps.Add(new EmailOtp
            {
                Email = email,
                Code = otp,
                ExpireAt = DateTime.Now.AddMinutes(5)
            });

            await _context.SaveChangesAsync();
            await _emailService.SendOtpEmail(email, otp);

            TempData["SuccessMessage"] = "Mã OTP mới đã được gửi!";
            return RedirectToAction("VerifyOtpPage", new { email });
        }
    }
}
