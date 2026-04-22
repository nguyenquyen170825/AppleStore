using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DUANCUAHANGAPPLE.Data;
using DUANCUAHANGAPPLE.Models;

namespace DUANCUAHANGAPPLE.Controllers
{
    public class ProfileController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly DUANCUAHANGAPPLE.Services.OtpService _otpService;
        private readonly DUANCUAHANGAPPLE.Services.SendEmailService _emailService;

        public ProfileController(ApplicationDbContext context, 
            DUANCUAHANGAPPLE.Services.OtpService otpService,
            DUANCUAHANGAPPLE.Services.SendEmailService emailService)
        {
            _context = context;
            _otpService = otpService;
            _emailService = emailService;
        }
        // ============ BỔ SUNG THÔNG TIN LẦN ĐẦU ============
        [Authorize]
        [HttpGet]
        public IActionResult CompleteProfile()
        {
            var userId = int.Parse(User.FindFirst("UserId").Value);
            var user = _context.Users.Find(userId);

            return View("~/Views/Home/User/Account/Personal.cshtml", user);
        }

        [Authorize]
        [HttpPost]
        public IActionResult CompleteProfile(User model)
        {
            var userId = int.Parse(User.FindFirst("UserId").Value);
            var user = _context.Users.Find(userId);

            if (user == null)
            {
                return NotFound();
            }

            if (string.IsNullOrEmpty(model.FullName) || string.IsNullOrEmpty(model.Phone))
            {
                TempData["ErrorMessage"] = "Vui lòng nhập đầy đủ thông tin.";
                return RedirectToAction("Personal");
            }

            user.FullName = model.FullName;
            user.Phone = model.Phone;
            user.Address = model.Address ?? "Chưa cập nhật";
            user.NgaySinh = model.NgaySinh;
            user.Sex = model.Sex;

            _context.SaveChanges();

            TempData["SuccessMessage"] = "Cập nhật thành công!";
            return RedirectToAction("Personal");
        }
        
        // ================== TRANG CÁ NHÂN ==================
        [Authorize]
        [HttpGet]
        public IActionResult Personal()
        {
            var userId = int.Parse(User.FindFirst("UserId").Value);

            var user = _context.Users.Find(userId);

            if (user == null)
            {
                return RedirectToAction("Login", "Login");
            }

            return View("~/Views/Home/User/Account/Personal.cshtml", user);
        }

        [Authorize]
        [HttpPost]
        public IActionResult ResetPassword(string oldPassword, string newPassword, string confirmPassword, string otp)
        {
            var userId = int.Parse(User.FindFirst("UserId").Value);
            var user = _context.Users.Find(userId); 
            if (user != null)
            {
                var record = _context.EmailOtps
                .FirstOrDefault(x => x.Email == user.Email && x.Code == otp);

                if (record == null)
                {
                    TempData["ErrorMessage"] = "OTP sai";
                    return RedirectToAction("Personal");
                }

                if (record.ExpireAt < DateTime.Now)
                {
                    TempData["ErrorMessage"] = "OTP hết hạn";
                    return RedirectToAction("Personal");
                }
                if (user.Password != oldPassword)
                {
                    TempData["ErrorMessage"] = "Mật khẩu cũ không chính xác.";
                    return RedirectToAction("Personal");
                }

                if (newPassword != confirmPassword)
                {
                    TempData["ErrorMessage"] = "Mật khẩu xác nhận không khớp.";
                    return RedirectToAction("Personal");
                }

                user.Password = newPassword;
                user.PasswordLastUpdated = DateTime.Now;
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Đổi mật khẩu thành công!";
            }
            return RedirectToAction("Personal");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SendOtp(string email)
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

            return Ok(new Dictionary<string, string> { { "message", "Đã gửi mã OTP !" } });
        }
    }
}
