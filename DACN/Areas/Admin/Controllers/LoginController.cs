using DACN.Models;
using DACN.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace DACN.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LoginController : Controller
    {
        private readonly DacnContext _context;
        public LoginController(DacnContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(TbAccount tbAccount)
        {
            if (tbAccount == null)
            {
                return NotFound();
            }
            string pw = Function.MD5Password(tbAccount.Password);
            var check = _context.TbAccounts
    .FirstOrDefault(m => m.Email == tbAccount.Email && m.Password == pw);

            if (check == null)
            {
                Function._Message = "Invalid Username or password";
                return RedirectToAction("Index", "Login");
            }

            // Kiểm tra quyền 
            if (check.RoleId == 3)
            {
                Function._Message = "This account has no permissions";
                return RedirectToAction("Index", "Login");
            }

            // Gán thông tin tài khoản vào Function
            Function._Message = string.Empty;
            Function._MessageAcc = string.Empty;
            Function._RoleId = check.RoleId;
            Function._AccountId = check.AccountId;
            Function._Username = check.Username ?? string.Empty;
            Function._Email = check.Email ?? string.Empty;

            return RedirectToAction("Index", "Home");

        }
    }
}