using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantInfrastructure.Context;
using System.Threading;

namespace RestaurantInfrastructure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartsController : ControllerBase
    {
        private record CountByItemResponseItem(string Item, int Count);
        private record CountByCousineResponseItem(string Cousine, int Count);

        private readonly RestaurantDbContext _context;

        public ChartsController(RestaurantDbContext context)
        {
            _context = context;
        }

        [HttpGet("count-by-item")]
        public async Task<JsonResult> GetCountByItem(CancellationToken cancellationToken)
        {
            var responseItems = await _context
                .PreOrders
                .Include(x => x.MenuItem)
                .GroupBy(x => x.MenuItem.Name)
                .Select(group => new
                {
                    menuItemName = group.Key,
                    totalPreOrders = group.Sum(x => x.Quantity) ?? 0
                })
                .ToListAsync(cancellationToken);

            return new JsonResult(responseItems);
        }

        [HttpGet("menuitems-by-cousine")]
        public async Task<JsonResult> GetCountByCousineAsync(CancellationToken cancellationToken)
        {
            var responseItems = await _context.Cousines
                .Select(c => new
                {
                    cousineName = c.Name,
                    menuItemCount = c.MenuItems.Count()
                })
                .ToListAsync(cancellationToken);

            return new JsonResult(responseItems);
        }
    }
}
