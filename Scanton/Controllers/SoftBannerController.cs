using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Scanton.Models;

namespace Scanton.Controllers
{
    public class SoftBannerController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly ILogger<SoftBanner> _logger;

        public SoftBannerController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment, ILogger<SoftBanner> logger)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            _logger = logger;
        }


        [HttpGet("SoftBanner")]
        public async Task<IActionResult> Index()
        {
            var softBanner = await _context.SoftBanner.ToListAsync();
            return View(softBanner);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SoftBanner softBanner, IEnumerable<IFormFile> imageFile)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null  && imageFile.Any())
                {

                    string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "images", "soft", "banner");
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


                            string ImagePath = "/images/soft/banner/" + uniqueFileName;

                            if (fileTracker == 0)
                                softBanner.Banner_Image_1 = ImagePath;
                            else if(fileTracker == 1)
                                softBanner.Banner_Image_2 = ImagePath;
                            else if (fileTracker == 2)
                                softBanner.Banner_Image_3 = ImagePath;

                            fileTracker++;
                        }
                    }

                }

                softBanner.Created_Date=DateTime.UtcNow;
                _context.SoftBanner.Add(softBanner);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(softBanner);
        }

        public IActionResult Activate(int id, bool isActive)
        {
            try
            {
                var softBanner = _context.SoftBanner.FirstOrDefault(p => p.Id == id);
                if (softBanner == null)
                {
                    return NotFound();
                }

                // Update the IsActive field
                softBanner.Is_Active = isActive;
                softBanner.Updated_Date = DateTime.UtcNow;
                _context.SoftBanner.Update(softBanner);
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
            var softBanner = _context.SoftBanner.Find(id);
            if (softBanner != null)
            {
                _context.SoftBanner.Remove(softBanner);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }


        public IActionResult Edit(int id)
        {
            var pageModel = _context.SoftBanner.FirstOrDefault(p => p.Id == id);
            if (pageModel == null) return NotFound();
            return View(pageModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async  Task<IActionResult> Edit(int id, SoftBanner model, IEnumerable<IFormFile> imageFile)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var softBanner = _context.SoftBanner.FirstOrDefault(p => p.Id == id);
                    if (softBanner == null) return NotFound();

                    // Update the fields


                    string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "images", "soft", "banner");
                    Directory.CreateDirectory(uploadsFolder);

                    List<string> filePaths = new List<string>();
                    int fileTracker = 0;

                    // Ensure imageFile is not null or empty
                    if (imageFile != null && imageFile.Any())
                    {
                        foreach (var file in imageFile)
                        {
                            if (file.Length > 0)
                            {
                                // Generate unique file name
                                string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);
                                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                                // Save the file
                                using (var fileStream = new FileStream(filePath, FileMode.Create))
                                {
                                    await file.CopyToAsync(fileStream);
                                }

                                // Generate the relative image path
                                string ImagePath = "/images/soft/banner/" + uniqueFileName;

                                // Assign to the appropriate banner property
                                if (fileTracker == 0)
                                    softBanner.Banner_Image_1 = ImagePath;
                                else if (fileTracker == 1)
                                    softBanner.Banner_Image_2 = ImagePath;
                                else if (fileTracker == 2)
                                    softBanner.Banner_Image_3 = ImagePath;

                                // Increment the tracker
                                fileTracker++;

                                // Log the image path for debugging
                                _logger.LogInformation($"File {fileTracker}: {ImagePath}");
                            }
                        }
                    }
                    else
                    {
                        _logger.LogWarning("No files were uploaded.");
                    }




                    softBanner.Updated_Date = DateTime.UtcNow;
                    _context.SoftBanner.Update(softBanner);
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
