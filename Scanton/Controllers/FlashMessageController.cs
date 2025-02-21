using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Scanton.Models;

namespace Scanton.Controllers
{
    public class FlashMessageController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FlashMessageController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("FlashMessages")]
        public async Task<IActionResult> Index()
        {
            var flashMessages = await _context.FlashMessages.ToListAsync();
            return View(flashMessages);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FlashMessage flashMessage, IFormFile ImageFile)
        {
            if (ModelState.IsValid)
            {
                if (ImageFile != null)
                {
                    string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/flashs");
                    Directory.CreateDirectory(uploadsFolder);
                    string uniqueFileName = Guid.NewGuid() + "_" + ImageFile.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(stream);
                    }

                    flashMessage.ImagePath = "/images/flashs/" + uniqueFileName;
                }

                _context.Add(flashMessage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(flashMessage);
        }


        public IActionResult Delete(int id)
        {
            var message = _context.FlashMessages.Find(id);
            if (message != null)
            {
                _context.FlashMessages.Remove(message);
                _context.SaveChanges();
                TempData["Success"] = "Flash message deleted successfully!";
            }
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var flashMessage = await _context.FlashMessages.FindAsync(id);
            if (flashMessage == null) return NotFound();

            return View(flashMessage);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, FlashMessage flashMessage,IFormFile imageFile)
        {
            if (id != flashMessage.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    if (imageFile != null)
                    {
                        string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot","images","flashs");
                        Directory.CreateDirectory(uploadsFolder);

                        List<string> filePaths = new List<string>();
                       
                        if (imageFile.Length > 0)
                        {
                            string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(imageFile.FileName);
                            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                            using (var fileStream = new FileStream(filePath, FileMode.Create))
                            {
                                await imageFile.CopyToAsync(fileStream);
                            }


                            string ImagePath = "/images/flashs/" + uniqueFileName;

                            flashMessage.ImagePath = ImagePath;
                        }

                    }
                    flashMessage.CreatedAt = DateTime.Now;
                    _context.FlashMessages.Update(flashMessage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.FlashMessages.Any(b => b.Id == id)) return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(flashMessage);
        }
    }
}
