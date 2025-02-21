using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Scanton.Models;

namespace Scanton.Controllers
{
    public class ShortBannerController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly ILogger<SoftBanner> _logger;

        public ShortBannerController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment, ILogger<SoftBanner> logger)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            _logger = logger;
        }

        [HttpGet("ShortBanner")]
        public async Task<IActionResult> Index()
        {
            var softBanner = await _context.ShortBanner.ToListAsync();
            return View(softBanner);
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ShortBannner model, IEnumerable<IFormFile> imageFile)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null && imageFile.Any())
                {
                    string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "images", "short", "banner");
                    Directory.CreateDirectory(uploadsFolder);

                    List<string> filePaths = new List<string>();
                    int fileTracker = 0;
                    foreach (var file in imageFile)
                    {
                        if (file.Length > 0)
                        {
                            string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);
                            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                            using (var fileStream = new FileStream(filePath, FileMode.Create))
                            {
                                await file.CopyToAsync(fileStream);
                            }


                            string ImagePath = "/images/short/banner/" + uniqueFileName;

                             model.Short_Banner = ImagePath;
                        }
                    }
                }

                model.Created_Date = DateTime.UtcNow;
                _context.ShortBanner.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        public IActionResult Activate(int id, bool isActive)
        {
            try
            {
                var shortBannner = _context.ShortBanner.FirstOrDefault(p => p.Id == id);
                if (shortBannner == null)
                {
                    return NotFound();
                }

                // Update the IsActive field
                shortBannner.Is_Active = isActive;
                shortBannner.Updated_Date = DateTime.UtcNow;
                _context.ShortBanner.Update(shortBannner);
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

        public IActionResult Delete(int id)
        {
            var shortBannner = _context.ShortBanner.Find(id);
            if (shortBannner != null)
            {
                _context.ShortBanner.Remove(shortBannner);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }


        public IActionResult Edit(int id)
        {
            var shortModel = _context.ShortBanner.FirstOrDefault(p => p.Id == id);
            if (shortModel == null) return NotFound();
            return View(shortModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ShortBannner model, IFormFile imageFile)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }


            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage); // Or log to a file
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var shortModel = _context.ShortBanner.FirstOrDefault(p => p.Id == id);
                    if (shortModel == null) return NotFound();

                    shortModel.Short_Banner = model.Short_Banner;
                    shortModel.Sub_Tittle_Heading = model.Sub_Tittle_Heading;
                    shortModel.Heading_Tittle = model.Heading_Tittle;

                    if (imageFile != null )
                    {
                        string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "images", "short", "banner");
                        Directory.CreateDirectory(uploadsFolder);

                        List<string> filePaths = new List<string>();
                        int fileTracker = 0;
                            if (imageFile.Length > 0)
                            {
                                string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(imageFile.FileName);
                                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                                using (var fileStream = new FileStream(filePath, FileMode.Create))
                                {
                                    await imageFile.CopyToAsync(fileStream);
                                }


                                string ImagePath = "/images/short/banner/" + uniqueFileName;

                                shortModel.Short_Banner = ImagePath;
                            }
                        
                    }
                    shortModel.Updated_Date = DateTime.UtcNow;
                    _context.ShortBanner.Update(shortModel);
                    _context.SaveChanges();

                    TempData["SuccessMessage"] = "Product details updated successfully!";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"An error occurred: {ex.Message}");
                }
            }

           

            return View(model);
        }


    }
}
