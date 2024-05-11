using Microsoft.AspNetCore.Mvc;

namespace chic_lighting.Controllers
{
    public class ErrorController : Controller
    {
        public async Task<IActionResult> Index(string message)
        {
            ViewBag.Message = message;
            return View();
        }
    }
}
