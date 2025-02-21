using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Scanton.Models;

namespace Scanton.Controllers
{
    public class ParentCategoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ParentCategoryController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        [HttpGet("ParentCategory")]
        public IActionResult Index()
        {
            var parentCategories = _context.ParentCategories.ToList();
            return View(parentCategories);
        }

        [HttpPost]
        public async Task<IActionResult> Create(string name, string description, IFormFile imageFile)
        {
            
            if (!string.IsNullOrWhiteSpace(name))
            {
                if (imageFile != null)
                {
                    string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "images", "parent", "category");
                    Directory.CreateDirectory(uploadsFolder);

                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(fileStream);
                    }

                    var parentCategory = new ParentCategory { Name = name, Description = description, ImagePath = "/images/parent/category/" + uniqueFileName };
                    _context.ParentCategories.Add(parentCategory);
                    _context.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var parentCategory = _context.ParentCategories.Find(id);
            if (parentCategory != null)
            {
                _context.ParentCategories.Remove(parentCategory);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var category = await _context.ParentCategories.FindAsync(id);
            if (category == null) return NotFound();

            return View(category);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string name, string description, IFormFile imageFile)
        {
            var parentCategory = _context.ParentCategories.Find(id);

            if (parentCategory != null)
            {
                if (imageFile != null)
                {
                    string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "images", "parent", "category");
                    Directory.CreateDirectory(uploadsFolder);

                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(fileStream);
                    }
                    parentCategory.ImagePath = "/images/parent/category/" + uniqueFileName;
                    parentCategory.Name = name;
                    parentCategory.Description = description;
                    _context.SaveChanges();

                }


               
            }
            return RedirectToAction("Index");
        }
    }

}
