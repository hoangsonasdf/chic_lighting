using chic_lighting.DTOs;
using chic_lighting.Models;
using chic_lighting.Services.CommonService;
using chic_lighting.Services.UserService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace chic_lighting.Controllers
{
    public class OrderController : Controller
    {
        private readonly chic_lightingContext _context;
        private readonly IUserService _userService;
        private readonly ICommonService _commonService;

        public OrderController(chic_lightingContext context, IUserService userService, ICommonService commonService)
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
        public async Task<IActionResult> History(long statusID)
        {
            var user = await _userService.getCurrentUser();
            var countOrder = await _context.Orders
                .Where(o => o.UserId == user.Id && o.OrderStatusId == statusID)
                .CountAsync();  
            ViewBag.Count = countOrder;
            var listOrder = await _context.Orders
                .Where(o => o.UserId == user.Id && o.OrderStatusId == statusID)
                .Select(o => new
                {
                    o.Id,
                    ListProduct = o.OrderDetails
                    .Select(od => new
                    {
                        od.Product.Image,
                        od.Product.ProductName,
                        od.Quantity,
                        od.Price,
                    })
                })
                .ToListAsync();
            var orderStatus = await _context.OrderStatuses
                .Select(os => new
                {
                    os.Id,
                    os.Name,
                    os.Bootstapicon,
                })
                .ToListAsync();
            var currentOrderStatusName = await _context.OrderStatuses
                .Where(os => os.Id == statusID)
                .Select(os =>
                  os.Name
                )
                .SingleOrDefaultAsync();
            ViewBag.OrderStatusName = currentOrderStatusName;
            ViewBag.OrderStatus = orderStatus;
            ViewBag.History = listOrder;
            ViewBag.StatusID = statusID;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Cancel(long orderID)
        {
            try
            {
                var order = await _context.Orders
                    .Where(o => o.Id == orderID && o.OrderStatusId == 1)
                    .SingleOrDefaultAsync()
                    ?? throw new Exception("Order not found");
                order.OrderStatusId = 5;
                await _context.OrderDetails
                                  .Where(od => od.OrderId == orderID)
                                  .Select(od => new
                                  {
                                      od.Quantity,
                                      od.Product
                                  })
                                  .ForEachAsync(pl =>
                                  {
                                      pl.Product.Quantity += pl.Quantity;
                                  });

                await _context.SaveChangesAsync();
                return RedirectToAction("History", "Order", new { statusID = 1 });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Error", ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(OrderDTO request)
        {
            try
            {
                var user = await _userService.getCurrentUser();
                var newOrder = new Order
                {
                    Firstname = request.FirstName,
                    LastName = request.LastName,
                    Address = request.Address,
                    Phone = request.Phone,
                    Notes = request.Note,
                    UserId = user.Id,
                    OrderStatusId = 1,
                    OrderDate = DateTime.Now,
                };
                await _context.AddAsync(newOrder);
                await _context.SaveChangesAsync();

                foreach (var p in request.ProductInfor)
                {
                    var details = new OrderDetail
                    {
                        OrderId = newOrder.Id,
                        ProductId = p.ProductID,
                        Quantity = p.Quantity,
                        Price = p.Price
                    };
                    await _context.OrderDetails.AddAsync(details);
                }
                await _context.SaveChangesAsync();

                var getTotal = await (from o in _context.Orders
                                      join od in _context.OrderDetails
                                      on o.Id equals od.OrderId
                                      join p in _context.Products
                                      on od.ProductId equals p.Id
                                      select od.Quantity * (p.Saleprice != null ? p.Saleprice : p.Price)).SumAsync();

                var newTransaction = new Transaction
                {
                    OrderId = newOrder.Id,
                    PaymentId = request.Payment,
                    Total = getTotal,
                };
                await _context.AddAsync(newTransaction);
                await _context.SaveChangesAsync();

                var userCart = await _context.Carts
                    .Where(u => u.UserId == user.Id)
                    .SingleOrDefaultAsync();

                foreach (var p in request.ProductInfor)
                {
                    var cartItems = await _context.CartItems
                        .Where(ci => ci.CartId == userCart.Id && ci.ProductId == p.ProductID)
                        .SingleOrDefaultAsync();
                    if (cartItems != null)
                    {
                        _context.CartItems.Remove(cartItems);
                    }
                }
                foreach (var p in request.ProductInfor)
                {
                    var product = await _context.Products
                        .Where(pr => pr.Id == p.ProductID && pr.IsActive == true)
                        .SingleOrDefaultAsync();
                    if (product != null)
                    {
                        product.Quantity -= p.Quantity;
                        if (product.Quantity == 0)
                        {
                            product.ProductStatusId = 4;
                        }
                    }
                }
                await _context.SaveChangesAsync();
                TempData["OrderSuccess"] = "Order Successfully";
                return RedirectToAction("Index", "Cart");

            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Error", ex.InnerException);
            }
        }
    }
}
