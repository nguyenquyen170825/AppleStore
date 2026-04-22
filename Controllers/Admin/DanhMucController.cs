using DUANCUAHANGAPPLE.Models;
using DUANCUAHANGAPPLE.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace DUANCUAHANGAPPLE.Controllers.Admin
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DanhMucController : Controller
    {
        private readonly IDanhMucService _danhMucService;

        public DanhMucController(IDanhMucService danhMucService)
        {
            _danhMucService = danhMucService;
        }

        [HttpGet("Admin/DanhMuc/Index")]
        public IActionResult Index()
        {
            return View("~/Views/Home/Admin/DanhMuc/Index.cshtml");
        }

        [HttpGet("Admin/DanhMuc/Create")]
        public IActionResult Create()
        {
            return View("~/Views/Home/Admin/DanhMuc/Create.cshtml");
        }

        [HttpGet("Admin/DanhMuc/Edit/{id}")]
        public IActionResult Edit(int id)
        {
            ViewBag.CategoryId = id;
            return View("~/Views/Home/Admin/DanhMuc/Edit.cshtml");
        }

        [HttpGet("/api/danhmuc")]
        public async Task<ActionResult<IEnumerable<DanhMuc>>> GetAll()
        {
            var danhMucs = await _danhMucService.GetAllAsync();
            return Ok(danhMucs);
        }

        [HttpGet("/api/danhmuc/{id}")]
        public async Task<ActionResult<DanhMuc>> GetById(int id)
        {
            var danhMuc = await _danhMucService.GetByIdAsync(id);
            if (danhMuc == null) return NotFound(new { message = $"Không tìm thấy danh mục ID {id}" });
            return Ok(danhMuc);
        }

        [HttpPost("/api/danhmuc")]
        public async Task<ActionResult<DanhMuc>> Create([FromBody] DanhMuc danhMuc)
        {
            ModelState.Remove("SanPham");
            ModelState.Remove("SanPhams");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await _danhMucService.AddAsync(danhMuc);
            return CreatedAtAction(nameof(GetById), new { id = danhMuc.MaDanhMuc }, danhMuc);
        }

        [HttpPut("/api/danhmuc/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] DanhMuc danhMuc)
        {
            ModelState.Remove("SanPham");
            ModelState.Remove("SanPhams");
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _danhMucService.UpdateAsync(danhMuc);
            return NoContent();
        }

        [HttpDelete("/api/danhmuc/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id) || id.ToLower() == "null" || !int.TryParse(id, out int categoryId))
            {
                return BadRequest(new { message = "400 Bad Request" });
            }

            var danhMuc = await _danhMucService.GetByIdAsync(categoryId);
            if (danhMuc == null)
            {
                return NotFound(new { message = "404 Not Found" });
            }

            try
            {
                await _danhMucService.DeleteAsync(categoryId);
                return Ok(new { message = "200 OK / Xóa thành công" });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = "Lỗi hoặc không cho xóa",
                    detail = ex.Message
                });
            }
        }
    }
}
