using chic_lighting.DTOs;
using chic_lighting.Models;
using chic_lighting.Services.CommonService;
using chic_lighting.Services.UserService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace chic_lighting.Controllers
{
    public class AuthController : Controller
    {
        private readonly chic_lightingContext _context;
        private readonly ICommonService _commonService;
        private readonly IUserService _userService;
        public AuthController(chic_lightingContext context, ICommonService commonService, IUserService userService)
        {
            _context = context;
            _commonService = commonService;
            _userService = userService;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            Response.Cookies.Delete("token");
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO request)
        {
            try
            {
                var errorList = new Dictionary<string, string>();
                if (request.Username == null)
                {
                    errorList["NullUsername"] = "* Username connot be empty";
                }
                if (request.Password == null)
                {
                    errorList["NullPassword"] = "* Password connot be empty";
                }
                if (errorList.Count > 0)
                {
                    ViewBag.ErrorList = errorList;
                    return View();
                }
                var hashedPassword = await _commonService.Hash(request.Password);
                var hashedPasswordLower = hashedPassword.ToLower();
                var user = await _context.Users
                   .Where(u => u.Username == request.Username && u.Password.ToLower() == hashedPasswordLower)
                   .SingleOrDefaultAsync();
                if (user == null)
                {
                    errorList["Invalid"] = "* Username or Password is not valid";
                    ViewBag.ErrorList = errorList;
                    return View();
                }

                var token = await _userService.CreateToken(user);
                Response.Cookies.Append("token", token, new CookieOptions
                {
                    HttpOnly = true,
                    SameSite = SameSiteMode.Strict,
                    Expires= DateTime.Now.AddMinutes(30)
                });
                if (user.RoleId == 1)
                {
                    var currentMonth = DateTime.Now.Month;
                    var currentYear = DateTime.Now.Year;
                    return RedirectToAction("Index", "Dashboard", new MonthYearDTO
                    {
                        Year= currentYear,
                        Month= currentMonth,
                    });
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Error", ex.Message);
            }
        }


        [HttpGet]
        public async Task<IActionResult> Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDTO request)
        {
            try
            {

                var errorList = new Dictionary<string, string>();
                if (request.Firstname == null)
                {
                    errorList["NullFirstname"] = "* First Name cannot empty";
                }
                if (request.Lastname == null)
                {
                    errorList["NullLastname"] = "* Last Name cannot empty";
                }
                if (request.Username == null)
                {
                    errorList["NullUsername"] = "* Username cannot empty";
                }
                if (request.Email == null)
                {
                    errorList["NullEmail"] = "* Email cannot empty";
                }
                if (request.Address == null)
                {
                    errorList["NullAddress"] = "* Address cannot empty";
                }
                if (request.Password == null)
                {
                    errorList["NullPassword"] = "* Password cannot empty";
                }
                if (request.ConfirmPassword == null)
                {
                    errorList["NullConfirmPassword"] = "* Confirm assword cannot empty";
                }
                if (request.Password != request.ConfirmPassword)
                {
                    errorList["NotMatch"] = "* Password does not match";
                }
                if (!ModelState.IsValid)
                {
                    errorList["InvalideEmail"] = "* Invalid email";
                }
                if (await _context.Users.AnyAsync(u => u.Username == request.Username))
                {
                    errorList["ExistUsername"] = "* Username has already existed";
                }
                if (await _context.Users.AnyAsync(u => u.Email == request.Email))
                {
                    errorList["ExistEmail"] = "* Email has already existed";
                }
                if (errorList.Count > 0)
                {
                    ViewBag.ErrorList = errorList;
                    return View();
                }
                else
                {
                    string verifyCode;
                    while (true)
                    {
                        var rndstr = await _commonService.CreateRandomString(6);
                        verifyCode = rndstr.ToUpper();
                        if (!_context.Users.Any(x => x.VerifyCode == verifyCode))
                        {
                            break;
                        }
                    }

                    var newuser = new User
                    {
                        FirstName = request.Firstname,
                        LastName = request.Lastname,
                        Username = request.Username,
                        Email = request.Email,
                        Password = await _commonService.Hash(request.Password),
                        Address = request.Address,
                        CreateAt = DateTime.Now,
                        IsActive = false,
                        RoleId = 2,
                        VerifyCode = verifyCode,
                    };
                    await _context.Users.AddAsync(newuser);
                    await _context.SaveChangesAsync();
                    string body = string.Format("Your code verify is: {0}", newuser.VerifyCode);
                    await _commonService.sendEmail("Verifry Email", body, newuser.Email);
                    return RedirectToAction("Verify");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Error", ex.Message);
            }
        }
        [HttpGet]
        public async Task<IActionResult> Verify()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Verify(string Code)
        {
            try
            {
                var errorList = new Dictionary<string, string>();
                var unverified_user = await _context.Users
                .Where(u => u.VerifyCode == Code.ToUpper())
                .SingleOrDefaultAsync();
                if (unverified_user != null)
                {
                    if (unverified_user.IsActive == false)
                    {
                        unverified_user.VerifyAt = DateTime.Now;
                        unverified_user.IsActive = true;
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        errorList["Verified"] = "* User had already verified";
                    }
                }
                else
                {
                    errorList["InvalidCode"] = "* Invalid code";
                }
                if (errorList.Count > 0)
                {
                    ViewBag.ErrorList = errorList;
                    return View();
                }
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Error", ex.Message);
            }

        }
        [HttpGet]
        public async Task<IActionResult> ResetPassword()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> ResetPassword(string Email)
        {
            try
            {
                var user = await _context.Users
               .Where(u => u.Email == Email && u.IsActive == true)
               .SingleOrDefaultAsync();
                if (user != null)
                {
                    string resetpassword = await _commonService.CreateRandomString(10);
                    user.Password = await _commonService.Hash(resetpassword);
                    await _context.SaveChangesAsync();
                    string body = "Your Password is: " + resetpassword;
                    await _commonService.sendEmail("Reset Password", body, Email);
                    ViewBag.Success = "We've sent new password to your email, please check your email";
                    return View();
                }
                ViewBag.Error = "Your email hasn't been used";
                return View();
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Error", ex.Message);
            }
        }
    }
}
