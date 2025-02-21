using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scanton.Models;

namespace Scanton.Controllers
{
    public class IndexController : Controller
    {
        private readonly ApplicationDbContext _context;

        public IndexController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet("BannerList")]
        public IActionResult BannerList()
        {
            var banners = _context.Banners.ToList();
            return View(banners);
        }
        public async Task<IActionResult> Index()
        {
            var banners = await _context.Banners.ToListAsync();
            var viewModel = new IndexViewModel
            {
                Banners = banners.Select(b => new IndexViewModel.Banner
                {
                    ImagePath = b.ImagePath,
                    Title = b.Title,

                }).ToList()
            };

            return View(viewModel);
        }
    }
}
