using DUANCUAHANGAPPLE.Models;
using DUANCUAHANGAPPLE.DTOs;

namespace DUANCUAHANGAPPLE.Services
{
    public interface IProductService
    {
        Task<IEnumerable<SanPham>> GetAllProductsAsync();
        Task<ProductDetailDTO> GetProductDetailAsync(int id);
        Task AddProductAsync(SanPham product);
        Task UpdateProductAsync(SanPham product);
        Task DeleteProductAsync(int id);
        Task AddProductItemAsync(BienTheSanPham bienthe);
        Task UpdateProductItemAsync(BienTheSanPham bienthe);
        Task DeleteProductItemAsync(int id);
        Task<int> TotalProduct();
        Task<IEnumerable<HinhAnh>> GetImagesByVariantAsync(int variantId);
        Task SetMainImageAsync(int imageId, int variantId);
        Task UpdateProductSpecsAsync(ThongSoKyThuat thongSo);
        Task<BienTheSanPham> GetVariantByIdAsync(int id);
    }
}
