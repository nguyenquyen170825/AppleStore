using Microsoft.AspNetCore.Mvc;
using DUANCUAHANGAPPLE.Data;

namespace DUANCUAHANGAPPLE.Controllers
{
    public class SanPhamController : Controller
        {
            private readonly ApplicationDbContext _context;

            public SanPhamController(ApplicationDbContext context)
            {
                _context = context;
            }
            [HttpGet]
            public IActionResult GetFlashSale()
            {
                var list=_context.SanPham.Select(
                    sp=>new
                    {
                        ma=sp.Ma,
                        ten=sp.Ten,
                        gia=sp.Gia,
                        giamgia=sp.GiamGia,
                        hinhAnh=sp.HinhAnh,
                        soluong=sp.Soluong
                    }
                ).ToList();
                return Json(list);
            }
        }
}