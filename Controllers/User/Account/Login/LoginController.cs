using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DUANCUAHANGAPPLE.Data;
using DUANCUAHANGAPPLE.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace DUANCUAHANGAPPLE.Controllers
{
    public class LoginController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly DUANCUAHANGAPPLE.Services.OtpService _otpService;
        private readonly DUANCUAHANGAPPLE.Services.SendEmailService _emailService;

        public LoginController(ApplicationDbContext context, 
            DUANCUAHANGAPPLE.Services.OtpService otpService,
            DUANCUAHANGAPPLE.Services.SendEmailService emailService)
        {
            _context = context;
            _otpService = otpService;
            _emailService = emailService;
        }
        // ================== LOGIN ==================
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            if(string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                return BadRequest("Thiếu thông tin");
            }
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            
            if (user == null)
            {
                return Unauthorized("Sai email hoặc mật khẩu");
            }

            bool isPasswordCorrect = false;

            // Kiểm tra xem mật khẩu trong DB đã được băm chưa (BCrypt hash bắt đầu bằng $2)
            if (user.Password != null && user.Password.StartsWith("$2"))
            {
                isPasswordCorrect = BCrypt.Net.BCrypt.Verify(password, user.Password);
            }
            else
            {
                // Nếu chưa băm (mật khẩu cũ), so sánh trực tiếp
                if (user.Password == password)
                {
                    isPasswordCorrect = true;
                    // Tự động băm lại mật khẩu và lưu vào DB cho lần sau
                    user.Password = BCrypt.Net.BCrypt.HashPassword(password);
                    _context.SaveChanges();
                }
            }

            if (!isPasswordCorrect)
            {
                return Unauthorized("Sai email hoặc mật khẩu");
            }
            
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("UserId", user.Id.ToString()),
                new Claim("FullName", user.FullName ?? "Tài khoản"),
                new Claim(ClaimTypes.Role, user.Role ?? "Customer") // Thêm dòng này để phân quyền
            };
            
            var identity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity),
                new AuthenticationProperties
                {
                    IsPersistent = false, // Cookie sẽ biến mất khi      đóng trình duyệt
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30) // Hết hạn sau 30 phút không hoạt động
                });

            // Rẽ nhánh giao diện dựa trên Role
            if (user.Role == "Admin")
            {
                return RedirectToAction("AdminIndex", "Home", new { area = "Admin" });
            }

            // 🔥 kiểm tra đăng nhập lần đầu cho User thường
            if (!user.IsProfileCompleted)
            {
                return RedirectToAction("CompleteProfile", "Profile");
            }

            return RedirectToAction("Personal", "Profile");
        }
        // ================== EXTERNAL LOGIN ==================
        [HttpGet]
        public IActionResult ExternalLogin(string provider)
        {
            var properties = new AuthenticationProperties 
            { 
                RedirectUri = Url.Action("ExternalCallback", new { provider = provider }) 
            };
            return Challenge(properties, provider);
        }

        [HttpGet]
        public async Task<IActionResult> ExternalCallback(string provider)
        {
            var result = await HttpContext.AuthenticateAsync("ExternalCookies");
            // result = {
            //     Succeeded: true,
            //     Principal: { email, name, id },
            //     Properties: { ... },
            //     Failure: null
            // }
            if (!result.Succeeded)
            {
                return RedirectToAction("Login", "Home");
            }
            //trả thông tin tài khoản vè object và chỉ lấy mối principal 
            var claimsPrincipal = result.Principal;
            var providerId = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
            var email = claimsPrincipal.FindFirstValue(ClaimTypes.Email);
            var name = claimsPrincipal.FindFirstValue(ClaimTypes.Name);
            var accessToken = result.Properties.GetTokenValue("access_token");
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(providerId))
            {
                TempData["ErrorMessage"] = $"Không lấy được thông tin từ {provider}.";
                return RedirectToAction("Login", "Home");
            }

            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null)
            {
                user = new User
                {
                    Email = email,
                    FullName = name ?? $"Người dùng {provider}",
                    Password = "", 
                    Provider = provider,
                    ProviderId = providerId,
                    IsProfileCompleted = false,
                    PasswordLastUpdated = DateTime.Now
                };
                _context.Users.Add(user);
                _context.SaveChanges();
            }
            else
            {
                // Cập nhật ProviderId nếu user đăng nhập bằng email đã có trong hệ thống
                if (user.ProviderId != providerId || user.Provider != provider)
                {
                    user.Provider = provider;
                    user.ProviderId = providerId;
                    _context.SaveChanges();
                }
            }

            // Ghi đè cookie đăng nhập với thông tin User của db (bao gồm UserId và Role)
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("UserId", user.Id.ToString()),
                new Claim("FullName", user.FullName ?? "Tài khoản"),
                new Claim(ClaimTypes.Role, user.Role ?? "Customer") // Thêm quyền cho Google Login
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), new AuthenticationProperties { IsPersistent = false });

            // Hủy scheme tạm để bảo mật
            await HttpContext.SignOutAsync("ExternalCookies");

            // Rẽ nhánh Redirect
            if (user.Role == "Admin")
            {
                return RedirectToAction("AdminIndex", "Home", new { area = "Admin" });
            }

            if (!user.IsProfileCompleted)
            {
                return RedirectToAction("CompleteProfile", "Profile");
            }

            return RedirectToAction("Personal", "Profile");
        }
        // ================== LOGOUT ==================
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Quay về trạng thái CHƯA đăng nhập
            return RedirectToAction("Login", "Home");
        }
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View("~/Views/Home/User/Account/ForgotPassword.cshtml");
        }
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                TempData["ErrorMessage"] = "Vui lòng nhập email.";
                return RedirectToAction("ForgotPassword", "Login");
            }
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null)
            {
                TempData["ErrorMessage"] = "Email không tồn tại.";
                return RedirectToAction("ForgotPassword", "Login");
            }
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
            TempData["SuccessMessage"] = "Mã OTP đã được gửi!";
            return RedirectToAction("VerifyOtpPage", new { email });
        }
        [HttpGet]
        public IActionResult VerifyOtpPage(string email)
        {
            ViewBag.Email = email;
            return View("~/Views/Home/User/Account/VerifyOtpPage.cshtml");
        }
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
            // 🔥 Tạo user thật
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null)
            {
                user = new User
                {
                    FullName = user.FullName,
                    Email = user.Email,
                    Password = user.Password,
                    IsProfileCompleted = false,
                    PasswordLastUpdated = DateTime.Now
                };

                _context.Users.Add(user);
            }
            _context.EmailOtps.Remove(record);

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Đăng ký thành công!";
            return RedirectToAction("RegistrationSuccess", new { name = user.FullName });
        }
        // 1. Hàm GET: Dùng để HIỂN THỊ trang nhập mật khẩu mới
        [HttpGet]
        public IActionResult ResetPasswordNew(string email)
        {
            if (string.IsNullOrEmpty(email)) return RedirectToAction("Login", "Login");
            ViewBag.Email = email;
            return View("~/Views/Home/User/Account/ResetPasswordNew.cshtml");
        }
        [HttpPost]
        public async Task<IActionResult> ResetPasswordNew(string email, string newPassword, string confirmPassword)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(confirmPassword))
            {   
                TempData["ErrorMessage"] = "Vui lòng nhập đầy đủ thông tin.";
                ViewBag.Email = email;
                return View("~/Views/Home/User/Account/ResetPasswordNew.cshtml");
            }
            if (newPassword != confirmPassword)
            {
                TempData["ErrorMessage"] = "Mật khẩu xác nhận không khớp.";
                ViewBag.Email = email;
                return View("~/Views/Home/User/Account/ResetPasswordNew.cshtml");
            }
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null)
            {
                TempData["ErrorMessage"] = "Email không tồn tại.";
                return RedirectToAction("Login", "Home");
            }
            user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
            user.PasswordLastUpdated = DateTime.Now;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Mật khẩu đã được thay đổi! Mời bạn đăng nhập.";
            return RedirectToAction("Login", "Home");
        }
    } 
}