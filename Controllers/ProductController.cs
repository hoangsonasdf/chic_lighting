using chic_lighting.DTOs;
using chic_lighting.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace chic_lighting.Controllers
{
    public class ProductController : Controller
    {
        private readonly chic_lightingContext _context;
        public ProductController(chic_lightingContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products
                .Where(p => p.IsActive == true)
                .Select(p => new
                {
                    p.Id,
                    p.ProductName,
                    CategoryName = p.Category.CategoryName,
                    StatusName = p.ProductStatus.Name,
                    p.Price,
                    p.Saleprice,
                    p.Image
                })
                .ToListAsync();
            ViewBag.Products = products;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Admin()
        {
            var products = await _context.Products
                .Where(p => p.IsActive == true)
                .Select(p => new
                {
                    p.Id,
                    p.ProductName,
                    CategoryName = p.Category.CategoryName,
                    StatusName = p.ProductStatus.Name,
                    p.Price,
                    p.Quantity,
                    p.Saleprice,
                    p.Image
                })
                .ToListAsync();
            ViewBag.Products = products;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Detail(long id)
        {
            var product = await _context.Products
                .Where(p => p.IsActive == true && p.Id == id)
                .Select(p => new
                {
                    p.Id,
                    p.ProductName,
                    CategoryName = p.Category.CategoryName,
                    StatusName = p.ProductStatus.Name,
                    p.Price,
                    p.Quantity,
                    p.Saleprice,
                    p.Image
                })
                .SingleOrDefaultAsync();
            ViewBag.Product = product;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(long id, Dictionary<string, string> responseMesage)
        {
            var product = await _context.Products
                .Where(p => p.IsActive == true && p.Id == id)
                .Select(p => new
                {
                    p.Id,
                    p.ProductName,
                    CategoryName = p.Category.CategoryName,
                    p.CategoryId,
                    p.ProductStatusId,
                    StatusName = p.ProductStatus.Name,
                    p.Price,
                    p.Quantity,
                    p.Saleprice,
                    p.Image,
                    p.Description
                })
                .SingleOrDefaultAsync();
            var categories = await _context.Categories
                .Where(c => c.IsActive == true)
                .Select(c => new
                {
                    c.Id,
                    c.CategoryName
                })
                .ToListAsync();
            var statuses = await _context.ProductStatuses
                .Select(ps => new
                {
                    ps.Id,
                    ps.Name
                })
                .ToListAsync();
            ViewBag.Categories = categories;
            ViewBag.Product = product;
            ViewBag.Statuses = statuses;
            ViewBag.Response = responseMesage;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(long id, [FromForm] ProductDTO request)
        {
            try
            {
                var errorList = new Dictionary<string, string>();
                if (request.ProductName == null)
                {
                    errorList["NullProductName"] = "* Product name can not be empty";
                }
                if (request.Quantity == null)
                {
                    errorList["NullQuantity"] = "* Quantity can not be empty";
                }
                if (request.Price == null)
                {
                    errorList["NullPrice"] = "* Price can not be empty";
                }
                if (errorList.Count > 0)
                {
                    return RedirectToAction("Edit", "Product", new {id = id, responseMesage = errorList});
                }
                var product = await _context.Products
                    .Where(p => p.Id == id)
                    .SingleOrDefaultAsync()
                    ?? throw new Exception("An error occur");
                product.ProductName = request.ProductName;
                product.Quantity = request.Quantity;
                product.Price = request.Price;
                product.Saleprice = request.Saleprice;
                product.CategoryId = request.CategoryId;
                product.ProductStatusId = request.ProductStatusId;
                product.Description= request.Description;
                if (request.Image.Length > 0)
                {
                    string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                    string fileName = $"{timestamp}_{request.Image.FileName}";
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "lightType", fileName);
                    using (var stream = System.IO.File.Create(path))
                    {
                        await request.Image.CopyToAsync(stream);
                    }
                    product.Image = string.Format("/img/lightType/{0}", fileName);
                }
                await _context.SaveChangesAsync();
                var successMessage = new Dictionary<string, string>();
                successMessage["Success"] = "Edit product successfully!";
                 return RedirectToAction("Edit", "Product", new { id = id, responseMesage = successMessage });

            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Error", ex.Message);
            }
        }
    }
}
