using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DUANCUAHANGAPPLE.Controllers
{
    public class AccountController : Controller
    {
        [Authorize]
        [HttpGet]
        public IActionResult Personal()
        {
            return View("User/Account/Personal");
        }
    }
}