using Microsoft.AspNetCore.Mvc;
using DUANCUAHANGAPPLE.Data;
using Microsoft.EntityFrameworkCore;
using DUANCUAHANGAPPLE.Models;


namespace DUANCUAHANGAPPLE.Controllers{
    public class PayController : Controller{
        private readonly ApplicationDbContext _context;

        public PayController(ApplicationDbContext context)
        {

            _context = context;
        }
        
        [HttpGet]
        public IActionResult ThanhToan()
        {
            var json = HttpContext.Session.GetString("SelectedIds");
            if (string.IsNullOrEmpty(json))
            {
                return RedirectToAction("GioHang", "Cart");
            }
            var ids = System.Text.Json.JsonSerializer.Deserialize<List<int>>(json);
            
            // Lấy thông tin người dùng đang đăng nhập
            var userIdClaim = User.FindFirst("UserId");
            if (userIdClaim != null)
            {
                int userId = int.Parse(userIdClaim.Value);
                var user = _context.Users.FirstOrDefault(u => u.Id == userId);
                ViewBag.User = user; // Gửi thông tin người dùng sang View
            }

            var products = _context.GioHangs
                    .Include(x => x.BienTheSanPham)
                        .ThenInclude(x => x.SanPham)
                    .Include(x => x.BienTheSanPham)
                        .ThenInclude(x => x.HinhAnhs)
                    .Where(x => ids.Contains(x.BienTheId))
                    .ToList();

            var sum = products.Sum(x => x.BienTheSanPham.Gia * x.SoLuong);
            ViewBag.Sum = sum;

            // Lấy danh sách phiếu giảm giá của người dùng
            var userIdClaim2 = User.FindFirst("UserId");
            if (userIdClaim2 != null)
            {
                int uid = int.Parse(userIdClaim2.Value);
                var vouchers = _context.PhieuGiamGiaNguoiDungs
                                .Include(x => x.PhieuGiamGia)
                                .Where(x => x.UserId == uid && !x.DaSuDung && x.PhieuGiamGia.NgayKetThuc > DateTime.Now && x.PhieuGiamGia.TrangThai)
                                .ToList();
                ViewBag.Vouchers = vouchers;
            }
            else 
            {
                ViewBag.Vouchers = new List<PhieuGiamGiaNguoiDung>();
            }

            return View("~/Views/Home/User/thanhtoan.cshtml", products);
        }
        
        [HttpPost]
        public IActionResult ThanhToan([FromBody] List<int> ids)
        {
            if (ids == null || !ids.Any())
                return BadRequest();

            // lưu session nếu cần
            HttpContext.Session.SetString("SelectedIds", System.Text.Json.JsonSerializer.Serialize(ids));

            return Ok(); 
        }
    }
}