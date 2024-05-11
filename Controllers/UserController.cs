using chic_lighting.DTOs;
using chic_lighting.Models;
using chic_lighting.Services.CommonService;
using chic_lighting.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace chic_lighting.Controllers
{
    public class UserController : Controller
    {
        private readonly chic_lightingContext _context;
        private readonly IUserService _userService;
        private readonly ICommonService _commonService;
        public UserController(chic_lightingContext context, IUserService userService, ICommonService commonService)
        {
            _context = context;
            _userService = userService;
            _commonService = commonService;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Profile(Dictionary<string, string> responseMesage)
        {
            ViewBag.Response = responseMesage;
            var user = await _userService.getCurrentUser();
            ViewBag.User = user;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Change(ChangeDTO request)
        {
            ViewBag.Success = "Change password successfully";
            try
            {
                var errorList = new Dictionary<string, string>();
                if (request.CurrentPass == null || request.NewPass == null || request.ConfirmNewPass == null)
                {
                    errorList["Empty"] = "* Cannot empty!";
                }
                if (request.NewPass != request.ConfirmNewPass)
                {
                    errorList["NotMatch"] = "* Password does not match";
                }
                if (errorList.Count > 0)
                {
                    return RedirectToAction("profile", errorList);
                }
                var user = await _userService.getCurrentUser();
                if (user.Password != await _commonService.Hash(request.CurrentPass))
                {
                    errorList["InvalidPass"] = "* your current password is not correct";
                   return RedirectToAction("profile", errorList);
                }

                var changeUser = await _context.Users
                    .Where(u => u.Id == user.Id)
                    .SingleOrDefaultAsync();
                changeUser.Password = await _commonService.Hash(request.NewPass);
                await _context.SaveChangesAsync();
                var success = new Dictionary<string, string>();
                success["Success"] = "Change password successfully";
                return RedirectToAction("profile", success);

            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Error", ex.Message);
            }

        }
    }
}
