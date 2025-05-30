using DACN.Models;
using DACN.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace DACN.Controllers
{
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
            var check = _context.TbAccounts.Where(m => (m.Email == tbAccount.Email) && (m.Password == pw)).FirstOrDefault();
            if (check == null)
            {
                Function._Message = "Invalid Username or password";
                return RedirectToAction("Index", "Login");
            }
            Function._Message = string.Empty;
            Function._AccountId = check.AccountId;
            Function._Username = string.IsNullOrEmpty(check.Username) ? string.Empty : check.Username;
            Function._Email = string.IsNullOrEmpty(check.Email) ? string.Empty : check.Email;
            return RedirectToAction("Index", "Home");
        }
    }
}