using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.DependencyResolver;
using RestaurantDomain.Models;
using RestaurantInfrastructure.Context;
using RestaurantInfrastructure.Models;

namespace RestaurantInfrastructure.Controllers
{
    public class MenuItemsController : Controller
    {
        private readonly RestaurantDbContext _context;

        public MenuItemsController(RestaurantDbContext context)
        {
            _context = context;
        }

        // GET: MenuItems
        public async Task<IActionResult> Index(int? id, string? name, int? type, string searchString)
        {
            ViewBag.Id = id;
            ViewBag.Name = name;
            ViewBag.Type = type;
            if (type == 1)
            {
                var itemByCategory = _context.MenuItems.Where(m => m.Categories.Any(c => c.Id == id));
                return View(await itemByCategory.ToListAsync());
            }
            if (type == 2)
            {
                var itemByCousine = _context.MenuItems.Where(m => m.Cousines.Any(c => c.Id == id));
                return View(await itemByCousine.ToListAsync());
            }
            if(type == null)
            {
                var items = from m in _context.MenuItems
                            select m;
                if (!string.IsNullOrEmpty(searchString))
                {
                    items = items.Where(s => s.Name.Contains(searchString)).Concat(items.Where(s => s.Description.Contains(searchString)));
                }
                return View(await items.ToListAsync());
            }
            else
            {
                return RedirectToAction("Index", "Categories");
            }
        }

        // GET: MenuItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menuItem = await _context.MenuItems
                .Include(m => m.Categories)
                .Include(m => m.Cousines)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (menuItem == null)
            {
                return NotFound();
            }

            return View(menuItem);
        }

        // GET: MenuItems/Create
        public IActionResult Create()
        {
            var viewModel = new MenuItemCreateViewModel
            {
                AvailableCategories = _context.Categories
                    .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
                    .ToList(),
                AvailableCousines = _context.Cousines
                    .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
                    .ToList()
            };

            return View(viewModel);
        }

        // POST: MenuItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( MenuItemCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var menuItem = new MenuItem
                {
                    Name = viewModel.Name,
                    Description = viewModel.Description,
                    Price = viewModel.Price,
                    ImageUrl = viewModel.ImageUrl,
                    Availability = viewModel.Availability,
                };

                if (viewModel.SelectedCategoryIds != null)
                {
                    foreach (var categoryId in viewModel.SelectedCategoryIds)
                    {
                        var category = await _context.Categories.FindAsync(categoryId);
                        if (category != null)
                        {
                            menuItem.Categories.Add(category);
                        }
                    }
                }

                if (viewModel.SelectedCousineIds != null)
                {
                    foreach (var cousineId in viewModel.SelectedCousineIds)
                    {
                        var cousine = await _context.Cousines.FindAsync(cousineId);
                        if (cousine != null)
                        {
                            menuItem.Cousines.Add(cousine);
                        }
                    }
                }

                _context.MenuItems.Add(menuItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }

            viewModel.AvailableCategories = _context.Categories
                .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
                .ToList();

            viewModel.AvailableCousines = _context.Cousines
                .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
                .ToList();

            return View(viewModel);
        }

        // GET: MenuItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menuItem = await _context.MenuItems
                .Include(m => m.Categories)
                .Include(m => m.Cousines)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (menuItem == null)
            {
                return NotFound();
            }

            var viewModel = new MenuItemEditViewModel
            {
                Id = menuItem.Id,
                Name = menuItem.Name,
                Price = menuItem.Price,
                Description = menuItem.Description,
                ImageUrl = menuItem.ImageUrl,
                Availability = menuItem.Availability,
                SelectedCategoryIds = menuItem.Categories.Select(c => c.Id).ToList(),
                SelectedCousineIds = menuItem.Cousines.Select(c => c.Id).ToList(),

                AvailableCategories = _context.Categories
                    .Select(c => new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.Name
                    })
                    .ToList(),
                AvailableCousines = _context.Cousines
                    .Select(c => new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.Name
                    })
            };

            return View(viewModel);
        }

        // POST: MenuItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MenuItemEditViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                viewModel.AvailableCategories = _context.Categories
                     .Select(c => new SelectListItem
                     {
                         Value = c.Id.ToString(),
                         Text = c.Name
                     })
                     .ToList();
                viewModel.AvailableCousines = _context.Cousines
                    .Select(c => new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.Name
                    })
                    .ToList();

                return View(viewModel);
            }

            var menuItem = await _context.MenuItems
                .Include(c => c.Categories)
                .Include(c => c.Cousines)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (menuItem == null)
            {
                return NotFound();
            }

            menuItem.Name = viewModel.Name;
            menuItem.Description = viewModel.Description;
            menuItem.Price = viewModel.Price;
            menuItem.ImageUrl = viewModel.ImageUrl;
            menuItem.Availability = viewModel.Availability;

            menuItem.Categories.Clear();
            if (viewModel.SelectedCategoryIds != null)
            {
                foreach (var categoryId in viewModel.SelectedCategoryIds)
                {
                    var category = await _context.Categories.FindAsync(categoryId);
                    if (category != null)
                    {
                        menuItem.Categories.Add(category);
                    }
                }
            }

            menuItem.Cousines.Clear();
            if (viewModel.SelectedCousineIds != null)
            {
                foreach (var cousineId in viewModel.SelectedCousineIds)
                {
                    var cousine = await _context.Cousines.FindAsync(cousineId);
                    if (cousine != null)
                    {
                        menuItem.Cousines.Add(cousine);
                    }
                }
            }

            try
            {
                _context.Update(menuItem);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.MenuItems.Any(e => e.Id == menuItem.Id))
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

        // GET: MenuItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menuItem = await _context.MenuItems
                .Include(m => m.Categories)
                .Include(m => m.Cousines)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (menuItem == null)
            {
                return NotFound();
            }

            return View(menuItem);
        }

        // POST: MenuItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var menuItem = await _context.MenuItems
                .Include (m => m.Categories)
                .Include (m => m.Cousines)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (menuItem != null)
            {
                menuItem.Categories.Clear();
                menuItem.Cousines.Clear();

                _context.MenuItems.Remove(menuItem);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool MenuItemExists(int id)
        {
            return _context.MenuItems.Any(e => e.Id == id);
        }
    }
}
