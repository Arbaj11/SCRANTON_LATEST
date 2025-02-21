using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scanton.Models;
using System.Collections.Generic;

namespace Scanton.Controllers
{
    public class SocialMediaController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public SocialMediaController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        [HttpGet("SocialMedia")]
        public async Task<IActionResult> Index()
        {
            var socialMedia = await _context.SocialMedia.ToListAsync();
            return View(socialMedia);
        }

        public IActionResult Create()
        {
           return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SocialMedia model)
        {
            if (ModelState.IsValid)
            {

                model.Created_Date = DateTime.UtcNow;
                _context.SocialMedia.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }


        public IActionResult Activate(int id, bool isActive)
        {
            try
            {
                var socialMedia = _context.SocialMedia.FirstOrDefault(p => p.AccountId == id);
                if (socialMedia == null)
                {
                    return NotFound();
                }

                // Update the IsActive field
                socialMedia.Is_Active = isActive;
                socialMedia.Updated_Date = DateTime.UtcNow;
                _context.SocialMedia.Update(socialMedia);
                _context.SaveChanges();

                TempData["SuccessMessage"] = isActive
                    ? "Social Media activated successfully!"
                    : "Social Media deactivated successfully!";

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
            var socialMedia = _context.SocialMedia.Find(id);
            if (socialMedia != null)
            {
                _context.SocialMedia.Remove(socialMedia);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var socialMedia = await _context.SocialMedia.FindAsync(id);
            if (socialMedia == null) return NotFound();

            return View(socialMedia);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SocialMedia social)
        {
            if (id != social.AccountId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    social.Is_Active = true;
                    social.Updated_Date = DateTime.Now;
                    _context.SocialMedia.Update(social);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.SocialMedia.Any(b => b.AccountId == id)) return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(social);
        }
    }
}

