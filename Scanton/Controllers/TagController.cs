using Microsoft.AspNetCore.Mvc;
using Scanton.Models;

namespace Scanton.Controllers
{
    public class TagController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TagController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet("Tag")]
        public IActionResult Index()
        {
            var tags = _context.Tags.ToList();
            return View(tags);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Tag tag)
        {
            if (ModelState.IsValid)
            {
                _context.Tags.Add(tag);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(tag);
        }

        public IActionResult Edit(int id)
        {
            var tag = _context.Tags.Find(id);
            if (tag == null)
            {
                return NotFound();
            }
            return View(tag);
        }

        [HttpPost]
        public IActionResult Edit(Tag tag)
        {
            if (ModelState.IsValid)
            {
                _context.Tags.Update(tag);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(tag);
        }

        public IActionResult Delete(int id)
        {
            var tag = _context.Tags.Find(id);
            if (tag == null)
            {
                return NotFound();
            }
            _context.Tags.Remove(tag);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
