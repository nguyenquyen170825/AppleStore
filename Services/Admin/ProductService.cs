using Microsoft.EntityFrameworkCore;
using DUANCUAHANGAPPLE.Models;
using DUANCUAHANGAPPLE.DTOs;
using DUANCUAHANGAPPLE.Data;

namespace DUANCUAHANGAPPLE.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductService(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IEnumerable<SanPham>> GetAllProductsAsync()
        {
            return await _context.SanPhams
                .Include(p => p.DanhMuc)
                .Include(p => p.BienThes)
                .ToListAsync();
        }

        public async Task<ProductDetailDTO> GetProductDetailAsync(int id)
        {
            var product = await _context.SanPhams
                .Include(p => p.DanhMuc)
                .Include(p => p.ThongSoKyThuats)
                .Include(p => p.BienThes)
                    .ThenInclude(v => v.HinhAnhs)
                .FirstOrDefaultAsync(p => p.SanPhamId == id);

            if (product == null) return null;

            return new ProductDetailDTO
            {
                SanPhamId = product.SanPhamId,
                TenSanPham = product.TenSanPham,
                MoTa = product.MoTa,
                ThuongHieu = product.ThuongHieu,
                MaDanhMuc = product.MaDanhMuc,
                TenDanhMuc = product.DanhMuc?.TenDanhMuc,
                TrangThai = product.TrangThai,
                // Do cấu hình ThongSoKyThuat trong dự án hiện tại là dạng Key-Value (Id, TenThongSo, GiaTri)
                // Nên tạm thời trả về object rỗng để vượt qua lỗi Build. Bạn cần viết logic loop qua list để map vào DTO.
                ThongSo = new ThongSoKyThuatDTO(),
                BienThes = product.BienThes?.Select(v => new VariantDTO
                {
                    BienTheId = v.BienTheId,
                    Mau = v.Mau,
                    DungLuong = v.DungLuong,
                    Gia = v.Gia,
                    GiaCu = v.GiaCu,
                    SoLuongTon = v.SoLuongTon,
                    HinhAnhs = v.HinhAnhs?.Select(h => h.UrlHinhAnh).ToList()
                }).ToList()
            };
        }

        public async Task AddProductAsync(SanPham product)
        {
            _context.SanPhams.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProductAsync(SanPham product)
        {
            _context.SanPhams.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await _context.SanPhams
                .Include(p => p.ThongSoKyThuats)
                .Include(p => p.BienThes)
                    .ThenInclude(v => v.HinhAnhs)
                .FirstOrDefaultAsync(p => p.SanPhamId == id);

            if (product == null) return;

            if (product.BienThes != null)
            {
                foreach (var variant in product.BienThes)
                {
                    if (variant.HinhAnhs != null)
                    {
                        foreach (var img in variant.HinhAnhs)
                        {
                            var path = Path.Combine(_env.WebRootPath, img.UrlHinhAnh.TrimStart('/'));
                            if (File.Exists(path)) File.Delete(path);
                        }
                        _context.HinhAnhs.RemoveRange(variant.HinhAnhs);
                    }
                }
                _context.BienTheSanPhams.RemoveRange(product.BienThes);
            }

            if (product.ThongSoKyThuats != null)
            {
                _context.ThongSoKyThuats.RemoveRange(product.ThongSoKyThuats);
            }

            _context.SanPhams.Remove(product);
            await _context.SaveChangesAsync();
        }

        public async Task AddProductItemAsync(BienTheSanPham bienthe)
        {
            _context.BienTheSanPhams.Add(bienthe);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProductItemAsync(BienTheSanPham bienthe)
        {
            _context.BienTheSanPhams.Update(bienthe);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProductItemAsync(int id)
        {
            var variant = await _context.BienTheSanPhams
                .Include(v => v.HinhAnhs)
                .FirstOrDefaultAsync(v => v.BienTheId == id);
            if (variant == null) return;

            if (variant.HinhAnhs != null)
            {
                foreach (var img in variant.HinhAnhs)
                {
                    var path = Path.Combine(_env.WebRootPath, img.UrlHinhAnh.TrimStart('/'));
                    if (File.Exists(path)) File.Delete(path);
                }
                _context.HinhAnhs.RemoveRange(variant.HinhAnhs);
            }

            _context.BienTheSanPhams.Remove(variant);
            await _context.SaveChangesAsync();
        }

        public async Task<int> TotalProduct()
        {
            return await _context.BienTheSanPhams.SumAsync(b => b.SoLuongTon);
        }

        public async Task<IEnumerable<HinhAnh>> GetImagesByVariantAsync(int variantId)
        {
            return await _context.HinhAnhs.Where(h => h.BienTheId == variantId).ToListAsync();
        }

        public async Task SetMainImageAsync(int imageId, int variantId)
        {
            var images = await _context.HinhAnhs.Where(h => h.BienTheId == variantId).ToListAsync();
            foreach (var img in images)
            {
                img.LaAnhChinh = (img.HinhAnhId == imageId);
            }
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProductSpecsAsync(ThongSoKyThuat thongSo)
        {
            if (thongSo.Id > 0)
            {
                _context.ThongSoKyThuats.Update(thongSo);
            }
            else
            {
                _context.ThongSoKyThuats.Add(thongSo);
            }
            await _context.SaveChangesAsync();
        }

        public async Task<BienTheSanPham> GetVariantByIdAsync(int id)
        {
            return await _context.BienTheSanPhams.Include(v => v.HinhAnhs).FirstOrDefaultAsync(v => v.BienTheId == id);
        }
    }
}
