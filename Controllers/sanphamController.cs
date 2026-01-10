using Microsoft.AspNetCore.Mvc;

namespace DUANCUAHANGAPPLE.Controllers
{
    public class SanPhamController : Controller
        {
            private readonly ApplicationDbContext _context;
            public SanPhamController(ApplicationDbContext context)
            {
                _context = context;
            }

            
        }
}