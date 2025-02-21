using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scanton.Models;

namespace Scanton.Controllers
{
    public class COAController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public COAController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        [HttpGet("coa-list")]
        public async Task<IActionResult> Index()
        {
            var coa = await _context.COA.ToListAsync();
            return View(coa);
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(COA model, IFormFile Product_image, IFormFile Pdf_Path)
        {
            if (ModelState.IsValid)
            {
                // Process Product Image
                if (Product_image != null)
                {
                    string imageUploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "images", "COA", "products");
                    Directory.CreateDirectory(imageUploadsFolder); // Ensure directory exists

                    string uniqueImageFileName = Guid.NewGuid().ToString() + "_" + Product_image.FileName;
                    string imageFilePath = Path.Combine(imageUploadsFolder, uniqueImageFileName);

                    using (var fileStream = new FileStream(imageFilePath, FileMode.Create))
                    {
                        await Product_image.CopyToAsync(fileStream);
                    }

                    model.Product_image = "/images/COA/products/" + uniqueImageFileName; // Save relative path
                }

                // Process PDF Upload
                if (Pdf_Path != null)
                {
                    string pdfUploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "images", "COA", "Pdf");
                    Directory.CreateDirectory(pdfUploadsFolder); // Ensure directory exists

                    string uniquePdfFileName = Guid.NewGuid().ToString() + "_" + Pdf_Path.FileName;
                    string pdfFilePath = Path.Combine(pdfUploadsFolder, uniquePdfFileName);

                    using (var fileStream = new FileStream(pdfFilePath, FileMode.Create))
                    {
                        await Pdf_Path.CopyToAsync(fileStream);
                    }

                    model.Pdf_Path = "/images/COA/pdf/" + uniquePdfFileName; // Save relative path
                }

                // Save data to database
                model.Created_Date = DateTime.UtcNow;
                _context.COA.Add(model);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            // If validation fails, return the model back to the view
            return View(model);
        }

    }
}
