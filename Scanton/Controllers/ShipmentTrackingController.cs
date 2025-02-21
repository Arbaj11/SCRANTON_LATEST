using Microsoft.AspNetCore.Mvc;
using Scanton.Models;


namespace Scranton.Controllers
{
    public class ShipmentTrackingController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ShipmentTrackingController(ApplicationDbContext context)
        {
            _context = context;
        }

        // List all shipments
        [HttpGet("ShipmentTracking")]
        public IActionResult Index()
        {
            var shipments = _context.ShipmentTrackings.ToList();
            return View(shipments);
        }


        // GET: ShipmentTracking/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: ShipmentTracking/Create
        [HttpPost]
        public IActionResult Create(ShipmentTracking shipment)
        {
            if (ModelState.IsValid)
            {
                _context.ShipmentTrackings.Add(shipment);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(shipment);
        }

    }
}
