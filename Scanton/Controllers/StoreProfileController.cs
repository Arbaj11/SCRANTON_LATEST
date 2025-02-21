using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Scanton.Models;


namespace Scanton.Controllers
{
    public class StoreProfileController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public StoreProfileController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }



        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StoreProfile storeProfile, IFormFile ImageFile)
        {
            if (ModelState.IsValid)
            {
                if (ImageFile != null)
                {
                    string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "images", "store");
                    Directory.CreateDirectory(uploadsFolder);
                    string uniqueFileName = Guid.NewGuid() + "_" + ImageFile.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(stream);
                    }

                    storeProfile.ImagePath = "/images/store/" + uniqueFileName;
                }

                _context.StoreProfiles.Add(storeProfile);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(List));
            }
            return View(storeProfile);
        }

        [HttpGet("StoreProfile")]
        public IActionResult List()
        {
            var stores = _context.StoreProfiles.ToList();
            return View(stores);
        }



        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var store = await _context.StoreProfiles.FindAsync(id);
            if (store == null) return NotFound();

            return View(store);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, StoreProfile store, IFormFile imageFile)
        {
            if (id != store.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                // If ModelState is invalid, return the same view with the model to show validation errors
                return View(store);
            }

            try
            {
                if (imageFile != null)
                {
                    string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "images", "store");
                    Directory.CreateDirectory(uploadsFolder); // Ensures folder is created if it doesn't exist
                    string uniqueFileName = Guid.NewGuid() + "_" + imageFile.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }

                    store.ImagePath = "/images/store/" + uniqueFileName;
                }

                _context.Update(store);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.StoreProfiles.Any(b => b.Id == id)) // Ensure you check StoreProfiles
                {
                    return NotFound();
                }
                throw;
            }

            return RedirectToAction(nameof(List));
        }

    }
}
