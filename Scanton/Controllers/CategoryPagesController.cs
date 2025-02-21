using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scanton.Models;
using static Scanton.Models.IndexViewModel;

namespace Scanton.Controllers
{
    public class CategoryPagesController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public CategoryPagesController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }


        [Route("category-products/{category_id}")]
        public async Task<IActionResult> Index(int category_id, int pageNumber = 1, int pageSize = 10, string searchQuery = "")
        {
            try
            {
                var footersIcon = await _context.SocialMedia.ToListAsync();
                var footerPages = await _context.Page.ToListAsync();
                var footersDescription = await _context.FooterDescription.ToListAsync();
                var products = await _context.Products.ToListAsync();
                var CategoryDetails = await _context.Products.FirstOrDefaultAsync(p => p.CategoryId == category_id);
                var categories = await _context.Categories.ToListAsync();
                var categoriesNameByID = await _context.Categories.ToListAsync();

                if (CategoryDetails == null)
                {
                    
                    return RedirectToAction("ErrorCategory", "CategoryPages"); // Replace "Index" with the name of your target view/action
                }

                var filteredProducts = products.Where(p => p.CategoryId == CategoryDetails.CategoryId).ToList();
                var tenDaysAgo = DateTime.UtcNow.AddDays(-50);
                var justArrived = filteredProducts.Where(p => p.Created_Date >= tenDaysAgo).ToList();

                if (!string.IsNullOrEmpty(searchQuery))
                {
                    filteredProducts = filteredProducts.Where(p => p.Name.Contains(searchQuery, StringComparison.OrdinalIgnoreCase)).ToList();
                    justArrived = justArrived.Where(p => p.Name.Contains(searchQuery, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                var totalItems = filteredProducts.Count;
                var totalArriveItems = justArrived.Count;

                // Paginate the filtered products
                var paginatedProducts = filteredProducts
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                // Paginate the just arrived products
                var paginatedArrivedProducts = justArrived
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                var viewModel = new IndexViewModel
                {
                    Products = paginatedProducts.Select(p => new IndexViewModel.Product
                    {
                        Id = p.Id,
                        Name = p.Name,
                        ImagePath = p.ImagePath,
                        SubImage_1=p.SubImage_1,
                        SubImage_2=p.SubImage_2,
                        SubImage_3=p.SubImage_3,
                        SubImage_4=p.SubImage_4,
                        TittleBadge = p.TittleBadge,
                        PriceWithDiscount = p.PriceWithDiscount,
                        Price = p.Price,
                        Flavor = p.Flavor,
                        BadgeIsActive = p.BadgeIsActive,
                        FeaturedProducts = p.FeaturedProducts,
                        CategoryName = categoriesNameByID.FirstOrDefault(c => c.Id == CategoryDetails.CategoryId)?.Name ?? "Unknown"
                    }).ToList(),
                    JustArrivedProducts = paginatedArrivedProducts.Select(p => new IndexViewModel.Product
                    {
                        Id = p.Id,
                        Name = p.Name,
                        ImagePath = p.ImagePath,
                        SubImage_1 = p.SubImage_1,
                        SubImage_2 = p.SubImage_2,
                        SubImage_3 = p.SubImage_3,
                        SubImage_4 = p.SubImage_4,
                        TittleBadge = p.TittleBadge,
                        PriceWithDiscount = p.PriceWithDiscount,
                        Price = p.Price,
                        Flavor = p.Flavor,
                        BadgeIsActive = p.BadgeIsActive,
                        FeaturedProducts = p.FeaturedProducts,
                        CategoryName = categoriesNameByID.FirstOrDefault(c => c.Id == CategoryDetails.CategoryId)?.Name ?? "Unknown"
                    }).ToList()
                };
                ViewBag.Category_ID = category_id;
                ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
                ViewBag.TotalArrivedPages = (int)Math.Ceiling(totalArriveItems / (double)pageSize);
                ViewBag.CurrentPage = pageNumber;
                ViewBag.FooterIcon = footersIcon.Select(s => new IndexViewModel.SocialMedia { AccountIcon = s.AccountIcon, AccountName = s.AccountURLLink, AccountURLLink = s.AccountURLLink, Is_Active = s.Is_Active }).ToList();
                ViewBag.FooterPage = footerPages.Select(p => new IndexViewModel.Page { CustomePageData = p.CustomePageData, CustomePageTittle = p.CustomePageTittle, PageCategory = p.PageCategory, PageUrl = p.PageUrl, Is_Active = p.Is_Active }).ToList();
                ViewBag.FooterDescription = footersDescription.Select(f => new IndexViewModel.FooterDescription { Title = f.Title, Description = f.Description, Is_Active = f.Is_Active }).ToList();
                ViewBag.SearchQuery = searchQuery;

                return View(viewModel);
            }
            catch (Exception ex)
            {
                // Handle error appropriately
                throw;
            }
        }

        public IActionResult ErrorCategory()
        {
            return View();
        }


    }
}
