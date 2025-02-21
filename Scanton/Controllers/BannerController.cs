using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scanton.Models;

namespace Scanton.Controllers
{
    public class BannerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public BannerController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        [HttpGet("Banner")]
        public async Task<IActionResult> Index()
        {
            var banners = await _context.Banners.ToListAsync();
            return View(banners);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Banner banner, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null)
                {
                    string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "images/banners");
                    Directory.CreateDirectory(uploadsFolder);

                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(fileStream);
                    }

                    banner.ImagePath = "/images/banners/" + uniqueFileName;
                }

                _context.Banners.Add(banner);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(banner);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var banner = await _context.Banners.FindAsync(id);
            if (banner == null)
                return NotFound();

            _context.Banners.Remove(banner);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        public IActionResult Activate(int id, bool isActive)
        {
            try
            {
                var banner = _context.Banners.FirstOrDefault(p => p.Id == id);
                if (banner == null)
                {
                    return NotFound();
                }

                // Update the IsActive field
                banner.IsActive = isActive;
                
                _context.Banners.Update(banner);
                _context.SaveChanges();

                TempData["SuccessMessage"] = isActive
                    ? "Short Banner activated successfully!"
                    : "Short Banner deactivated successfully!";

                // Redirect to the current listing page
                return RedirectToAction("Index"); // Adjust if needed
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
                return RedirectToAction("Index"); // Adjust if needed
            }
        }
    }
}
