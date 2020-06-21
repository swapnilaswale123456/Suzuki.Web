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
    public class WorkShopLocationsController : Controller
    {
        private readonly SuzukiDBContext _context;

        public WorkShopLocationsController(SuzukiDBContext context)
        {
            _context = context;
        }

        // GET: WorkShopLocations
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
            var suzukiDBContext = _context.WorkShopLocation.Include(w => w.WorkShop);
            var workShopLocations = from s in suzukiDBContext
                              select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                workShopLocations = suzukiDBContext.Where(s => s.LocationName.Contains(searchString)
                                      || s.LocationCode.Contains(searchString) || s.Address.Contains(searchString) || s.Email.Contains(searchString));
            }
            int pageSize = 10;
            return View(await PaginatedList<WorkShopLocation>.CreateAsync(workShopLocations.AsNoTracking(), pageNumber ?? 1, pageSize));
        }
        // GET: WorkShopLocations/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workShopLocation = await _context.WorkShopLocation
                .Include(w => w.WorkShop)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (workShopLocation == null)
            {
                return NotFound();
            }

            return View(workShopLocation);
        }

        // GET: WorkShopLocations/Create
        public IActionResult Create()
        {
            ViewData["WorkShopId"] = new SelectList(_context.WorkShop, "Id", "WorkShopName");
            return View();
        }

        // POST: WorkShopLocations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,WorkShopId,LocationCode,LocationName,Address,Email,ContactPerson,MobileNumber,CreatedOn")] WorkShopLocation workShopLocation)
        {
            if (ModelState.IsValid)
            {
                workShopLocation.Id = Guid.NewGuid();
                _context.Add(workShopLocation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["WorkShopId"] = new SelectList(_context.WorkShop, "Id", "WorkShopName", workShopLocation.WorkShopId);
            return View(workShopLocation);
        }

        // GET: WorkShopLocations/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workShopLocation = await _context.WorkShopLocation.FindAsync(id);
            if (workShopLocation == null)
            {
                return NotFound();
            }
            ViewData["WorkShopId"] = new SelectList(_context.WorkShop, "Id", "WorkShopName", workShopLocation.WorkShopId);
            return View(workShopLocation);
        }

        // POST: WorkShopLocations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,WorkShopId,LocationCode,LocationName,Address,Email,ContactPerson,MobileNumber,CreatedOn")] WorkShopLocation workShopLocation)
        {
            if (id != workShopLocation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(workShopLocation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WorkShopLocationExists(workShopLocation.Id))
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
            ViewData["WorkShopId"] = new SelectList(_context.WorkShop, "Id", "WorkShopName", workShopLocation.WorkShopId);
            return View(workShopLocation);
        }

        // GET: WorkShopLocations/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workShopLocation = await _context.WorkShopLocation
                .Include(w => w.WorkShop)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (workShopLocation == null)
            {
                return NotFound();
            }

            return View(workShopLocation);
        }

        // POST: WorkShopLocations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var workShopLocation = await _context.WorkShopLocation.FindAsync(id);
            _context.WorkShopLocation.Remove(workShopLocation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WorkShopLocationExists(Guid id)
        {
            return _context.WorkShopLocation.Any(e => e.Id == id);
        }
    }
}
