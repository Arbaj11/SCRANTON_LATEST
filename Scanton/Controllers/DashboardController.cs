using Microsoft.AspNetCore.Mvc;

namespace Scanton.Controllers
{
    public class DashboardController : Controller
    {
        
        public IActionResult Index()
        {
            // Demo Data
            ViewBag.TotalUsers = 1250;
            ViewBag.TotalOrders = 300;
            ViewBag.TotalRevenue = 45000.75;
            ViewBag.PendingShipments = 25;

            return View();
        }
    }
}
