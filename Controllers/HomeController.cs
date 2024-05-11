using chic_lighting.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace chic_lighting.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly chic_lightingContext _context;

        public HomeController(ILogger<HomeController> logger, chic_lightingContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var feedbacks = await _context.Feedbacks
                .Where(f => f.Rate == 3)
                .Select(f => new
                {
                    f.Name,
                    f.Comment
                })
                .Take(3)
                .ToListAsync();
            var categories = await _context.Categories
                .Where(c => c.IsActive == true)
                .Select(c => new
                {
                    c.Id,
                    c.CategoryName,
                    c.Description,
                    Image = c.Products
                    .Select(p => p.Image)
                    .FirstOrDefault()
                })
                .ToListAsync();
            ViewBag.Categories = categories;
            ViewBag.Feedback = feedbacks;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}