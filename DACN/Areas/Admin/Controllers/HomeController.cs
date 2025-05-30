using DACN.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace DACN.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        [Area("Admin")]
        public IActionResult Index()
        {
            if (!Function.IsLogin())
                return RedirectToAction("Index", "Login");
            return View();
        }
        public IActionResult LogOut()
        {
            Function._AccountId = 0;
            Function._RoleId = 0;
            Function._Username = string.Empty;
            Function._Email = string.Empty;
            Function._Message = string.Empty;
            Function._MessageAcc = string.Empty;
            Function._MessageEmail = string.Empty;
            return RedirectToAction("Index", "Home");
        }
    }
}
