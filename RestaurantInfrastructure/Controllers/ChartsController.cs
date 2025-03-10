using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantInfrastructure.Context;

namespace RestaurantInfrastructure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartsController : ControllerBase
    {
        private record CountByItemResponseItem(string Item, int Count);
        private record CountByCousineResponseItem(string Cousine, int Count);
        private readonly RestaurantDbContext _context;

        private ChartsController(RestaurantDbContext context)
        {
            _context = context;
        }

        [HttpGet("countByItem")]
        public async Task<JsonResult> GetCountByItemAsync(CancellationToken cancellationToken)
        {
            var responseItems = await _context
                .PreOrders
                .GroupBy(x => x.MenuItem.Name)
                .Select(group => new CountByItemResponseItem(group.Key.ToString(), group.Count()))
                .ToListAsync();

            return new JsonResult(responseItems);
        }

        public async Task<JsonResult> GetCountByCousinAsync(CancellationToken cancellationToken)
        {
            var responseItems = await _context
                .Cousines
                .GroupBy(x => x.Name)
                .Select(group => new CountByCousineResponseItem(group.Key.ToString(), group.Count()))
                .ToListAsync();

            return new JsonResult(responseItems);
        }
    }
}
