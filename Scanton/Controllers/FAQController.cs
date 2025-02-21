using Microsoft.AspNetCore.Mvc;
using Scanton.Models;

namespace Scanton.Controllers
{
    public class FAQController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FAQController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: FAQ/List
        [HttpGet("FAQ")]
        public IActionResult List()
        {
            var faqs = _context.FAQs.ToList();
            return View(faqs);
        }

        // GET: FAQ/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: FAQ/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(FAQ faq)
        {
            if (ModelState.IsValid)
            {
                faq.CreatedAt = DateTime.UtcNow;
                _context.FAQs.Add(faq);
                _context.SaveChanges();
                return RedirectToAction(nameof(List));
            }
            return View(faq);

        }

        public async Task<IActionResult> Delete(int id)
        {
            var faq = await _context.FAQs.FindAsync(id);
            if (faq == null)
                return NotFound();

            _context.FAQs.Remove(faq);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(List));
        }
    }
}
