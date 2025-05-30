using DACN.Models;
using Microsoft.AspNetCore.Mvc;

namespace DACN.Controllers
{
    public class BlogCommentController : Controller
    {
        private readonly DacnContext _context;
        public BlogCommentController(DacnContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(string name, string phone, string email, string message)
        {
            try
            {
                TbBlogComment comment = new TbBlogComment();
                comment.Name = name;
                comment.Phone = phone;
                comment.Email = email;
                comment.Detail = message;
                comment.CreatedDate = DateTime.Now;
                _context.Add(comment);
                _context.SaveChangesAsync();
                return Json(new { status = true });
            }
            catch
            {
                return Json(new { status = false });
            }
        }
    }
}
