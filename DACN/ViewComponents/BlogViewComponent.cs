using DACN.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DACN.ViewComponents
{
    public class BlogViewComponent : ViewComponent
    {
        private readonly DacnContext _context;

        public BlogViewComponent(DacnContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var items = _context.TbBlogs.Where(m => (bool)m.IsActive);
            return await Task.FromResult<IViewComponentResult>
                (View(items.OrderByDescending(m => m.CreatedDate).ToList()));
        }
    }
}
