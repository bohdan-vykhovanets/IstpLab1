using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RestaurantDomain.Models;
using RestaurantInfrastructure.Context;

namespace RestaurantInfrastructure.Controllers
{
    public class CousinesController : Controller
    {
        private readonly RestaurantDbContext _context;

        public CousinesController(RestaurantDbContext context)
        {
            _context = context;
        }

        // GET: Cousines
        public async Task<IActionResult> Index()
        {
            return View(await _context.Cousines.ToListAsync());
        }

        // GET: Cousines/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cousine = await _context.Cousines
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cousine == null)
            {
                return NotFound();
            }

            //return View(cousine);
            return RedirectToAction("Index", "MenuItems", new { id = cousine.Id, name = cousine.Name, type = 2 });
        }

        // GET: Cousines/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Cousines/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Id")] Cousine cousine)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cousine);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cousine);
        }

        // GET: Cousines/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cousine = await _context.Cousines.FindAsync(id);
            if (cousine == null)
            {
                return NotFound();
            }
            return View(cousine);
        }

        // POST: Cousines/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,Id")] Cousine cousine)
        {
            if (id != cousine.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cousine);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CousineExists(cousine.Id))
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
            return View(cousine);
        }

        // GET: Cousines/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cousine = await _context.Cousines
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cousine == null)
            {
                return NotFound();
            }

            return View(cousine);
        }

        // POST: Cousines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cousine = await _context.Cousines.FindAsync(id);
            if (cousine != null)
            {
                _context.Cousines.Remove(cousine);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CousineExists(int id)
        {
            return _context.Cousines.Any(e => e.Id == id);
        }
    }
}
