using Microsoft.AspNetCore.Mvc;
using DUANCUAHANGAPPLE.Data;
using Microsoft.EntityFrameworkCore;

namespace DUANCUAHANGAPPLE.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        // DANH MỤC
        [HttpGet]
        public IActionResult Category(int id)
        {
            var list = _context.SanPhams
                .Where(p => p.MaDanhMuc == id)
                .Include(p => p.BienThes)
                .ThenInclude(bt => bt.HinhAnhs)
                .ToList();

            if (!list.Any())
                return NotFound();

            if (id == 1)
            {
                return View("~/Views/Home/User/Iphonedanhmuc.cshtml", list);
            }
            else if (id == 2)
            {
                return View("~/Views/Home/User/Laptopdanhmuc.cshtml", list);
            }

            return NotFound();
        }

        // FLASH SALE (CHUNG CHO CẢ IPHONE + LAPTOP)
        [HttpGet]
        public IActionResult GetFlashSale()
        {
            var list = _context.BienTheSanPhams
                .Include(bt => bt.SanPham)
                .Include(bt => bt.HinhAnhs)
                // .Where(bt => bt.GiamGia > 0)
                .Select(bt => new
                {
                    id = bt.BienTheId,
                    ten = bt.SanPham.TenSanPham,
                    gia = bt.Gia,
                    giamgia = bt.GiamGia,
                    soluong = bt.SoLuongTon,

                    hinhAnh = bt.HinhAnhs
                        .Select(img => img.UrlHinhAnh)
                        .FirstOrDefault()
                })
                .ToList();

            return Json(list);
        }

        // CHI TIẾT SẢN PHẨM
        [HttpGet]
        public IActionResult Chitietsanpham(int id)
        {
            var product = _context.SanPhams
                .Include(p => p.BienThes)
                .ThenInclude(bt => bt.HinhAnhs)
                .Include(p => p.ThongSo)
                .FirstOrDefault(p => p.SanPhamId == id);

            if (product == null)
                return NotFound();

            if (product.MaDanhMuc == 1)
            {
                return View("~/Views/Home/User/Iphonechitietsanpham.cshtml", product);
            }
            else if (product.MaDanhMuc == 2)
            {
                return View("~/Views/Home/User/Chitietsanpham.cshtml", product);
            }

            return NotFound();
        }

        // FLASH SALE LAPTOP (GIỮ HÀM THEO YÊU CẦU)
        [HttpGet]
        public IActionResult GetFlashSaleLaptop()
        {
            var laptops = _context.BienTheSanPhams
                .Include(bt => bt.SanPham)
                .Where(bt => bt.SanPham.MaDanhMuc == 2)
                .Select(bt => new
                {
                    id = bt.BienTheId,
                    tenLaptop = bt.SanPham.TenSanPham,
                    gia = bt.Gia,
                    giaKhuyenMai = bt.GiaCu,
                    soLuongTon = bt.SoLuongTon
                })
                .ToList();

            return Json(laptops);
        }

        // CHI TIẾT LAPTOP (GIỮ HÀM)
        [HttpGet]
        public IActionResult Chitiet(int id)
        {
            var laptop = _context.SanPhams
                .Include(p => p.BienThes)
                .ThenInclude(bt => bt.HinhAnhs)
                .Include(p => p.ThongSo)
                .FirstOrDefault(p => p.SanPhamId == id && p.MaDanhMuc == 2);

            if (laptop == null)
                return NotFound();

            return View("~/Views/Home/User/Chitietsanpham.cshtml", laptop);
        }
        //Gio Hang
        [HttpGet]
        public IActionResult GioHang(){
            return View("~/Views/Home/User/GioHang.cshtml");
        }
    }
}