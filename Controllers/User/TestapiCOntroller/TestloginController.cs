using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DUANCUAHANGAPPLE.Data;
using DUANCUAHANGAPPLE.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace DUANCUAHANGAPPLE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestloginController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TestloginController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            // TC03 - Thiếu thông tin
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(new { message = "Thiếu thông tin" });
            }

            // TC04 - Email sai format
            if (!new System.ComponentModel.DataAnnotations.EmailAddressAttribute().IsValid(request.Email))
            {
                return BadRequest(new { message = "Email không hợp lệ" });
            }

            // TC06 - Password quá ngắn
            if (request.Password.Length <= 2)
            {
                return BadRequest(new { message = "Mật khẩu không hợp lệ" });
            }
            // Tìm user theo email trước (tách riêng ra)
            var user = _context.Users.FirstOrDefault(u => u.Email == request.Email);

            // TC05 - Không tồn tại
            if (user == null)
            {
                return NotFound(new { message = "Tài khoản không tồn tại" });
            }

            // TC07 - Account bị khóa (bạn phải có field IsLocked)
            if (user.IsLocked) // nhớ thêm field này trong DB
            {
                return StatusCode(403, new { message = "Tài khoản đã bị khóa" });
            }

            // TC02 - Sai password
            if (user.Password != request.Password)
            {
                return Unauthorized(new { message = "Sai email hoặc mật khẩu" });
            }

            

            // TC01 - Thành công
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("UserId", user.Id.ToString())
            };

            var key = "day_la_secret_key_123456";
            var keyBytes = Encoding.UTF8.GetBytes(key);

            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(keyBytes),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new
            {
                message = "Đăng nhập thành công",
                token = tokenString,
                user = new
                {
                    id = user.Id,
                    email = user.Email,
                    fullName = user.FullName,
                    phone = user.Phone,
                    address = user.Address,
                    isProfileCompleted = user.IsProfileCompleted
                }
            });
        }
    }
}