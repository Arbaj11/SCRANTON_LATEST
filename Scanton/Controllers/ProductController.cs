using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Scanton.Models;


namespace Scanton.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ProductController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        // GET: Product/Create
        public IActionResult Create()
        {
            var product = new Product();
            ViewBag.ParentCategories = _context.ParentCategories.ToList();
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.StoreProfile = _context.StoreProfiles.ToList();
            return View(product);
        }

        // POST: Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(Product product, IEnumerable<IFormFile> imageFile)
        {
            if (ModelState.IsValid)
            {
                
                if (imageFile != null && imageFile.Any())
                {

                    string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "images", "products");
                    Directory.CreateDirectory(uploadsFolder);

                    string galleryFolder = Path.Combine(uploadsFolder, "gallery");
                    Directory.CreateDirectory(galleryFolder);

                    List<string> filePaths = new List<string>();
                    int fileTracker = 0;
                    foreach (var file in imageFile)
                    {
                        if (file.Length > 0)
                        {
                            string uniqueFileName = Guid.NewGuid().ToString() + "_" +Path.GetFileName(file.FileName);
                            string filePath;

                            

                            string imagePath;

                            if (fileTracker > 0)
                            {
                                filePath = Path.Combine(galleryFolder, uniqueFileName);
                                imagePath = "/images/products/gallery/" + uniqueFileName;

                                  if (fileTracker == 1)
                                    product.SubImage_1 = imagePath;

                                else if (fileTracker == 2)
                                    product.SubImage_2 = imagePath;

                                else if (fileTracker == 3)
                                    product.SubImage_3 = imagePath;

                                else if (fileTracker == 4)
                                    product.SubImage_4 = imagePath;
                            }
                            else
                            {
                                filePath = Path.Combine(uploadsFolder, uniqueFileName);
                                imagePath = "/images/products/" + uniqueFileName;
                                if (fileTracker == 0)
                                {
                                    product.ImagePath = imagePath;
                                }
                                
                            }
                            using (var fileStream = new FileStream(filePath, FileMode.Create))
                            {
                                await file.CopyToAsync(fileStream);
                            }
                            fileTracker++;
                        }
                    }

                }
                var storeProfile= _context.StoreProfiles.FirstOrDefault(sn=>sn.Id == product.StoreProfileId);
                product.StoreName = storeProfile.StoreName;
                product.StoreZipCode = storeProfile.ZipCode;
                // Save the product (with selected categories)
                product.Created_Date = DateTime.UtcNow;
                product.Updated_Date = DateTime.UtcNow;
                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Product created successfully!";
                return RedirectToAction(nameof(List));
            }

            // Reload dropdowns in case of validation error
            ViewBag.ParentCategories = _context.ParentCategories.ToList();
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.StoreProfile = _context.StoreProfiles.ToList();
            return View(product);
        }
        // GET: Product/List

        [HttpGet("Products")]
        public IActionResult List()
        {
            var products = _context.Products;
            //.Include(p => p.ParentCategory)
            //.Include(p => p.Category)
            //.ToList();

            return View(products);
        }
        public IActionResult Edit(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (product == null) return NotFound();

            // Ensure ParentCategories, Categories, and StoreProfiles are not null or empty
            var parentCategories = _context.ParentCategories.ToList();
            var categories = _context.Categories.ToList();
            var storeProfiles = _context.StoreProfiles.ToList();

            if (parentCategories == null || categories == null || storeProfiles == null)
            {
                ModelState.AddModelError("", "Required data (Categories, Parent Categories, or Store Profiles) is missing.");
                return View(product);
            }

            // Populate dropdown lists
            ViewBag.ParentCategories = new SelectList(parentCategories, "Id", "Name", product.ParentCategoryId);
            ViewBag.Categories = new SelectList(categories, "Id", "Name", product.CategoryId);
            ViewBag.StoreProfiles = new SelectList(storeProfiles, "Id", "StoreName", product.StoreProfileId);

            // Store zip code logic
            var selectedStoreProfile = storeProfiles.FirstOrDefault(sp => sp.Id == product.StoreProfileId);
            if (selectedStoreProfile != null)
            {
                ViewBag.StoreProfileZipCode = selectedStoreProfile.ZipCode;
            }

            return View(product);
        }



        // POST: Product/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product model, IEnumerable<IFormFile> imageFile)
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
                    var product = _context.Products.FirstOrDefault(p => p.Id == id);
                    if (product == null) return NotFound();

                    // Update product fields
                    product.Name = model.Name;
                    product.Description = model.Description;
                    product.Price = model.Price;
                    product.MSP = model.MSP;
                    product.Weight = model.Weight;
                    product.PricePerPcs = model.PricePerPcs;
                    product.Stain = model.Stain;
                    product.Flavor = model.Flavor;
                    product.PackSize = model.PackSize;
                    product.ParentCategoryId = model.ParentCategoryId;
                    product.CategoryId = model.CategoryId;
                    product.JustArrivedProducts = model.JustArrivedProducts;
                    product.TittleBadge = model.TittleBadge;
                    product.ShortDescription = model.ShortDescription;

                    string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "images", "products");
                    Directory.CreateDirectory(uploadsFolder);

                    string galleryFolder = Path.Combine(_hostEnvironment.WebRootPath, "images", "products","gallery");
                    Directory.CreateDirectory(galleryFolder);


                    // Handle main product image
                    if (imageFile != null && imageFile.Any(f => f.Length > 0))
                    {
                        int fileTracker = 0;
                        foreach (var file in imageFile)
                        {
                            if (file.Length > 0)
                            {
                                string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);
                                string filePath;



                                // Save the image
                               
                                string imagePath;
                                if (fileTracker > 0)
                                {
                                    filePath = Path.Combine(galleryFolder, uniqueFileName);
                                    imagePath = "/images/products/gallery/" + uniqueFileName;

                                    if (!string.IsNullOrEmpty(imagePath))
                                    {
                                        if (fileTracker == 1)
                                            product.SubImage_1 = imagePath;

                                        else if (fileTracker == 2)
                                            product.SubImage_2 = imagePath;

                                        else if (fileTracker == 3)
                                            product.SubImage_3 = imagePath;

                                        else if (fileTracker == 4)
                                            product.SubImage_4 = imagePath;
                                    }

                                }
                                else
                                {
                                    filePath = Path.Combine(uploadsFolder, uniqueFileName);
                                    imagePath = "/images/products/" + uniqueFileName;
                                    if (fileTracker == 0)
                                    {
                                        product.ImagePath = imagePath;
                                    }

                                }
                                using (var fileStream = new FileStream(filePath, FileMode.OpenOrCreate))
                                {
                                    await file.CopyToAsync(fileStream);
                                }
                                fileTracker++;
                            }
                        }
                    }

                    // Update product's updated date
                    product.Updated_Date = DateTime.UtcNow;

                    // Save changes to the database
                    _context.Products.Update(product);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Product details updated successfully!";
                    return RedirectToAction("List");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"An error occurred: {ex.Message}");
                }
            }

            // Repopulate dropdown lists in case of validation errors
            ViewBag.ParentCategories = new SelectList(_context.ParentCategories, "Id", "Name", model.ParentCategoryId);
            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name", model.CategoryId);

            return View(model);
        }




        public IActionResult Activate(int id, bool isActive)
        {
            try
            {
                var product = _context.Products.FirstOrDefault(p => p.Id == id);
                if (product == null)
                {
                    return NotFound();
                }

                // Update the IsActive field
                product.BadgeIsActive = isActive;
                product.Updated_Date = DateTime.UtcNow;
                _context.Products.Update(product);
                _context.SaveChanges();

                TempData["SuccessMessage"] = isActive
                    ? "Product activated successfully!"
                    : "Product deactivated successfully!";

                // Redirect to the current listing page
                return RedirectToAction("List"); // Adjust if needed
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
                return RedirectToAction("List"); // Adjust if needed
            }
        }

        public IActionResult Delete(int id)
        {
            var product = _context.Products.Find(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
            return RedirectToAction("List");
        }

    }
}
