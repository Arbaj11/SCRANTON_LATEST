using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Scanton.Models;

namespace Scanton.Controllers
{
    public class PageController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public PageController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        [HttpGet("admin-page-list")]
        public async Task<IActionResult> Index()
        {
            var pageModel = await _context.Page.ToListAsync();
            return View(pageModel);
        }

        public IActionResult Create()
        {
            var pageModel = new Page();
            return View(pageModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Page model)
        {
            if (ModelState.IsValid)
            {

                model.Created_date = DateTime.UtcNow;
                _context.Page.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        public IActionResult Activate(int id, bool isActive)
        {
            try
            {
                var pageModel = _context.Page.FirstOrDefault(p => p.CustomPageld == id);
                if (pageModel == null)
                {
                    return NotFound();
                }

                // Update the IsActive field
                pageModel.Is_Active = isActive;
                pageModel.Updated_date = DateTime.UtcNow;
                _context.Page.Update(pageModel);
                _context.SaveChanges();

                TempData["SuccessMessage"] = isActive
                    ? "Page activated successfully!"
                    : "Page deactivated successfully!";

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
            var pageModel = _context.Page.Find(id);
            if (pageModel != null)
            {
                _context.Page.Remove(pageModel);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var pageModel = _context.Page.FirstOrDefault(p => p.CustomPageld == id);
            if (pageModel == null) return NotFound();
            return View(pageModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Page model)
        {
            if (id != model.CustomPageld)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var pageModel = _context.Page.FirstOrDefault(p => p.CustomPageld == id);
                    if (pageModel == null) return NotFound();

                    // Update the fields
                    pageModel.CustomPageld = model.CustomPageld;
                    pageModel.CustomePageData = model.CustomePageData;
                    pageModel.CustomePageTittle = model.CustomePageTittle;
                    pageModel.PageUrl = model.PageUrl;



                    pageModel.Updated_date = DateTime.UtcNow;
                    _context.Page.Update(pageModel);
                    _context.SaveChanges();

                    TempData["SuccessMessage"] = "Page details updated successfully!";
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
