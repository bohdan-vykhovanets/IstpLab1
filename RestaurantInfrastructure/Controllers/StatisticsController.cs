using Microsoft.AspNetCore.Mvc;

namespace RestaurantInfrastructure.Controllers
{
    public class StatisticsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
