using chic_lighting.DTOs;
using chic_lighting.Models;
using Microsoft.AspNetCore.Mvc;

namespace chic_lighting.Controllers
{
    public class FeedbackController : Controller
    {
        private readonly chic_lightingContext _context;

        public FeedbackController(chic_lightingContext context)
        {
            _context= context;
        }
        [HttpGet]
        public async Task<IActionResult> Index(Dictionary<string, string> message)
        {
            ViewBag.Message = message;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Send(FeedbackDTO request)
        {
            var errorList = new Dictionary<string, string>();
            if (request.Name == null)
            {
                errorList["NullName"] = "* Name cannot be empty";
            }
            if (request.Email == null)
            {
                errorList["NullEmail"] = "* Email cannot be empty";
            }
            if (request.Rate == null)
            {
                errorList["NullRate"] = "* Rate cannot be empty";
            }
            if (request.Comment == null)
            {
                errorList["NullComment"] = "* Comment cannot be empty";
            }
            if (errorList.Count > 0)
            {
                return RedirectToAction("Index", errorList);
            }
            var newFeedback = new Feedback
            {
                Name = request.Name,
                Email = request.Email,
                Comment = request.Comment,
                Rate = short.Parse(request.Rate),
                CreatedAt = DateTime.Now,
            };
            await _context.AddAsync(newFeedback);
            await _context.SaveChangesAsync();
            var success = new Dictionary<string, string>();
            success["Success"] = "Your message has been sent. Thank you!";
            return RedirectToAction("Index", success);
        }
    }
}
