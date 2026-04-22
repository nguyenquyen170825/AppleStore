using DUANCUAHANGAPPLE.Models;
using DUANCUAHANGAPPLE.Services;
using DUANCUAHANGAPPLE.ViewModels;
using Microsoft.AspNetCore.Mvc;
using DUANCUAHANGAPPLE.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace DUANCUAHANGAPPLE.Controllers.Admin
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly IDanhMucService _danhMucService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IProductService productService, IDanhMucService danhMucService, IWebHostEnvironment webHostEnvironment)
        {
            _productService = productService;
            _danhMucService = danhMucService;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet("Admin/Product/Detail/{id}")]
        public IActionResult Detail(int id)
        {
            ViewBag.ProductId = id;
            return View("~/Views/Home/Admin/Product/Detail.cshtml");
        }

        [HttpGet("Admin/Product/Index")]
        public async Task<IActionResult> Index()
        {
            var categories = await _danhMucService.GetAllAsync();
            ViewBag.DanhMucNames = categories.Select(dm => new { dm.MaDanhMuc, dm.TenDanhMuc }).ToList();
            return View("~/Views/Home/Admin/Product/Index.cshtml");
        }

        [HttpGet("Admin/Product/Create")]
        public async Task<IActionResult> Create()
        {
            var categories = await _danhMucService.GetAllAsync();
            ViewBag.DanhMucNames = categories.Select(dm => new { dm.MaDanhMuc, dm.TenDanhMuc }).ToList();
            return View("~/Views/Home/Admin/Product/Create.cshtml");
        }

        [HttpGet("Admin/Product/Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var categories = await _danhMucService.GetAllAsync();
            ViewBag.DanhMucNames = categories.Select(dm => new { dm.MaDanhMuc, dm.TenDanhMuc }).ToList();
            ViewBag.ProductId = id;
            return View("~/Views/Home/Admin/Product/Edit.cshtml");
        }

        [HttpGet("Admin/Product/CreateVariant")]
        public IActionResult CreateVariant([FromQuery] int productId)
        {
            ViewBag.ProductId = productId;
            return View("~/Views/Home/Admin/Product/CreateVariant.cshtml");
        }

        [HttpGet("Admin/Product/EditVariant/{id}")]
        public IActionResult EditVariant(int id)
        {
            ViewBag.VariantId = id;
            return View("~/Views/Home/Admin/Product/EditVariant.cshtml");
        }

        // API endpoints
        [HttpGet("/api/product/{id}")]
        public async Task<ActionResult<ProductDetailDTO>> GetById(int id)
        {
            var product = await _productService.GetProductDetailAsync(id);
            if (product == null) return NotFound(new { message = $"Không tìm thấy sản phẩm ID {id}" });
            return Ok(product);
        }

        [HttpGet("/api/product")]
        public async Task<ActionResult<IEnumerable<ProductListDTO>>> GetAll()
        {
            var products = await _productService.GetAllProductsAsync();
            
            var response = products.Select(p => new ProductListDTO
            {
                SanPhamId = p.SanPhamId,
                TenSanPham = p.TenSanPham,
                TenDanhMuc = p.DanhMuc?.TenDanhMuc ?? "N/A",
                TrangThai = p.TrangThai,
                // Lấy giá thấp nhất của các biến thể làm giá hiển thị
                GiaGoc = p.BienThes?.Any() == true ? p.BienThes.Min(v => v.Gia) : 0,
                // Tổng tồn kho của tất cả biến thể
                TongSoLuongTon = p.BienThes?.Sum(v => v.SoLuongTon) ?? 0,
                // Lấy ảnh của biến thể đầu tiên làm ảnh đại diện
                HinhAnhDaiDien = p.BienThes?.FirstOrDefault()?.HinhAnhs?.FirstOrDefault()?.UrlHinhAnh ?? ""
            }).ToList();

            return Ok(response);
        }

        [HttpPost("/api/product")]
        public async Task<ActionResult<SanPham>> Create([FromBody] SanPham product)
        {
            ModelState.Remove("DanhMuc");
            ModelState.Remove("BienThes");
            ModelState.Remove("ThongSoKyThuats");
            ModelState.Remove("ChiTietThanhToans");

            if (!ModelState.IsValid) return BadRequest(ModelState);

            product.NgayTao = DateTime.Now;
            await _productService.AddProductAsync(product);
            return CreatedAtAction(nameof(GetById), new { id = product.SanPhamId }, product);
        }

        [HttpPut("/api/product/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] SanPham product)
        {
            if (id != product.SanPhamId) return BadRequest(new { message = "ID không khớp" });
            
            ModelState.Remove("DanhMuc");
            ModelState.Remove("BienThes");
            ModelState.Remove("ThongSoKyThuats");
            ModelState.Remove("ChiTietThanhToans");

            if (!ModelState.IsValid) return BadRequest(ModelState);

            await _productService.UpdateProductAsync(product);
            return NoContent();
        }

        [HttpPut("/api/product/specs/{id}")]
        public async Task<IActionResult> UpdateSpecs(int id, [FromBody] ThongSoKyThuat thongSo)
        {
            await _productService.UpdateProductSpecsAsync(thongSo);
            return NoContent();
        }

        [HttpDelete("/api/product/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _productService.DeleteProductAsync(id);
            return NoContent();
        }

        [HttpPost("/api/variant")]
        public async Task<ActionResult<BienTheSanPham>> CreateVariantAPI([FromForm] BienTheVM model)
        {
            var variant = new BienTheSanPham
            {
                SanPhamId = model.SanPhamId,
                Mau = model.Mau,
                DungLuong = model.DungLuong,
                Gia = model.Gia,
                GiaCu = model.GiaCu ?? 0,
                SoLuongTon = model.SoLuongTon,
                NgayTao = DateTime.Now,
                TrangThai = true
            };

            var files = Request.Form.Files.GetFiles("HinhAnhFiles");
            if (files != null && files.Count > 0)
            {
                variant.HinhAnhs = new List<HinhAnh>();
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                bool isFirst = true;
                foreach (var file in files)
                {
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }

                    variant.HinhAnhs.Add(new HinhAnh { UrlHinhAnh = "/uploads/" + uniqueFileName, LaAnhChinh = isFirst });
                    isFirst = false;
                }
            }

            await _productService.AddProductItemAsync(variant);
            return Ok(variant);
        }
        
        [HttpDelete("/api/variant/{id}")]
        public async Task<IActionResult> DeleteVariant(int id)
        {
            try
            {
                await _productService.DeleteProductItemAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("/api/variant/{id}")]
        public async Task<IActionResult> GetVariantById(int id)
        {
            var variant = await _productService.GetVariantByIdAsync(id);
            if (variant == null) return NotFound(new { message = "Không tìm thấy biến thể!" });

            return Ok(new
            {
                sanPhamId = variant.SanPhamId,
                mau = variant.Mau,
                dungLuong = variant.DungLuong,
                gia = variant.Gia,
                giaCu = variant.GiaCu,
                soLuongTon = variant.SoLuongTon,
                hinhAnhs = variant.HinhAnhs?.Select(h => new { id = h.HinhAnhId, url = h.UrlHinhAnh }).ToList()
            });
        }

        [HttpPut("/api/variant/{id}")]
        public async Task<IActionResult> UpdateVariantAPI(int id, [FromForm] BienTheVM model)
        {
            var variant = await _productService.GetVariantByIdAsync(id);
            if (variant == null) return NotFound(new { message = "Không tìm thấy biến thể!" });

            variant.Mau = model.Mau;
            variant.DungLuong = model.DungLuong;
            variant.Gia = model.Gia;
            variant.GiaCu = model.GiaCu ?? 0;
            variant.SoLuongTon = model.SoLuongTon;

            var files = Request.Form.Files.GetFiles("HinhAnhFiles");
            if (files != null && files.Count > 0)
            {
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                if (variant.HinhAnhs == null) variant.HinhAnhs = new List<HinhAnh>();

                bool isFirst = !variant.HinhAnhs.Any(h => h.LaAnhChinh == true);
                foreach (var file in files)
                {
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }

                    variant.HinhAnhs.Add(new HinhAnh { UrlHinhAnh = "/uploads/" + uniqueFileName, LaAnhChinh = isFirst });
                    isFirst = false;
                }
            }

            await _productService.UpdateProductItemAsync(variant);
            return Ok(new { message = "Cập nhật thành công!" });
        }
    }
}
