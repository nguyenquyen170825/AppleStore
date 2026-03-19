using Microsoft.AspNetCore.Mvc;
using DUANCUAHANGAPPLE.Data;
using Microsoft.EntityFrameworkCore;
namespace DUANCUAHANGAPPLE.ViewComponents
{
    public class MenudanhmucViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public MenudanhmucViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke()
        {
            var danhmuc=_context.DanhMucs.ToList();
            return View(danhmuc);
        }
    }
}