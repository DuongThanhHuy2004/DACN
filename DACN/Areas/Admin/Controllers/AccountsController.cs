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
    public class AccountsController : Controller
    {
        private readonly DacnContext _context;

        public AccountsController(DacnContext context)
        {
            _context = context;
        }

        // GET: Admin/Accounts
        public async Task<IActionResult> Index()
        {
            var dacnContext = _context.TbAccounts.Include(t => t.BlogManager).Include(t => t.Role);
            return View(await dacnContext.ToListAsync());
        }

        // GET: Admin/Accounts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbAccount = await _context.TbAccounts
                .Include(t => t.BlogManager)
                .Include(t => t.Role)
                .FirstOrDefaultAsync(m => m.AccountId == id);
            if (tbAccount == null)
            {
                return NotFound();
            }

            return View(tbAccount);
        }

        // GET: Admin/Accounts/Create
        public IActionResult Create()
        {
            ViewData["BlogManagerId"] = new SelectList(_context.TbCategories, "CategoryId", "CategoryId");
            ViewData["RoleId"] = new SelectList(_context.TbRoles, "RoleId", "RoleId");
            return View();
        }

        // POST: Admin/Accounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AccountId,Username,Password,FullName,Phone,Email,RoleId,LastLogin,IsActive,BlogManagerId")] TbAccount tbAccount)
        {
            if (ModelState.IsValid)
            {
                // Hash mật khẩu trước khi lưu
                tbAccount.Password = Function.MD5Password(tbAccount.Password);

                _context.Add(tbAccount);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BlogManagerId"] = new SelectList(_context.TbCategories, "CategoryId", "Title", tbAccount.BlogManagerId);
            ViewData["RoleId"] = new SelectList(_context.TbRoles, "RoleId", "RoleName", tbAccount.RoleId);
            return View(tbAccount);
        }


        // GET: Admin/Accounts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbAccount = await _context.TbAccounts.FindAsync(id);
            if (tbAccount == null)
            {
                return NotFound();
            }
            ViewData["BlogManagerId"] = new SelectList(_context.TbCategories, "CategoryId", "Title", tbAccount.BlogManagerId);
            ViewData["RoleId"] = new SelectList(_context.TbRoles, "RoleId", "RoleName", tbAccount.RoleId);
            return View(tbAccount);
        }

        // POST: Admin/Accounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AccountId,Username,Password,FullName,Phone,Email,RoleId,LastLogin,IsActive,BlogManagerId")] TbAccount tbAccount)
        {
            if (id != tbAccount.AccountId)
            {
                return NotFound();
            }
            var account = await _context.TbAccounts.FindAsync(id);
            if (ModelState.IsValid)
            {
                try
                {
                    if (account == null)
                        return NotFound();

                    // Cập nhật các trường khác
                    account.Username = tbAccount.Username;
                    account.FullName = tbAccount.FullName;
                    account.Phone = tbAccount.Phone;
                    account.Email = tbAccount.Email;
                    account.RoleId = tbAccount.RoleId;
                    account.LastLogin = tbAccount.LastLogin;
                    account.IsActive = tbAccount.IsActive;

                    if (Function._RoleId == 1 || account.AccountId != Function._AccountId)
                    {
                        account.RoleId = tbAccount.RoleId;
                    }

                    if (Function._RoleId == 1 && account.RoleId == 2)
                    {
                        account.BlogManagerId = tbAccount.BlogManagerId;
                    }

                    // Xử lý mật khẩu
                    if (!string.IsNullOrEmpty(tbAccount.Password))
                    {
                        // Nếu có nhập mật khẩu mới, hãy hash trước khi lưu (nếu có dùng hash)
                        account.Password = Function.MD5Password(tbAccount.Password);
                        // Thay bằng Hash(tbAccount.Password) nếu có hash
                    }
                    // Nếu không nhập mật khẩu mới, giữ nguyên mật khẩu cũ

                    _context.Update(account);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TbAccountExists(tbAccount.AccountId))
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

            ViewData["BlogManagerId"] = new SelectList(_context.TbCategories, "CategoryId", "Title", tbAccount.BlogManagerId);
            ViewData["RoleId"] = new SelectList(_context.TbRoles, "RoleId", "RoleName", tbAccount.RoleId);
            return View(tbAccount);
        }


        // GET: Admin/Accounts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbAccount = await _context.TbAccounts
                .Include(t => t.BlogManager)
                .Include(t => t.Role)
                .FirstOrDefaultAsync(m => m.AccountId == id);
            if (tbAccount == null)
            {
                return NotFound();
            }

            return View(tbAccount);
        }

        // POST: Admin/Accounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tbAccount = await _context.TbAccounts.FindAsync(id);
            if (tbAccount != null)
            {
                _context.TbAccounts.Remove(tbAccount);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TbAccountExists(int id)
        {
            return _context.TbAccounts.Any(e => e.AccountId == id);
        }
    }
}


