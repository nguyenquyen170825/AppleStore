using Microsoft.AspNetCore.Mvc;
using DUANCUAHANGAPPLE.Data;
using DUANCUAHANGAPPLE.Models;

namespace DUANCUAHANGAPPLE.Controllers
{
    public class NewController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NewController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var data = _context.News
                .OrderByDescending(x => x.Ngaytao)
                .Take(20)
                .ToList();

            return Ok(data);
        }
    }
}