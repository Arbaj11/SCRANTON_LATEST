using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scanton.Models;

namespace Scanton.Controllers
{
    public class VideoGalleryController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public VideoGalleryController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        [HttpGet("VideoGallery")]
        public async Task<IActionResult> Index()
        {
            var videos = await _context.VideoGallery.ToListAsync();
            return View(videos);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VideoGallery video, IFormFile videoFile)
        {
            if (ModelState.IsValid)
            {
                if (videoFile != null)
                {
                    string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "images" ,"videos");
                    Directory.CreateDirectory(uploadsFolder);

                    // Generate a unique filename for the video
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + videoFile.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // Save the video to the server
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await videoFile.CopyToAsync(fileStream);
                    }

                    // Assign the video path to the VideoGallery model
                    video.VideoPath = "/images/videos/" + uniqueFileName;
                }

                // Add the video object to the context and save
                _context.VideoGallery.Add(video);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(video);
        }


        public async Task<IActionResult> Delete(int id)
        {
            var video = await _context.VideoGallery.FindAsync(id);
            if (video == null)
                return NotFound();

            _context.VideoGallery.Remove(video);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        public IActionResult Activate(int id, bool isActive)
        {
            try
            {
                var video = _context.VideoGallery.FirstOrDefault(p => p.Id == id);
                if (video == null)
                {
                    return NotFound();
                }

                // Update the IsActive field
                video.Is_Active = isActive;

                _context.VideoGallery.Update(video);
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
