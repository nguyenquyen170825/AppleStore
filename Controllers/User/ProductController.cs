using Microsoft.AspNetCore.Mvc;
using DUANCUAHANGAPPLE.Data;

namespace DUANCUAHANGAPPLE.Controllers
{
    public class ProductController : Controller
        {
            private readonly ApplicationDbContext _context;

            public ProductController(ApplicationDbContext context)
            {
                _context = context;
            }
            [HttpGet]
            public IActionResult()
            {
                var list=_context.sanpham.Select(
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