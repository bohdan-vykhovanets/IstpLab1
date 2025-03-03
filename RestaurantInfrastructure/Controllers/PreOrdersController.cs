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
    public class PreOrdersController : Controller
    {
        private readonly RestaurantDbContext _context;

        public PreOrdersController(RestaurantDbContext context)
        {
            _context = context;
        }

        // GET: PreOrders
        public async Task<IActionResult> Index()
        {
            var restaurantDbContext = _context.PreOrders.Include(p => p.MenuItem).Include(p => p.Reservation);
            return View(await restaurantDbContext.ToListAsync());
        }

        // GET: PreOrders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var preOrder = await _context.PreOrders
                .Include(p => p.MenuItem)
                .Include(p => p.Reservation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (preOrder == null)
            {
                return NotFound();
            }

            return View(preOrder);
        }

        // GET: PreOrders/Create
        public IActionResult Create()
        {
            ViewData["MenuItemId"] = new SelectList(_context.MenuItems, "Id", "Id");
            ViewData["ReservationId"] = new SelectList(_context.Reservations, "Id", "Id");
            return View();
        }

        // POST: PreOrders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReservationId,MenuItemId,Quantity,Id")] PreOrder preOrder)
        {
            if (ModelState.IsValid)
            {
                _context.Add(preOrder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MenuItemId"] = new SelectList(_context.MenuItems, "Id", "Id", preOrder.MenuItemId);
            ViewData["ReservationId"] = new SelectList(_context.Reservations, "Id", "Id", preOrder.ReservationId);
            return View(preOrder);
        }

        // GET: PreOrders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var preOrder = await _context.PreOrders.FindAsync(id);
            if (preOrder == null)
            {
                return NotFound();
            }
            ViewData["MenuItemId"] = new SelectList(_context.MenuItems, "Id", "Id", preOrder.MenuItemId);
            ViewData["ReservationId"] = new SelectList(_context.Reservations, "Id", "Id", preOrder.ReservationId);
            return View(preOrder);
        }

        // POST: PreOrders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReservationId,MenuItemId,Quantity,Id")] PreOrder preOrder)
        {
            if (id != preOrder.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(preOrder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PreOrderExists(preOrder.Id))
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
            ViewData["MenuItemId"] = new SelectList(_context.MenuItems, "Id", "Id", preOrder.MenuItemId);
            ViewData["ReservationId"] = new SelectList(_context.Reservations, "Id", "Id", preOrder.ReservationId);
            return View(preOrder);
        }

        // GET: PreOrders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var preOrder = await _context.PreOrders
                .Include(p => p.MenuItem)
                .Include(p => p.Reservation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (preOrder == null)
            {
                return NotFound();
            }

            return View(preOrder);
        }

        // POST: PreOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var preOrder = await _context.PreOrders.FindAsync(id);
            if (preOrder != null)
            {
                _context.PreOrders.Remove(preOrder);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PreOrderExists(int id)
        {
            return _context.PreOrders.Any(e => e.Id == id);
        }
    }
}
