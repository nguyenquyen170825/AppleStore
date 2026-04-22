using DUANCUAHANGAPPLE.Models;
using DUANCUAHANGAPPLE.Services;
using DUANCUAHANGAPPLE.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace DUANCUAHANGAPPLE.Controllers.Admin
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class PictureController : Controller
    {
        private readonly IPictureService _pictureService;
        private readonly IProductService _productService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PictureController(IPictureService pictureService, IProductService productService, IWebHostEnvironment webHostEnvironment)
        {
            _pictureService = pictureService;
            _productService = productService;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet("Admin/Picture/Index/{id}")]
        public IActionResult Index(int id)
        {
            ViewBag.VariantId = id;
            return View("~/Views/Home/Admin/Picture/Index.cshtml");
        }

        [HttpGet("Admin/Picture/Create/{id}")]
        public IActionResult Create(int id)
        {
            ViewBag.VariantId = id;
            return View("~/Views/Home/Admin/Picture/Create.cshtml");
        }

        [HttpGet("/api/picture/{variantId}")]
        public async Task<IActionResult> GetPicturesByVariantId(int variantId)
        {
            var pictures = await _productService.GetImagesByVariantAsync(variantId);

            var pictureVMs = pictures.Select(p => new HinhAnhVM
            {
                HinhAnhId = p.HinhAnhId,
                BienTheId = p.BienTheId,
                UrlHinhAnh = p.UrlHinhAnh,
                LaAnhChinh = p.LaAnhChinh
            }).ToList();

            return Ok(pictureVMs);
        }

        [HttpDelete("/api/picture/{id}")]
        public async Task<IActionResult> DeletePicture(int id)
        {
            await _pictureService.DeleteAsync(id);
            return Ok();
        }

        [HttpPost("/api/picture")]
        public async Task<IActionResult> AddPicture([FromForm] int variantId, [FromForm] bool isMain, [FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest(new { message = "Vui lòng chọn một tệp ảnh." });

            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            var picture = new HinhAnh
            {
                BienTheId = variantId,
                UrlHinhAnh = "/uploads/" + uniqueFileName,
                LaAnhChinh = isMain,
                NgayTao = DateTime.Now
            };

            await _pictureService.AddAsync(picture);
            return Ok();
        }
    }
}
