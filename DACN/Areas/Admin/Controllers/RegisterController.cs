using DACN.Models;
using DACN.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace DACN.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RegisterController : Controller
    {
        private readonly DacnContext _context;
        public RegisterController(DacnContext context)
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
            var check = _context.TbAccounts.Where(m => m.Email == tbAccount.Email).FirstOrDefault();
            if (check != null)
            {
                Function._MessageEmail = "Duplicate Email!";
                return RedirectToAction("Index", "Register");
            }
            Function._MessageEmail = string.Empty;
            tbAccount.Password = Function.MD5Password(tbAccount.Password);
            _context.Add(tbAccount);
            _context.SaveChanges();
            return RedirectToAction("Index", "Login");
        }
    }
}