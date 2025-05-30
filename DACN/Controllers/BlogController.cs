using DACN.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace DACN.Controllers
{
    public class BlogController : Controller
    {
        private readonly DacnContext _context;

        public BlogController(DacnContext context)
        {
            _context = context;
        }

        public IActionResult Index(int id)
        {

            var blog = _context.TbBlogs.Include(i => i.TbBlogComments).Where(i => i.CategoryId == id).ToList();
            return View(blog);
        }

        public IActionResult Search(string searchString)
        {
            if (string.IsNullOrEmpty(searchString))
            {
                return View("Index", new List<TbBlog>());
            }
            var blogs = _context.TbBlogs
                .Where(b => b.Title.Contains(searchString) || b.Detail.Contains(searchString) || b.Description.Contains(searchString)).Include(i => i.TbBlogComments)
                .ToList();
            return View( blogs);
        }


            [Route("/blog/{alias}-{id}.html")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TbBlogs == null)
            {
                return NotFound();
            }

            var blog = await _context.TbBlogs
                .FirstOrDefaultAsync(m => m.BlogId == id);
            if (blog == null)
            {
                return NotFound();
            }
            ViewBag.blogComment = _context.TbBlogComments.Where(i => i.BlogId == id).ToList();
            return View(blog);
        }
    }
}
