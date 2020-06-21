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
    public class DistributorsController : Controller
    {
        private readonly SuzukiDBContext _context;

        public DistributorsController(SuzukiDBContext context)
        {
            _context = context;
        }

        // GET: Distributors
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
            var suzukiDBContext = _context.Distributor.Include(d => d.Country);
            var distributor = from s in suzukiDBContext
                              select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                 distributor = suzukiDBContext.Where(s => s.DistributorName.Contains(searchString)
                                       || s.DistributorCode.Contains(searchString) || s.Address.Contains(searchString) || s.Email.Contains(searchString));
            }
            int pageSize = 10;
            return View(await PaginatedList<Distributor>.CreateAsync(distributor.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Distributors/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var distributor = await _context.Distributor
                .Include(d => d.Country)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (distributor == null)
            {
                return NotFound();
            }

            return View(distributor);
        }

        // GET: Distributors/Create
        public IActionResult Create()
        {
            ViewData["CountryId"] = new SelectList(_context.Country, "Id", "CountryName");
            return View();
        }

        // POST: Distributors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CountryId,DistributorCode,DistributorName,Address,Email,CreatedOn,Mobile")] Distributor distributor)
        {
            if (ModelState.IsValid)
            {
                distributor.Id = Guid.NewGuid();
                _context.Add(distributor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CountryId"] = new SelectList(_context.Country, "Id", "CountryName", distributor.CountryId);
            return View(distributor);
        }

        // GET: Distributors/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var distributor = await _context.Distributor.FindAsync(id);
            if (distributor == null)
            {
                return NotFound();
            }
            ViewData["CountryId"] = new SelectList(_context.Country, "Id", "CountryName", distributor.CountryId);
            return View(distributor);
        }

        // POST: Distributors/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,CountryId,DistributorCode,DistributorName,Address,Email,CreatedOn,Mobile")] Distributor distributor)
        {
            if (id != distributor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(distributor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DistributorExists(distributor.Id))
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
            ViewData["CountryId"] = new SelectList(_context.Country, "Id", "CountryName", distributor.CountryId);
            return View(distributor);
        }

        // GET: Distributors/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var distributor = await _context.Distributor
                .Include(d => d.Country)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (distributor == null)
            {
                return NotFound();
            }

            return View(distributor);
        }

        // POST: Distributors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var distributor = await _context.Distributor.FindAsync(id);
            _context.Distributor.Remove(distributor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DistributorExists(Guid id)
        {
            return _context.Distributor.Any(e => e.Id == id);
        }
    }
}
