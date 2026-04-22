using Microsoft.AspNetCore.Mvc;
using DUANCUAHANGAPPLE.Data;
using Microsoft.EntityFrameworkCore;
using DUANCUAHANGAPPLE.Models;

namespace DUANCUAHANGAPPLE.Controllers{
    public class CartController : Controller{
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult GioHang(){
            int? userid = GetUserId();
            if (userid == null) return RedirectToAction("Login", "Home");

            var list = _context.GioHangs
                    .Where(x => x.MaNguoiDung == userid)
                    .Include(x => x.BienTheSanPham)
                        .ThenInclude(bt => bt.SanPham)
                    .Include(x => x.BienTheSanPham)
                        .ThenInclude(bt => bt.HinhAnhs)
                    .ToList();
            return View("~/Views/Home/User/GioHang.cshtml",list);
        }
        [HttpPost]
        public IActionResult AddGioHang(int bientheid)
        {
            try
            {
                var userIdClaim = User.FindFirst("UserId");
                if(userIdClaim == null)
                {
                    return Json(new { success = false, message = "Vui lòng đăng nhập để mua hàng!" });
                }
                int userid = int.Parse(userIdClaim.Value);

                var items = _context.GioHangs
                    .FirstOrDefault(x => x.MaNguoiDung == userid && x.BienTheId == bientheid);

                if (items != null)
                {
                    items.SoLuong += 1; 
                }
                else
                {
                    GioHang gh = new GioHang()
                    {
                        MaNguoiDung = userid,
                        BienTheId = bientheid,
                        SoLuong = 1,
                        NgayThem = DateTime.Now
                    };

                    _context.GioHangs.Add(gh);
                }

                _context.SaveChanges();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }
        [HttpGet]
        public IActionResult LoadCart()
        {
            int? userid = GetUserId();
            if (userid == null) return PartialView("Partial/MiniGioHang", new List<GioHang>());

            var cart = _context.GioHangs
                        .Where(x => x.MaNguoiDung == userid) 
                        .Include(x => x.BienTheSanPham)
                            .ThenInclude(bt => bt.SanPham)
                        .Include(x => x.BienTheSanPham)
                            .ThenInclude(bt => bt.HinhAnhs)
                        .ToList();

            return PartialView("Partial/MiniGioHang", cart);
        }
        [HttpPost]
        public IActionResult UpdateQuantity(int bientheid, int change)
        {
            int? userid = GetUserId();
            if (userid == null) return Unauthorized();

            var item = _context.GioHangs
                .FirstOrDefault(x => x.MaNguoiDung == userid && x.BienTheId == bientheid);

            if (item != null)
            {
                item.SoLuong += change;

                if (item.SoLuong <= 0)
                {
                    _context.GioHangs.Remove(item);
                }

                _context.SaveChanges();
            }

            return Ok();
        }
        public IActionResult Delete(int bientheid)
        {
            int? userid = GetUserId();
            if (userid == null) return Unauthorized();

            var item = _context.GioHangs
                .FirstOrDefault(x => x.MaNguoiDung == userid && x.BienTheId == bientheid);
            if(item!= null)
            {
                _context.GioHangs.Remove(item);
            }
            _context.SaveChanges();
            return Ok();
        }

        // API lấy số lượng sản phẩm trong giỏ hàng
        [HttpGet]
        public IActionResult GetCartCount()
        {
            int? userid = GetUserId();
            if (userid == null) return Json(new { count = 0 });

            int count = _context.GioHangs
                .Where(x => x.MaNguoiDung == userid)
                .Sum(x => x.SoLuong);

            return Json(new { count = count });
        }

        private int? GetUserId()
        {
            var userIdClaim = User.FindFirst("UserId");
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int id))
            {
                return id;
            }
            return null;
        }
    }
}