using DUANCUAHANGAPPLE.Data;
using Microsoft.EntityFrameworkCore;
using DUANCUAHANGAPPLE.Models;

namespace DUANCUAHANGAPPLE.Services
{
    public class DanhMucService : IDanhMucService
    {
        private readonly ApplicationDbContext _context;
        public DanhMucService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<DanhMuc>> GetAllAsync()
        {
            return await _context.DanhMucs.ToListAsync();
        }

        public async Task<DanhMuc?> GetByIdAsync(int id)
        {
            return await _context.DanhMucs.FindAsync(id);
        }

        public async Task AddAsync(DanhMuc danhMuc)
        {
            _context.DanhMucs.Add(danhMuc);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(DanhMuc danhMuc)
        {
            _context.DanhMucs.Update(danhMuc);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var danhMuc = await _context.DanhMucs.FindAsync(id);
            if (danhMuc == null) return;

            // Trong dự án hiện tại, DanhMucId thường được map là MaDanhMuc trong SanPham
            bool hasProducts = await _context.SanPhams.AnyAsync(sp => sp.MaDanhMuc == id);
            if (hasProducts)
            {
                throw new Exception("Không thể xóa danh mục này vì vẫn còn sản phẩm bên trong.");
            }

            _context.DanhMucs.Remove(danhMuc);
            await _context.SaveChangesAsync();
        }
    }
}
