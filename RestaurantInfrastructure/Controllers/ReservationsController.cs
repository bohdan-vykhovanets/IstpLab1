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
            var restaurantDbContext = _context.Reservations.Include(r => r.User).Include(r => r.Tables);
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

            DateTime now = DateTime.Now;
            now = now.AddMinutes(-now.Minute).AddSeconds(-now.Second);
            ViewBag.DefaultDate = now.ToString("yyyy-MM-ddTHH:00");

            return View(viewModel);
        }

        // POST: Reservations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReservationCreateViewModel viewModel)
        {
            //TimeSpan reservationDuration = TimeSpan.FromHours(2);
            //DateTime newReservationEnd = viewModel.ReservationDate.Add(reservationDuration);

            //var conflictingReservations = _context.Reservations
            //    .Where(r => r.ReservationDateStart < newReservationEnd &&
            //                r.ReservationDateStart + reservationDuration > viewModel.ReservationDate &&
            //                r.Tables.Any(t => viewModel.SelectedTableIds.Contains(t.Id)))
            //    .ToList();

            //if (conflictingReservations.Any())
            //{
            //    string conflictMessage = "Вибраний вам час пересікається з уже інсуючим бронюванням: " +
            //        string.Join(", ", conflictingReservations.Select(r => $"{r.ReservationDateStart:yyyy-MM-dd HH:mm} - {r.ReservationDateStart.AddHours(2):HH:mm}"));

            //    ModelState.AddModelError("ReservationDate", conflictMessage);

            //    viewModel.AvailableTables = await _context.Tables
            //        .Select(t => new SelectListItem { Value = t.Id.ToString(), Text = t.Id.ToString() })
            //        .ToListAsync();
            //    viewModel.AvailableMenuItems = await _context.MenuItems
            //        .Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.Name })
            //        .ToListAsync();

            //    return View(viewModel);
            //}

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
                UserId = viewModel.UserId,
                ReservationDateStart = viewModel.ReservationDateStart,
                ReservationDateEnd = viewModel.ReservationDateEnd,
                NumberOfGuests = viewModel.NumberOfGuests,
                SpecialRequests = viewModel.SpecialRequests,

                Tables = new List<Table>(),
                PreOrders = new List<PreOrder>()
            };

            var user = _context.Users.FirstOrDefault(u => u.Id == viewModel.UserId);
            if (user != null)
            {
                reservation.User = user;
            }

            if (viewModel.SelectedTableIds != null)
            {
                foreach (var tableId in viewModel.SelectedTableIds)
                {
                    var table = await _context.Tables.FindAsync(tableId);
                    if (table != null)
                    {
                        reservation.Tables.Add(table);
                        table.Availability = false;
                        //table.ReservationId = reservation.Id;
                    }
                }
            }

            foreach (var preOrderVm in viewModel.PreOrders)
            {
                var preOrder = new PreOrder
                {
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

            var reservation = await _context.Reservations
                .Include(r => r.Tables)
                .Include(r => r.PreOrders)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reservation == null)
            {
                return NotFound();
            }

            var viewModel = new ReservationEditViewModel
            {
                ReservationId = reservation.Id,
                UserId = reservation.UserId,
                ReservationDateStart = reservation.ReservationDateStart,
                ReservationDateEnd = reservation.ReservationDateEnd,
                NumberOfGuests = reservation.NumberOfGuests,
                SpecialRequests = reservation.SpecialRequests,
                SelectedTableIds = reservation.Tables.Select(t => t.Id).ToList(),
                PreOrders = reservation.PreOrders.Select(po => new PreOrderCreateViewModel
                {
                    MenuItemId = po.MenuItemId,
                    Quantity = po.Quantity
                }).ToList()
            };

            viewModel.AvailableTables = await _context.Tables
                .Select(t => new SelectListItem { Value = t.Id.ToString(), Text = t.Id.ToString() })
                .ToListAsync();

            viewModel.AvailableMenuItems = await _context.MenuItems
                .Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.Name })
                .ToListAsync();

            return View(viewModel);
        }

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ReservationEditViewModel viewModel)
        {
            if (id != viewModel.ReservationId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                viewModel.AvailableTables = await _context.Tables
                    .Select(t => new SelectListItem { Value = t.Id.ToString(), Text = t.Id.ToString() })
                    .ToListAsync();
                viewModel.AvailableMenuItems = await _context.MenuItems
                    .Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.Name })
                    .ToListAsync();
                return View(viewModel);
            }

            var reservation = await _context.Reservations
                .Include(r => r.Tables)
                .Include(r => r.PreOrders)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reservation == null)
            {
                return NotFound();
            }

            // Update scalar properties
            reservation.ReservationDateStart = viewModel.ReservationDateStart;
            reservation.ReservationDateEnd = viewModel.ReservationDateEnd;
            reservation.NumberOfGuests = viewModel.NumberOfGuests;
            reservation.SpecialRequests = viewModel.SpecialRequests;

            // Update tables: clear current selections and add new ones.
            reservation.Tables.Clear();
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

            // Update PreOrders: remove existing ones and add new ones based on the view model.
            _context.PreOrders.RemoveRange(reservation.PreOrders);
            reservation.PreOrders.Clear();
            foreach (var preOrderVm in viewModel.PreOrders)
            {
                var preOrder = new PreOrder
                {
                    MenuItemId = preOrderVm.MenuItemId,
                    Quantity = preOrderVm.Quantity
                };
                reservation.PreOrders.Add(preOrder);
            }

            try
            {
                _context.Update(reservation);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Reservations.Any(r => r.Id == reservation.Id))
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
            var reservation = await _context.Reservations
                .Include(r => r.PreOrders)
                .FirstOrDefaultAsync(r => r.Id == id);
            var tables = await _context.Tables
                .Where(t => t.ReservationId == id)
                .ToListAsync();

            if (reservation != null)
            {
                _context.PreOrders.RemoveRange(reservation.PreOrders);
                _context.Reservations.Remove(reservation);
                foreach (var table in tables)
                {
                    table.ReservationId = null;
                    table.Availability = true;
                }
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ReservationExists(int id)
        {
            return _context.Reservations.Any(e => e.Id == id);
        }

        [HttpGet("available-tables")]
        public async Task<IActionResult> GetAvailableTables(DateTime start, DateTime end, CancellationToken cancellationToken)
        {
            //start = DateTime.SpecifyKind(start, DateTimeKind.Local);
            //end = DateTime.SpecifyKind(end, DateTimeKind.Local);

            // Validate that times are whole hours and within 10:00 - 22:00 (you can add extra logic here).
            if (start.Minute != 0 || end.Minute != 0)
            {
                return BadRequest("Reservation times must be on whole hours.");
            }
            if (start.Hour < 10 || end.Hour > 22 || start.Date != end.Date)
            {
                return BadRequest("Reservations must be within the same day between 10:00 and 22:00.");
            }

            //start = start.ToUniversalTime();
            //end = end.ToUniversalTime();

            // Get tables that do not have any overlapping reservation.
            // Assumes each Reservation has a collection of Tables.
            // (Adjust the query if your relationship is modeled differently.)
            var availableTables = await _context.Tables
                .Where(t => !_context.Reservations
                    .Where(r => r.ReservationDateStart < end && r.ReservationDateEnd > start)
                    .Any(r => r.Tables.Any(x => x.Id == t.Id)))
                .ToListAsync(cancellationToken);

            // Return a simplified version of the table (e.g., ID, NumberOfSeats, and IsOutdoor)
            var result = availableTables.Select(t => new {
                t.Id,
                t.NumberOfSeats,
                t.IsOutdoor
            });
            return Json(result);
        }

    }
}
