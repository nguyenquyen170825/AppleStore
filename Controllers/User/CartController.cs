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
        [HttpPost]
        public IActionResult AddGioHang(int bientheid)
        {
            try
            {
                int userid = 1;

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

        public IActionResult LoadCart()
        {
            int userid = 1;

            var cart = _context.GioHangs
                .Include(x => x.BienTheSanPham)
                .ThenInclude(bt => bt.SanPham)
                .ToList();

            return PartialView("_CartPartial", cart);
        }
    }
}