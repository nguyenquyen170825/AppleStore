using Microsoft.AspNetCore.Mvc;
using DUANCUAHANGAPPLE.Data;
using Microsoft.EntityFrameworkCore;
namespace DUANCUAHANGAPPLE.ViewComponents
{
    public class GioHangViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public GioHangViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke()
        {
            var list=_context.GioHangs
                                      .Include(gh => gh.BienTheSanPham)
                                      .Include(gh => gh.BienTheSanPham.SanPham)
                                      .Include(gh => gh.BienTheSanPham.HinhAnhs)    
                                      .ToList();
            return View(list);
        }
    }
}