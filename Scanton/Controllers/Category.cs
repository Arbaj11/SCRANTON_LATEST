using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Scanton.Models;


namespace Scanton.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public CategoryController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;

        }

        [HttpGet("Category")]
        public IActionResult Index()
        {
            var categories = _context.Categories.ToList();
            var parentCategories = _context.ParentCategories.ToList();
            ViewBag.ParentCategories = parentCategories;
            return View(categories);
        }

        [HttpPost]
        public async Task<IActionResult> Create(IFormFile imageFile, string name, string description, int parentCategoryId)
        {
            if (!string.IsNullOrWhiteSpace(name) && parentCategoryId > 0)
            {

                string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "images","category");
                Directory.CreateDirectory(uploadsFolder);

                string uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }
                var category = new Scanton.Models.Category
                {
                    Name = name,
                    Description = description,
                    ParentCategoryId = parentCategoryId,
                    ImagePath = "/images/category/" + uniqueFileName
                };
                _context.Categories.Add(category);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var category = _context.Categories.Find(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var category = _context.Categories.FirstOrDefault(p => p.Id == id);
            if (category == null) return NotFound();

            var parentCategories = _context.ParentCategories.ToList();
            ViewBag.ParentCategories = parentCategories;
            return View(category);
        }

        [HttpGet]
        public JsonResult GetCategoriesByParentId(int parentId)
        {
            var categories = _context.Categories
                                     .Where(c => c.ParentCategoryId == parentId)
                                     .Select(c => new { c.Id, c.Name })
                                     .ToList();
            return Json(categories);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category model, IFormFile imageFile)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }


            if (ModelState.IsValid)
            {
                try
                {
                    var category = _context.Categories.FirstOrDefault(p => p.Id == id);
                    if (category == null) return NotFound();

                    // Update the fields
                    category.Name = model.Name;
                    category.Description = model.Description;
                    category.ImagePath = model.ImagePath;
                    category.ParentCategoryId = model.ParentCategoryId;


                    if (imageFile != null && imageFile.Length > 0)
                    {
                        // Delete the old image if a new image is uploaded
                        if (!string.IsNullOrEmpty(category.ImagePath))
                        {
                            var oldImagePath = Path.Combine(_hostEnvironment.WebRootPath, category.ImagePath.TrimStart('/'));
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }

                        // Save the new image
                        string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "images", "category");
                        Directory.CreateDirectory(uploadsFolder);

                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(fileStream);
                        }



                        category.ImagePath = "/images/category/" + uniqueFileName; 
                    }


                    
                    category.CreatedAt = DateTime.UtcNow;
                    _context.Categories.Update(category);
                    _context.SaveChanges();

                    TempData["SuccessMessage"] = "Product details updated successfully!";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"An error occurred: {ex.Message}");
                }
            }

            // Repopulate dropdown lists in case of validation errors
            ViewBag.ParentCategories = new SelectList(_context.ParentCategories, "Id", "Name", model.ParentCategoryId);
            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name", model.Id);

            return View(model);
        }
    }

}
