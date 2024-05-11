using chic_lighting.DTOs;
using chic_lighting.Models;
using chic_lighting.Services.UserService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace chic_lighting.Controllers
{
    public class CartController : Controller
    {
        private readonly chic_lightingContext _context;
        private readonly IUserService _userService;

        public CartController(chic_lightingContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userService.getCurrentUser();
            var userCart = await _context.Carts
                .Where(c => c.UserId == user.Id)
                .SingleOrDefaultAsync();
            if (userCart == null)
            {
                await _context.AddAsync(new Cart
                {
                    UserId = user.Id,
                    CreatedAt = DateTime.Now,
                });
                await _context.SaveChangesAsync();
            }
            var productsInCart = await _context.CartItems
                .Where(ci => ci.CartId == userCart.Id)
                .Select(ci => new
                {
                    ci.Id,
                    ProductId = ci.Product.Id,
                    ProductName = ci.Product.ProductName,
                    ci.Quantity,
                    Price = ci.Product.Saleprice != null ? (ci.Product.Saleprice * ci.Quantity) : (ci.Product.Price * ci.Quantity),
                    ProductImage = ci.Product.Image
                })
                .ToListAsync();
            var total = await (from c in _context.Carts
                               join ci in _context.CartItems
                               on c.Id equals ci.CartId
                               join p in _context.Products
                               on ci.ProductId equals p.Id
                               select ci.Quantity * (p.Saleprice != null ? p.Saleprice : p.Price)).SumAsync();
            var payments = await _context.Payments
                .Select(p => new
                {
                    p.Id,
                    p.Name
                })
                .ToListAsync();
            ViewBag.Products = productsInCart;
            ViewBag.Total = total;
            ViewBag.Payments = payments;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddCartDTO request)
        {
            try
            {
                var user = await _userService.getCurrentUser();
                var userCart = await _context.Carts
                    .Where(c => c.UserId == user.Id)
                    .SingleOrDefaultAsync();
                if (userCart == null)
                {
                    userCart = new Cart
                    {
                        UserId = user.Id,
                        CreatedAt = DateTime.Now,
                    };
                    _context.Carts.Add(userCart);
                    await _context.SaveChangesAsync();
                }
                if (await _context.CartItems.AnyAsync(ci => ci.ProductId == request.ProductId))
                {
                    TempData["AlertMessage"] = "Product has already in cart";
                    return RedirectToAction("Index");
                }
                var newCartItem = new CartItem
                {
                    CartId = userCart.Id,
                    ProductId = request.ProductId,
                    Quantity = request.Quantity,
                };
                await _context.AddAsync(newCartItem);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Product");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Error", ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(long ProductId)
        {
            try
            {
                var user = await _userService.getCurrentUser();
                var userCart = await _context.Carts
                    .Where(c => c.UserId == user.Id)
                    .SingleOrDefaultAsync();
                if (userCart == null)
                {
                    userCart = new Cart
                    {
                        UserId = user.Id,
                        CreatedAt = DateTime.Now,
                    };
                    _context.Carts.Add(userCart);
                    await _context.SaveChangesAsync();
                }
                var product = await _context.CartItems
                    .Where(ci => ci.ProductId == ProductId)
                    .SingleOrDefaultAsync();
                if (product != null)
                {
                    _context.Remove(product);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Error", ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Up(long id)
        {
            try
            {
                var cartItem = await _context.CartItems
                    .Where(ci => ci.Id == id)
                    .SingleOrDefaultAsync();
                cartItem.Quantity++;
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Error", ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Down(long id)
        {
            try
            {
                var cartItem = await _context.CartItems
                    .Where(ci => ci.Id == id)
                    .SingleOrDefaultAsync();
                cartItem.Quantity--;
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Error", ex.Message);
            }
        }
    }
}
