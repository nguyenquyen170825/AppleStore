using System.Diagnostics;
using DUANCUAHANGAPPLE.Models;
using Microsoft.AspNetCore.Mvc;
using DUANCUAHANGAPPLE.Services;
using Microsoft.AspNetCore.Authorization;

namespace DUANCUAHANGAPPLE.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;
        private readonly IUserService _userService;

        public HomeController(
            ILogger<HomeController> logger, 
            IOrderService orderService, 
            IProductService productService, 
            IUserService userService)
        {
            _logger = logger;
            _orderService = orderService;
            _productService = productService;
            _userService = userService;
        }

        //----------------USER-------------------
        public IActionResult Index()
        {
            return View("User/Index");
        }

        public IActionResult Privacy()
        {
            return View("User/Privacy");
        }

        public IActionResult Login()
        {
            return View("User/Account/Login");
        }

        public IActionResult Register()
        {
            return View("User/Account/Register");
        }

        //--------------ADMIN--------------------
        [Area("Admin")]
        [Authorize(Roles = "Admin")]
        [HttpGet("Admin/Home/Index")]
        public async Task<IActionResult> AdminIndex()
        {
            // 1. Tổng doanh thu
            ViewBag.DoanhThu = await _orderService.TotalPrice();

            // 2. Số đơn hàng theo trạng thái
            var allOrders = await _orderService.GetAllOrdersAsync();
            ViewBag.DonHangCho = allOrders.Count(o => o.TrangThai == "Chờ xác nhận");
            ViewBag.DaGiao = allOrders.Count(o => o.TrangThai == "Hoàn tất" || o.TrangThai == "Đã giao hàng");
            ViewBag.DangGiao = allOrders.Count(o => o.TrangThai == "Đang giao hàng");
            ViewBag.DaHuy = allOrders.Count(o => o.TrangThai == "Đã hủy");

            // 3. Số lượng hàng tồn kho
            ViewBag.HangTonKho = await _productService.TotalProduct();

            // 4. Số lượng người dùng
            ViewBag.SoNguoiDung = await _userService.TotalUser();

            // 5. Thống kê doanh thu theo tháng (Năm hiện tại)
            var currentYear = DateTime.Now.Year;
            var monthlyRevenue = new decimal[12];
            foreach (var order in allOrders.Where(o => o.TrangThai != "Đã hủy" && o.TrangThai != "Chờ xác nhận"))
            {
                if (order.NgayThanhToan.Year == currentYear)
                {
                    monthlyRevenue[order.NgayThanhToan.Month - 1] += order.TongTien;
                }
            }
            ViewBag.MonthlyRevenue = monthlyRevenue;

            // 6. Sản phẩm bán chạy
            var bestSellingProducts = await _orderService.GetProductsBestSaleAsync();
            ViewBag.BestSellingProducts = bestSellingProducts;

            return View("~/Views/Home/Admin/Home/Index.cshtml");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
