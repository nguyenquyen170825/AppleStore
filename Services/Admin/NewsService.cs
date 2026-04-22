using DUANCUAHANGAPPLE.Models;
using DUANCUAHANGAPPLE.Data;
using Microsoft.EntityFrameworkCore;

namespace DUANCUAHANGAPPLE.Services
{
    public class NewsService : INewsService
    {
        private readonly ApplicationDbContext _context;
        public NewsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<New>> GetAllAsync()
        {
            return await _context.News.ToListAsync();
        }

        public async Task<New?> GetByIdAsync(int id)
        {
            return await _context.News.FindAsync(id);
        }

        public async Task<int> TotalNews()
        {
            return await _context.News.CountAsync();
        }

        public async Task AddAsync(New news)
        {
            _context.News.Add(news);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(New news)
        {
            _context.News.Update(news);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var news = await _context.News.FindAsync(id);
            if (news != null)
            {
                _context.News.Remove(news);
                await _context.SaveChangesAsync();
            }
        }
    }
}
