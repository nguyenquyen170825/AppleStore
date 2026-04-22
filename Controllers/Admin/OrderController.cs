using DUANCUAHANGAPPLE.Models;
using DUANCUAHANGAPPLE.Services;
using Microsoft.AspNetCore.Mvc;
using DUANCUAHANGAPPLE.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace DUANCUAHANGAPPLE.Controllers.Admin
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("Admin/Order/Index")]
        public IActionResult Index()
        {
            return View("~/Views/Home/Admin/Order/Index.cshtml");
        }

        [HttpGet("Admin/Order/Detail/{id}")]
        public IActionResult Detail(int id)
        {
            ViewBag.OrderId = id;
            return View("~/Views/Home/Admin/Order/Detail.cshtml");
        }

        [HttpGet("Admin/Order/Edit/{id}")]
        public IActionResult Edit(int id)
        {
            ViewBag.OrderId = id;
            return View("~/Views/Home/Admin/Order/Edit.cshtml");
        }

        [HttpGet("Admin/Order/Create")]
        public IActionResult Create()
        {
            return View("~/Views/Home/Admin/Order/Create.cshtml");
        }

        [HttpGet("Admin/Order/EditOrderItem/{orderId}/{itemId}")]
        public IActionResult EditOrderItem(int orderId, int itemId)
        {
            ViewBag.OrderId = orderId;
            ViewBag.OrderItemId = itemId;
            return View("~/Views/Home/Admin/Order/EditOrderItem.cshtml");
        }

        // API
        [HttpGet("/api/order")]
        public async Task<ActionResult<IEnumerable<ThanhToan>>> GetAll()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders);
        }

        [HttpGet("/api/order/detail/{id}")]
        public async Task<ActionResult<OderItemResponseDTO>> GetOrderDetailById(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null) return NotFound(new { message = $"Không tìm thấy đơn hàng ID {id}" });

            var responseDto = new OderItemResponseDTO
            {
                id = order.Id,
                TenKhachHang = order.User?.FullName ?? order.HoTen ?? "Khách vãng lai", 
                SDT = order.SoDienThoai,
                DiaChiGiaoHang = order.DiaChiChiTiet,
                NgayDat = order.NgayThanhToan,
                TongTien = order.TongTien,
                Status = order.TrangThai ?? "Chờ xác nhận",
                Items = order.ChiTietThanhToans.Select(item => new OrderDetailDTO
                {
                    BienTheID = item.BienTheId ?? 0, 
                    TenSanPham = item.TenSanPham ?? item.BienTheSanPham?.SanPham?.TenSanPham,
                    Mau = item.BienTheSanPham?.Mau,
                    DungLuong = item.BienTheSanPham?.DungLuong,
                    ImageUrl = item.BienTheSanPham?.HinhAnhs?.FirstOrDefault()?.UrlHinhAnh,
                    Quantity = item.SoLuong,
                    UnitPrice = item.DonGia
                }).ToList()
            };

            return Ok(responseDto);
        }

        [HttpPut("/api/order/{id}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] ThanhToan order)
        {
            order.Id = id;
            var success = await _orderService.UpdateOrderAsync(order);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpDelete("/api/order/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _orderService.DeleteOrderAsync(id);
            if (!success) return NotFound(new { message = $"Không tìm thấy đơn hàng ID {id}" });
            return NoContent();
        }

        [HttpPut("/api/order/{orderId}/item/{bienTheId}")]
        public async Task<IActionResult> UpdateOrderItem(int orderId, int bienTheId, [FromBody] OrderItemUpdateDTO dto)
        {
            var success = await _orderService.UpdateOrderItemAsync(orderId, bienTheId, dto.Quantity);
            if (!success) return NotFound(new { message = "Không tìm thấy sản phẩm trong đơn hàng" });
            return NoContent();
        }

        [HttpDelete("/api/order/{orderId}/item/{bienTheId}")]
        public async Task<IActionResult> DeleteOrderItem(int orderId, int bienTheId)
        {
            var success = await _orderService.DeleteOrderItemAsync(orderId, bienTheId);
            if (!success) return NotFound(new { message = "Không tìm thấy sản phẩm trong đơn hàng" });
            return NoContent();
        }

        [HttpPost("/api/order/{orderId}/item")]
        public async Task<IActionResult> AddOrderItem(int orderId, [FromBody] CreateOrderItemDTO dto)
        {
            var success = await _orderService.AddOrderItemAsync(orderId, dto);
            if (!success) return NotFound(new { message = "Không tìm thấy đơn hàng" });
            return Ok(new { message = "Đã thêm sản phẩm thành công" });
        }
    }
}
