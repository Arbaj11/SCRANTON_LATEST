using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scanton.Models;

namespace Scanton.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public OrderController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            var orders = await _context.OrderDetails
                               .Include(o => o.CustomerDetails)  
                               .ToListAsync();
            return View(orders);
        }


        public IActionResult Status(int id)
        {
            try
            {
                var orderModel = _context.OrderDetails.FirstOrDefault(p => p.OrderId == id);
                if (orderModel == null)
                {
                    return NotFound();
                }

                if(orderModel.OrderStatus == "Pending")
                {
                    orderModel.OrderStatus = "Complete";
                }
                else
                {
                    orderModel.OrderStatus = "Pending";
                }
                
              
                _context.OrderDetails.Update(orderModel);
                _context.SaveChanges();



                return Redirect("/Order/Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
                return RedirectToAction("Index");
            }
        }


        public IActionResult Processing(int id)
        {
            try
            {
                var orderModel = _context.OrderDetails.FirstOrDefault(p => p.OrderId == id);
                if (orderModel == null)
                {
                    return NotFound();
                }
                orderModel.OrderStatus = "Processing";
                _context.OrderDetails.Update(orderModel);
                _context.SaveChanges();
                return Redirect("/Order/Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        public IActionResult Delete(int id)
        {
            var orderModel = _context.OrderDetails.Find(id);
            if (orderModel != null)
            {
                _context.OrderDetails.Remove(orderModel);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
