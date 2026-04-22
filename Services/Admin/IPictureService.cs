using DUANCUAHANGAPPLE.Models;

namespace DUANCUAHANGAPPLE.Services
{
    public interface IPictureService
    {
        Task<IEnumerable<HinhAnh>> GetAllAsync();
        Task AddAsync(HinhAnh hinhAnh);
        Task DeleteAsync(int id);
    }
}
