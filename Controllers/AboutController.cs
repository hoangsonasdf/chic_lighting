using Microsoft.AspNetCore.Mvc;

namespace chic_lighting.Controllers
{
    public class AboutController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}
