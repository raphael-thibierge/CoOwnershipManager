using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CoOwnershipManager.Data;
using Microsoft.AspNetCore.Authorization;

namespace CoOwnershipManager.Controllers.Admin
{
    [Authorize]
    [Authorize(Policy = "IsAdmin")]
    public class BuildingAdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BuildingAdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: BuildingAdmin
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Buildings.Include(b => b.Address);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: BuildingAdmin/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var building = await _context.Buildings
                .Include(b => b.Address)
                .Include(b => b.Apartments)
                .ThenInclude(a => a.Unhabitants)
                .Include(b => b.Posts)
                .ThenInclude(p => p.Author)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (building == null)
            {
                return NotFound();
            }

            return View(building);
        }

        // GET: BuildingAdmin/Create
        public IActionResult Create()
        {
            ViewData["AddressId"] = new SelectList(_context.Addresses, "Id", "Id");
            return View();
        }

        // POST: BuildingAdmin/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,AddressId")] Building building)
        {
            if (ModelState.IsValid)
            {
                _context.Add(building);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AddressId"] = new SelectList(_context.Addresses, "Id", "Id", building.AddressId);
            return View(building);
        }

        // GET: BuildingAdmin/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var building = await _context.Buildings.FindAsync(id);
            if (building == null)
            {
                return NotFound();
            }
            ViewData["AddressId"] = new SelectList(_context.Addresses, "Id", "Id", building.AddressId);
            return View(building);
        }

        // POST: BuildingAdmin/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,AddressId")] Building building)
        {
            if (id != building.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(building);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BuildingExists(building.Id))
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
            ViewData["AddressId"] = new SelectList(_context.Addresses, "Id", "Id", building.AddressId);
            return View(building);
        }

        // GET: BuildingAdmin/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var building = await _context.Buildings
                .Include(b => b.Address)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (building == null)
            {
                return NotFound();
            }

            return View(building);
        }

        // POST: BuildingAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var building = await _context.Buildings.FindAsync(id);
            _context.Buildings.Remove(building);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BuildingExists(int id)
        {
            return _context.Buildings.Any(e => e.Id == id);
        }
    }
}
