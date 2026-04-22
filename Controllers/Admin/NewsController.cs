using DUANCUAHANGAPPLE.Models;
using Microsoft.AspNetCore.Mvc;
using DUANCUAHANGAPPLE.Services;
using Microsoft.AspNetCore.Authorization;

namespace DUANCUAHANGAPPLE.Controllers.Admin
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class NewsController : Controller
    {
        private readonly INewsService _newsService;
        private readonly IWebHostEnvironment _env;

        public NewsController(INewsService newsService, IWebHostEnvironment env)
        {
            _newsService = newsService;
            _env = env;
        }

        [HttpGet("Admin/News/Index")]
        public IActionResult Index()
        {
            return View("~/Views/Home/Admin/News/Index.cshtml");
        }

        [HttpGet("Admin/News/Create")]
        public IActionResult Create()
        {
            return View("~/Views/Home/Admin/News/Create.cshtml");
        }

        [HttpGet("Admin/News/Edit/{id}")]
        public IActionResult Edit(int id)
        {
            ViewBag.NewsId = id;
            return View("~/Views/Home/Admin/News/Edit.cshtml");
        }

        [HttpGet("/api/news")]
        public async Task<ActionResult<IEnumerable<New>>> GetAll()
        {
            var news = await _newsService.GetAllAsync();
            return Ok(news);
        }

        [HttpGet("/api/news/{id}")]
        public async Task<ActionResult<New>> GetById(int id)
        {
            var item = await _newsService.GetByIdAsync(id);
            if (item == null) return NotFound(new { message = $"Không tìm thấy bài viết ID {id}" });
            return Ok(item);
        }

        [HttpPost("/api/news")]
        public async Task<IActionResult> Create([FromBody] New news)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            news.Ngaytao = DateTime.Now;
            await _newsService.AddAsync(news);
            return Ok(new { message = "Thêm tin tức thành công", data = news });
        }

        [HttpPut("/api/news/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] New news)
        {
            if (id != news.Id) return BadRequest(new { message = "ID không khớp" });
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _newsService.UpdateAsync(news);
            return NoContent();
        }

        [HttpDelete("/api/news/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _newsService.DeleteAsync(id);
            return NoContent();
        }

        [HttpPost("/api/news/upload")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0) return BadRequest(new { message = "Không có tệp được chọn." });

            var uploadsFolder = Path.Combine(_env.WebRootPath, "images", "news");
            if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Ok(new { url = $"/images/news/{uniqueFileName}" });
        }
    }
}