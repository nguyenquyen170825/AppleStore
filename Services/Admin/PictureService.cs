using DUANCUAHANGAPPLE.Models;
using DUANCUAHANGAPPLE.Data;
using Microsoft.EntityFrameworkCore;

namespace DUANCUAHANGAPPLE.Services
{
    public class PictureService : IPictureService
    {
        private readonly ApplicationDbContext _context;
        public PictureService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<HinhAnh>> GetAllAsync()
        {
            // Trong model HinhAnh của bạn là BienTheSanPham chứ không phải BienThe
            return await _context.HinhAnhs.Include(h => h.BienTheSanPham).ToListAsync();
        }

        public async Task AddAsync(HinhAnh hinhAnh)
        {
            _context.HinhAnhs.Add(hinhAnh);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var img = await _context.HinhAnhs.FindAsync(id);
            if (img != null)
            {
                _context.HinhAnhs.Remove(img);
                await _context.SaveChangesAsync();
            }
        }
    }
}
