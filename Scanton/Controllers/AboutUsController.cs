using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualBasic.FileIO;
using Scanton.Models;
using System.Collections.Generic;

namespace Scanton.Controllers
{
    public class AboutUsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public AboutUsController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }


        [HttpGet("AboutUs")]
        public async Task<IActionResult> Index()
        {
            var aboutus = await _context.AboutUs.ToListAsync();
            return View(aboutus);
        }

        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AboutUs aboutUs, IEnumerable<IFormFile> imageFile, IEnumerable<string> fileType, IFormFile videoFile)
        {
            if (ModelState.IsValid)
            {
                string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "images", "aboutus");
                Directory.CreateDirectory(uploadsFolder);

                string productFolder = Path.Combine(uploadsFolder, "products");
                Directory.CreateDirectory(productFolder);

                // Handle image file uploads
                if (imageFile != null && imageFile.Any())
                {
                    var fileList = imageFile.Zip(fileType, (file, type) => new { File = file, Type = type });

                    foreach (var item in fileList)
                    {
                        if (item.File.Length > 0)
                        {
                            string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(item.File.FileName);
                            string filePath;
                            string imagePath;

                            if (item.Type == "Banner_Image")
                            {
                                filePath = Path.Combine(uploadsFolder, uniqueFileName);
                                imagePath = "/images/aboutus/" + uniqueFileName;
                                aboutUs.Banner_Image = imagePath;
                            }
                            else if (item.Type == "Product_Image1")
                            {
                                filePath = Path.Combine(productFolder, uniqueFileName);
                                imagePath = "/images/aboutus/products/" + uniqueFileName;
                                aboutUs.Product_Image1 = imagePath;
                            }
                            else if (item.Type == "Product_Image2")
                            {
                                filePath = Path.Combine(productFolder, uniqueFileName);
                                imagePath = "/images/aboutus/products/" + uniqueFileName;
                                aboutUs.Product_Image2 = imagePath;
                            }
                            else
                            {
                                continue; // Skip any unexpected file types
                            }

                            using (var fileStream = new FileStream(filePath, FileMode.Create))
                            {
                                await item.File.CopyToAsync(fileStream);
                            }
                        }
                    }
                }

                // Handle video file upload
                if (videoFile != null && videoFile.Length > 0)
                {
                    string videoFolder = Path.Combine(_hostEnvironment.WebRootPath, "images", "videos");
                    Directory.CreateDirectory(videoFolder);

                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(videoFile.FileName);
                    string videoFilePath = Path.Combine(videoFolder, uniqueFileName);

                    using (var fileStream = new FileStream(videoFilePath, FileMode.Create))
                    {
                        await videoFile.CopyToAsync(fileStream);
                    }

                    // Assign the video path to the AboutUs model
                    aboutUs.VideoPath = "/images/videos/" + uniqueFileName;
                }

                aboutUs.Created_Date = DateTime.UtcNow;
                _context.AboutUs.Add(aboutUs);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Product created successfully!";
                return RedirectToAction(nameof(Index));
            }

            return View(aboutUs);
        }


        public async Task<IActionResult> Edit(int id)
        {
            if (id == null) return NotFound();

            var aboutUs = await _context.AboutUs.FindAsync(id);
            if (aboutUs == null) return NotFound();


            return View(aboutUs);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AboutUs aboutUs, IFormFile? BannerImageFile, IFormFile? ProductImage1File, IFormFile? ProductImage2File, IFormFile? VideoFile)
        {
            try
            {
               
                if (id != aboutUs.Id)
                {
                    return NotFound();
                }

                
                var existingAboutUs = await _context.AboutUs.FindAsync(id);
                if (existingAboutUs == null)
                {
                    return NotFound();
                }

               
                if (!ModelState.IsValid)
                {
                    return View(existingAboutUs);
                }

                // Update fields from the form
                existingAboutUs.Banner_Title = aboutUs.Banner_Title;
                existingAboutUs.Banner_SubTitle = aboutUs.Banner_SubTitle;
                existingAboutUs.Product1_Title = aboutUs.Product1_Title;
                existingAboutUs.Product1_Description = aboutUs.Product1_Description;
                existingAboutUs.Product2_Title = aboutUs.Product2_Title;
                existingAboutUs.Product2_Description = aboutUs.Product2_Description;
                existingAboutUs.VideoTitle = aboutUs.VideoTitle;

                string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "images", "aboutus");
                Directory.CreateDirectory(uploadsFolder);

                string productFolder = Path.Combine(uploadsFolder, "products");
                Directory.CreateDirectory(productFolder);

                // Handle Banner Image file upload
                if (BannerImageFile != null && BannerImageFile.Length > 0)
                {
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(BannerImageFile.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await BannerImageFile.CopyToAsync(fileStream);
                    }

                    existingAboutUs.Banner_Image = "/images/aboutus/" + uniqueFileName;
                }

                // Handle Product Image 1 file upload
                if (ProductImage1File != null && ProductImage1File.Length > 0)
                {
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(ProductImage1File.FileName);
                    string filePath = Path.Combine(productFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await ProductImage1File.CopyToAsync(fileStream);
                    }

                    existingAboutUs.Product_Image1 = "/images/aboutus/products/" + uniqueFileName;
                }

                // Handle Product Image 2 file upload
                if (ProductImage2File != null && ProductImage2File.Length > 0)
                {
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(ProductImage2File.FileName);
                    string filePath = Path.Combine(productFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await ProductImage2File.CopyToAsync(fileStream);
                    }

                    existingAboutUs.Product_Image2 = "/images/aboutus/products/" + uniqueFileName;
                }

                // Handle Video file upload
                if (VideoFile != null && VideoFile.Length > 0)
                {
                    string videoFolder = Path.Combine(_hostEnvironment.WebRootPath, "images", "videos");
                    Directory.CreateDirectory(videoFolder);

                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(VideoFile.FileName);
                    string videoFilePath = Path.Combine(videoFolder, uniqueFileName);

                    using (var fileStream = new FileStream(videoFilePath, FileMode.Create))
                    {
                        await VideoFile.CopyToAsync(fileStream);
                    }

                    existingAboutUs.VideoPath = "/images/videos/" + uniqueFileName;
                }

                // Update the last modified date
                existingAboutUs.Updated_Date = DateTime.UtcNow;

                // Save changes to the database
                _context.AboutUs.Update(existingAboutUs);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Product updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                ModelState.AddModelError(string.Empty, "An error occurred while updating the product.");
            }

            return View(aboutUs);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var aboutUs = await _context.AboutUs.FindAsync(id);
            if (aboutUs == null)
                return NotFound();

            _context.AboutUs.Remove(aboutUs);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        public IActionResult Activate(int id, bool isActive)
        {
            try
            {
                var aboutUs = _context.AboutUs.FirstOrDefault(p => p.Id == id);
                if (aboutUs == null)
                {
                    return NotFound();
                }

                // Update the IsActive field
                aboutUs.Is_Active = isActive;

                _context.AboutUs.Update(aboutUs);
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
