using DUANCUAHANGAPPLE.Models;
using DUANCUAHANGAPPLE.Data;
using Microsoft.EntityFrameworkCore;

namespace DUANCUAHANGAPPLE.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task AddAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> TotalUser()
        {
            return await _context.Users.CountAsync();
        }

        public async Task<decimal> GetTotalRevenue()
        {
            return await _context.ThanhToans
                .Where(o => o.TrangThai == "Hoàn tất" || o.TrangThai == "Đã giao hàng")
                .SumAsync(o => o.TongTien);
        }
    }
}
