using DUANCUAHANGAPPLE.Models;

namespace DUANCUAHANGAPPLE.Services
{
    public interface IDanhMucService
    {
        Task<IEnumerable<DanhMuc>> GetAllAsync();
        Task<DanhMuc?> GetByIdAsync(int id);
        Task AddAsync(DanhMuc danhMuc);
        Task UpdateAsync(DanhMuc danhMuc);
        Task DeleteAsync(int id);
    }
}
