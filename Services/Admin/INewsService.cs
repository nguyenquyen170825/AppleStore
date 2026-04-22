using DUANCUAHANGAPPLE.Models;

namespace DUANCUAHANGAPPLE.Services
{
    public interface INewsService
    {
        Task<IEnumerable<New>> GetAllAsync();
        Task<New?> GetByIdAsync(int id);
        Task<int> TotalNews();
        Task AddAsync(New news);
        Task UpdateAsync(New news);
        Task DeleteAsync(int id);
    }
}
