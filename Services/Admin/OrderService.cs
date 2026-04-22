using Microsoft.EntityFrameworkCore;
using DUANCUAHANGAPPLE.Models;
using DUANCUAHANGAPPLE.DTOs;
using DUANCUAHANGAPPLE.Data;

namespace DUANCUAHANGAPPLE.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;
        
        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ThanhToan>> GetAllOrdersAsync()
        {
            return await _context.ThanhToans
                .Include(o => o.User)
                .ToListAsync();
        }

        public async Task<ThanhToan> GetOrderByIdAsync(int id)
        {
            return await _context.ThanhToans
                .Include(o => o.User)
                .Include(o => o.ChiTietThanhToans)
                    .ThenInclude(oi => oi.BienTheSanPham)
                        .ThenInclude(bt => bt.SanPham)
                .Include(o => o.ChiTietThanhToans)
                    .ThenInclude(oi => oi.BienTheSanPham)
                        .ThenInclude(bt => bt.HinhAnhs)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<ThanhToan> CreateOrderAsync(CreateOrderDTO dto)
        {
            var order = new ThanhToan
            {
                SoDienThoai = dto.PhoneNumber,
                DiaChiChiTiet = dto.ShippingAddress,
                UserId = dto.UserId ?? 1, 
                NgayThanhToan = DateTime.Now,
                TrangThai = "Chờ xác nhận",
                TongTien = dto.Items.Sum(i => i.Quantity * i.UnitPrice),
                ChiTietThanhToans = dto.Items.Select(i => new ChiTietThanhToan
                {
                    BienTheId = i.BienTheId,
                    SoLuong = i.Quantity,
                    DonGia = i.UnitPrice,
                    TenSanPham = ""
                }).ToList(),
                MaDonHang = "DH" + DateTime.Now.Ticks,
                TinhThanh = "",
                QuanHuyen = "",
                PhuongXa = "",
                HoTen = "",
                Email = "",
                PhuongThucThanhToan = "COD"
            };
            _context.ThanhToans.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<bool> UpdateOrderAsync(ThanhToan order)
        {
            var existing = await _context.ThanhToans.FindAsync(order.Id);
            if (existing == null) return false;

            existing.TrangThai = order.TrangThai;
            existing.SoDienThoai = order.SoDienThoai;
            existing.DiaChiChiTiet = order.DiaChiChiTiet;
            
            _context.ThanhToans.Update(existing);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteOrderAsync(int id)
        {
            var order = await _context.ThanhToans
                .Include(o => o.ChiTietThanhToans)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null) return false;

            if (order.ChiTietThanhToans != null && order.ChiTietThanhToans.Any())
            {
                _context.ChiTietThanhToans.RemoveRange(order.ChiTietThanhToans); 
            }
            
            _context.ThanhToans.Remove(order);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> TotalOrder()
        {
            return await _context.ThanhToans.CountAsync();
        }

        public async Task<int> TotalPrice()
        {
            var total = await _context.ThanhToans
                .Where(o => o.TrangThai == "Hoàn tất" || o.TrangThai == "Đã giao hàng")
                .SumAsync(o => o.TongTien);

             return (int)total;
        }

        public async Task<IEnumerable<ThanhToan>> GetProductsBestSaleAsync()
        {
            // Logic tạm thời để lấy danh sách đơn hàng có sản phẩm bán chạy
            return await _context.ThanhToans
                .Include(o => o.User)
                .OrderByDescending(o => o.TongTien)
                .Take(5)
                .ToListAsync();
        }

        public async Task<bool> UpdateOrderItemAsync(int orderId, int bienTheId, int quantity)
        {
            var orderItem = await _context.ChiTietThanhToans.FirstOrDefaultAsync(oi => oi.ThanhToanId == orderId && oi.BienTheId == bienTheId);
            if (orderItem == null) return false;

            orderItem.SoLuong = quantity;
            _context.ChiTietThanhToans.Update(orderItem);
            await _context.SaveChangesAsync();

            var order = await _context.ThanhToans.Include(o => o.ChiTietThanhToans).FirstOrDefaultAsync(o => o.Id == orderId);
            if (order != null)
            {
                order.TongTien = order.ChiTietThanhToans.Sum(oi => oi.SoLuong * oi.DonGia);
                _context.ThanhToans.Update(order);
                await _context.SaveChangesAsync();
            }
            return true;
        }

        public async Task<bool> DeleteOrderItemAsync(int orderId, int bienTheId)
        {
            var orderItem = await _context.ChiTietThanhToans.FirstOrDefaultAsync(oi => oi.ThanhToanId == orderId && oi.BienTheId == bienTheId);
            if (orderItem == null) return false;

            _context.ChiTietThanhToans.Remove(orderItem);
            await _context.SaveChangesAsync();

            var order = await _context.ThanhToans.Include(o => o.ChiTietThanhToans).FirstOrDefaultAsync(o => o.Id == orderId);
            if (order != null)
            {
                order.TongTien = order.ChiTietThanhToans.Sum(oi => oi.SoLuong * oi.DonGia);
                _context.ThanhToans.Update(order);
                await _context.SaveChangesAsync();
            }
            return true;
        }

        public async Task<bool> AddOrderItemAsync(int orderId, CreateOrderItemDTO dto)
        {
            var order = await _context.ThanhToans.Include(o => o.ChiTietThanhToans).FirstOrDefaultAsync(o => o.Id == orderId);
            if (order == null) return false;

            var existingItem = order.ChiTietThanhToans.FirstOrDefault(oi => oi.BienTheId == dto.BienTheId);
            if (existingItem != null)
            {
                existingItem.SoLuong += dto.Quantity;
                _context.ChiTietThanhToans.Update(existingItem);
            }
            else
            {
                var newItem = new ChiTietThanhToan
                {
                    ThanhToanId = orderId,
                    BienTheId = dto.BienTheId,
                    SoLuong = dto.Quantity,
                    DonGia = dto.UnitPrice,
                    TenSanPham = ""
                };
                _context.ChiTietThanhToans.Add(newItem);
            }
            await _context.SaveChangesAsync();

            order.TongTien = order.ChiTietThanhToans.Sum(oi => oi.SoLuong * oi.DonGia);
            _context.ThanhToans.Update(order);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
