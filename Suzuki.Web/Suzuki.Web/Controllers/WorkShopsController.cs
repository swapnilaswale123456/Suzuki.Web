using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Suzuki.Web.Common;
using Suzuki.Web.Models;

namespace Suzuki.Web.Controllers
{
    public class WorkShopsController : Controller
    {
        private readonly SuzukiDBContext _context;

        public WorkShopsController(SuzukiDBContext context)
        {
            _context = context;
        }

        // GET: WorkShops
        public async Task<IActionResult> Index(string currentFilter, string searchString, int? pageNumber)
        {
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;
            var suzukiDBContext = _context.WorkShop.Include(w => w.Distributer);
            var workShops = from s in suzukiDBContext
                              select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                workShops = suzukiDBContext.Where(s => s.WorkShopName.Contains(searchString)
                                      || s.WorkShopCode.Contains(searchString) || s.Address.Contains(searchString) || s.Email.Contains(searchString));
            }
            int pageSize = 10;
            return View(await PaginatedList<WorkShop>.CreateAsync(workShops.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: WorkShops/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workShop = await _context.WorkShop
                .Include(w => w.Distributer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (workShop == null)
            {
                return NotFound();
            }

            return View(workShop);
        }

        // GET: WorkShops/Create
        public IActionResult Create()
        {
            ViewData["DistributerId"] = new SelectList(_context.Distributor, "Id", "DistributorName");
            return View();
        }

        // POST: WorkShops/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,WorkShopCode,WorkShopName,DistributerId,Address,Email,ContactPerson,MobileNumber,CreatedOn")] WorkShop workShop)
        {
            if (ModelState.IsValid)
            {
                workShop.Id = Guid.NewGuid();
                _context.Add(workShop);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DistributerId"] = new SelectList(_context.Distributor, "Id", "DistributorName", workShop.DistributerId);
            return View(workShop);
        }

        // GET: WorkShops/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workShop = await _context.WorkShop.FindAsync(id);
            if (workShop == null)
            {
                return NotFound();
            }
            ViewData["DistributerId"] = new SelectList(_context.Distributor, "Id", "DistributorName", workShop.DistributerId);
            return View(workShop);
        }

        // POST: WorkShops/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,WorkShopCode,WorkShopName,DistributerId,Address,Email,ContactPerson,MobileNumber,CreatedOn")] WorkShop workShop)
        {
            if (id != workShop.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(workShop);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WorkShopExists(workShop.Id))
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
            ViewData["DistributerId"] = new SelectList(_context.Distributor, "Id", "DistributorName", workShop.DistributerId);
            return View(workShop);
        }

        // GET: WorkShops/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workShop = await _context.WorkShop
                .Include(w => w.Distributer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (workShop == null)
            {
                return NotFound();
            }

            return View(workShop);
        }

        // POST: WorkShops/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var workShop = await _context.WorkShop.FindAsync(id);
            _context.WorkShop.Remove(workShop);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WorkShopExists(Guid id)
        {
            return _context.WorkShop.Any(e => e.Id == id);
        }
    }
}
