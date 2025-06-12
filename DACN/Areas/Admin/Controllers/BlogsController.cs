using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DACN.Models;
using DACN.Utilities;

namespace DACN.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BlogsController : Controller
    {
        private readonly DacnContext _context;

        public BlogsController(DacnContext context)
        {
            _context = context;
        }

        // GET: Admin/Blogs
        public async Task<IActionResult> Index()
        {
            if (!Function.IsLogin())
            {
                return RedirectToAction("Index", "Login");
            }

            var currentAccount = await _context.TbAccounts
                .FirstOrDefaultAsync(m => m.AccountId == Function._AccountId);

            IQueryable<TbBlog> blogsQuery = _context.TbBlogs
                .Include(t => t.Account)
                .Include(t => t.Category);

            if (currentAccount != null)
            {
                if (currentAccount.RoleId == 1)
                {
                    // Admin: xem tất cả bài viết
                    // Không cần lọc gì thêm
                }
                else if (currentAccount.RoleId == 2)
                {
                    // Quản lý danh mục: chỉ xem bài viết thuộc danh mục mình quản lý
                    blogsQuery = blogsQuery.Where(b => b.CategoryId == currentAccount.BlogManagerId);
                }
                else
                {
                    // Các role khác: chỉ xem bài viết đã duyệt
                    blogsQuery = blogsQuery.Where(b => b.Status == 1);
                }
            }
            else
            {
                // Nếu không tìm thấy tài khoản, trả về rỗng
                blogsQuery = blogsQuery.Where(b => false);
            }

            return View(await blogsQuery.ToListAsync());
        }


        // GET: Admin/Blogs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbBlog = await _context.TbBlogs
                .Include(t => t.Account)
                .Include(t => t.Category)
                .FirstOrDefaultAsync(m => m.BlogId == id);
            if (tbBlog == null)
            {
                return NotFound();
            }

            return View(tbBlog);
        }

        // GET: Admin/Blogs/Create
        public IActionResult Create()
        {
            ViewData["AccountId"] = new SelectList(_context.TbAccounts, "AccountId", "AccountId");
            ViewData["CategoryId"] = new SelectList(_context.TbCategories, "CategoryId", "Title");
            return View();
        }

        // POST: Admin/Blogs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BlogId,Title,Alias,CategoryId,Description,Detail,Image,SeoTitle,SeoDescription,SeoKeywords,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy,AccountId,IsActive")] TbBlog tbBlog)
        {
            if (!Function.IsLogin())
            {
                return RedirectToAction("Index", "Login");
            }
            var check = _context.TbAccounts.Where(m => m.AccountId == Function._AccountId).FirstOrDefault();
            if (check.RoleId == 2)
            {
                tbBlog.CategoryId = check.BlogManagerId;
            }
            if (ModelState.IsValid)
            {
                tbBlog.Alias = DACN.Utilities.Function.TitleSlugGenerationAlias(tbBlog.Title);
                tbBlog.Status = 0; // 0 = Chờ duyệt
                _context.Add(tbBlog);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountId"] = new SelectList(_context.TbAccounts, "AccountId", "AccountId", tbBlog.AccountId);
            ViewData["CategoryId"] = new SelectList(_context.TbCategories, "CategoryId", "CategoryId", tbBlog.CategoryId);
            return View(tbBlog);
        }

        // GET: Admin/Blogs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbBlog = await _context.TbBlogs.FindAsync(id);
            if (tbBlog == null)
            {
                return NotFound();
            }
            ViewData["AccountId"] = new SelectList(_context.TbAccounts, "AccountId", "AccountId", tbBlog.AccountId);
            ViewData["CategoryId"] = new SelectList(_context.TbCategories, "CategoryId", "Title", tbBlog.CategoryId);
            return View(tbBlog);
        }

        // POST: Admin/Blogs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BlogId,Title,Alias,CategoryId,Description,Detail,Image,SeoTitle,SeoDescription,SeoKeywords,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy,AccountId,IsActive, Status")] TbBlog tbBlog)
        {
            if (id != tbBlog.BlogId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if(Function._RoleId == 2){
                        tbBlog.Status = 0;
                    }
                    else
                    {
                        tbBlog.Status = 1;
                    }
                        _context.Update(tbBlog);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TbBlogExists(tbBlog.BlogId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountId"] = new SelectList(_context.TbAccounts, "AccountId", "AccountId", tbBlog.AccountId);
            ViewData["CategoryId"] = new SelectList(_context.TbCategories, "CategoryId", "CategoryId", tbBlog.CategoryId);
            return View(tbBlog);
        }

        // GET: Admin/Blogs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbBlog = await _context.TbBlogs
                .Include(t => t.Account)
                .Include(t => t.Category)
                .FirstOrDefaultAsync(m => m.BlogId == id);
            if (tbBlog == null)
            {
                return NotFound();
            }

            return View(tbBlog);
        }

        // POST: Admin/Blogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tbBlog = await _context.TbBlogs.FindAsync(id);
            if (tbBlog != null)
            {
                _context.TbBlogs.Remove(tbBlog);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TbBlogExists(int id)
        {
            return _context.TbBlogs.Any(e => e.BlogId == id);
        }

        // GET: Admin/Blogs/Pending
        public async Task<IActionResult> Pending()
        {
            var pendingBlogs = await _context.TbBlogs
                .Include(b => b.Account).Include(i=>i.Category)
                .Where(b => b.Status == 0)
                .ToListAsync();
            return View(pendingBlogs);
        }

        // POST: Admin/Blogs/Approve/5
        [HttpPost]
        public async Task<IActionResult> Approve(int id)
        {
            var blog = await _context.TbBlogs.FindAsync(id);
            if (blog == null) return NotFound();

            blog.Status = 1; // Đã duyệt
            await _context.SaveChangesAsync();
            return RedirectToAction("Pending");
        }

        // POST: Admin/Blogs/Reject/5
        [HttpPost]
        public async Task<IActionResult> Reject(int id)
        {
            var blog = await _context.TbBlogs.FindAsync(id);
            if (blog == null) return NotFound();

            blog.Status = 2; // Từ chối
            await _context.SaveChangesAsync();
            return RedirectToAction("Pending");
        }

    }
}
