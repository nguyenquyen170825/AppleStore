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
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        
        // ================== LOGIN ==================
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var user = _context.Users
                .FirstOrDefault(u => u.Email == email && u.Password == password);

            if (user == null)
            {
                return Unauthorized("Sai email hoặc mật khẩu");
            }
            
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim("UserId", user.Id.ToString())
            };

            var identity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity));

            // 🔥 kiểm tra đăng nhập lần đầu
            if (!user.IsProfileCompleted)
            {
                return RedirectToAction("CompleteProfile");
            }

            return RedirectToAction("Personal");
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
        public IActionResult CompleteProfile(string fullName, string phone, string address)
        {
            var userId = int.Parse(User.FindFirst("UserId").Value);

            var user = _context.Users.Find(userId);

            user.FullName = fullName;
            user.Phone = phone;
            user.Address = address;
            user.IsProfileCompleted = true;

            _context.SaveChanges();

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
                return RedirectToAction("Login", "Home");
            }

            return View("~/Views/Home/User/Account/Personal.cshtml", user);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Quay về trạng thái CHƯA đăng nhập
            return RedirectToAction("Login", "Home");
            // hoặc: return RedirectToAction("Index", "Home");
        }
    }
}