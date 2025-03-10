using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RestaurantDomain.Models;
using RestaurantInfrastructure.Context;
using RestaurantInfrastructure.Models;

namespace RestaurantInfrastructure.Controllers
{
    public class ReservationsController : Controller
    {
        private readonly RestaurantDbContext _context;

        public ReservationsController(RestaurantDbContext context)
        {
            _context = context;
        }

        // GET: Reservations
        public async Task<IActionResult> Index()
        {
            var restaurantDbContext = _context.Reservations.Include(r => r.User);
            return View(await restaurantDbContext.ToListAsync());
        }

        // GET: Reservations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // GET: Reservations/Create
        public IActionResult Create()
        {
            var viewModel = new ReservationCreateViewModel
            {
                AvailableTables = _context.Tables
                    .Select(t => new SelectListItem { Value = t.Id.ToString(), Text = t.Id.ToString() })
                    .ToList(),
                AvailableMenuItems = _context.MenuItems
                    .Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.Name })
                    .ToList(),
                PreOrders = new List<PreOrderCreateViewModel>
                {
                    new PreOrderCreateViewModel()
                }
            };

            return View(viewModel);
        }

        // POST: Reservations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReservationCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.AvailableTables = await _context.Tables
                    .Select(t => new SelectListItem { Value = t.Id.ToString(), Text = t.Id.ToString()})
                    .ToListAsync();
                viewModel.AvailableMenuItems = await _context.MenuItems
                    .Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.Name })
                    .ToListAsync();
                return View(viewModel);
            }

            var reservation = new Reservation
            {
                ReservationDate = viewModel.ReservationDate,
                NumberOfGuests = viewModel.NumberOfGuests,
                SpecialRequests = viewModel.SpecialRequests,

                Tables = new List<Table>(),
                PreOrders = new List<PreOrder>()
            };

            if (viewModel.SelectedTableIds != null)
            {
                foreach (var tableId in viewModel.SelectedTableIds)
                {
                    var table = await _context.Tables.FindAsync(tableId);
                    if (table != null)
                    {
                        reservation.Tables.Add(table);
                    }
                }
            }

            foreach (var preOrderVm in viewModel.PreOrders)
            {
                var preOrder = new PreOrder
                {
                    ReservationId = preOrderVm.ReservationId,
                    MenuItemId = preOrderVm.MenuItemId,
                    Quantity = preOrderVm.Quantity

                };

                reservation.PreOrders.Add(preOrder);
            }

            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // GET: Reservations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", reservation.UserId);
            return View(reservation);
        }

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,ReservationDate,NumberOfGuests,SpecialRequests,Id")] Reservation reservation)
        {
            if (id != reservation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reservation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationExists(reservation.Id))
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
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", reservation.UserId);
            return View(reservation);
        }

        // GET: Reservations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation != null)
            {
                _context.Reservations.Remove(reservation);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservationExists(int id)
        {
            return _context.Reservations.Any(e => e.Id == id);
        }
    }
}
