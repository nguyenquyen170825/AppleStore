using DUANCUAHANGAPPLE.Models;
using DUANCUAHANGAPPLE.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace DUANCUAHANGAPPLE.Controllers.Admin
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("Admin/User/Index")]
        public IActionResult Index()
        {
            return View("~/Views/Home/Admin/User/Index.cshtml");
        }

        [HttpGet("Admin/User/Create")]
        public IActionResult Create()
        {
            ViewBag.Roles = new List<string> { "Admin", "Customer" };
            return View("~/Views/Home/Admin/User/Create.cshtml");
        }

        [HttpGet("Admin/User/Edit/{id}")]
        public IActionResult Edit(int id)
        {
            ViewBag.UserId = id;
            return View("~/Views/Home/Admin/User/Edit.cshtml");
        }

        [HttpGet("/api/user")]
        public async Task<ActionResult<IEnumerable<User>>> GetAll([FromQuery] string? role)
        {
            var users = await _userService.GetAllAsync();
            if (!string.IsNullOrEmpty(role))
            {
                users = users.Where(u => u.Role == role);
            }
            return Ok(users);
        }

        [HttpGet("/api/user/{id}")]
        public async Task<ActionResult<User>> GetById(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null) return NotFound(new { message = $"Không tìm thấy người dùng ID {id}" });
            return Ok(user);
        }

        [HttpPost("/api/user")]
        public async Task<ActionResult<User>> Create([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                ModelState.Remove("ThanhToans");
                ModelState.Remove("PhieuNguoiDungs");
                ModelState.Remove("EmailOtps");
                if (!ModelState.IsValid) return BadRequest(ModelState);
            }

            await _userService.AddAsync(user);
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }

        [HttpPut("/api/user/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] User user)
        {
            if (id != user.Id) return BadRequest(new { message = "ID không khớp" });
            
            ModelState.Remove("ThanhToans");
            ModelState.Remove("PhieuNguoiDungs");
            ModelState.Remove("EmailOtps");

            if (!ModelState.IsValid) return BadRequest(ModelState);

            await _userService.UpdateAsync(user);
            return NoContent();
        }

        [HttpDelete("/api/user/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _userService.DeleteAsync(id);
            return NoContent();
        }
    }
}
