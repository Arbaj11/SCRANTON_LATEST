using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Scanton.Models;

namespace Scanton.Controllers
{
    public class FooterDescriptionController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public FooterDescriptionController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        [HttpGet("FooterDescription")]
        public async Task<IActionResult> Index()
        {
            var footer =await _context.FooterDescription.ToListAsync();
            return View(footer);
        }

        public IActionResult Create()
        {
            var model = new FooterDescription();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FooterDescription model)
        {
            if (ModelState.IsValid)
            {
               
                model.Created_Date = DateTime.UtcNow;
                _context.FooterDescription.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        public IActionResult Activate(int id, bool isActive)
        {
            try
            {
                var footer = _context.FooterDescription.FirstOrDefault(p => p.Id == id);
                if (footer == null)
                {
                    return NotFound();
                }

                // Update the IsActive field
                footer.Is_Active = isActive;
                footer.Updated_Date = DateTime.UtcNow;
                _context.FooterDescription.Update(footer);
                _context.SaveChanges();

                TempData["SuccessMessage"] = isActive
                    ? "Soft Banner activated successfully!"
                    : "Soft Banner deactivated successfully!";

                // Redirect to the current listing page
                return RedirectToAction("Index"); // Adjust if needed
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
                return RedirectToAction("Index"); // Adjust if needed
            }
        }

        public IActionResult Delete(int id)
        {
            var footer = _context.FooterDescription.Find(id);
            if (footer != null)
            {
                _context.FooterDescription.Remove(footer);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var footer = await _context.FooterDescription.FindAsync(id);
            if (footer == null) return NotFound();

            return View(footer);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, FooterDescription footer)
        {
            if (id != footer.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    footer.Updated_Date = DateTime.Now;
                    _context.FooterDescription.Update(footer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.FooterDescription.Any(b => b.Id == id)) return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(footer);
        }
    }
}
