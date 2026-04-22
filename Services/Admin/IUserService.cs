using DUANCUAHANGAPPLE.Models;

namespace DUANCUAHANGAPPLE.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> GetByIdAsync(int id);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(int id);
        Task<int> TotalUser();
        Task<decimal> GetTotalRevenue();
    }
}
