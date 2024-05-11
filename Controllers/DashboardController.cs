using chic_lighting.DTOs;
using chic_lighting.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;
using System.Linq;



namespace chic_lighting.Controllers
{
    public class DashboardController : Controller
    {
        private readonly chic_lightingContext _context;
        public DashboardController(chic_lightingContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index(MonthYearDTO request)
        {
            var currentYear = DateTime.Now.Year;


            var months = new List<SelectListItem>();
            for (int i = 1; i <= 12; i++)
            {
                months.Add(new SelectListItem
                {
                    Value = i.ToString(),
                    Text = new DateTime(2000, i, 1).ToString("MMMM")
                });
            }

            var years = new List<SelectListItem>();
            for (int i = 2000; i <= currentYear; i++)
            {
                years.Add(new SelectListItem
                {
                    Value = i.ToString(),
                    Text = i.ToString()
                });
            }

            var data = await (from o in _context.Orders
                              join od in _context.OrderDetails
                              on o.Id equals od.OrderId
                              join p in _context.Products
                              on od.ProductId equals p.Id
                              where o.OrderStatusId != 5 && o.OrderDate.Year == request.Year && o.OrderDate.Month == request.Month
                              group new { od, p } by new { p.Id, p.ProductName }
                              into g
                              orderby g.Sum(x => x.od.Quantity)
                              descending
                              select new
                              {
                                  Count = g.Sum(x => x.od.Quantity),
                                  ProductName = g.Key.ProductName,
                                  ProductId = g.Key.Id
                              })
                              .Take(10)
                              .ToListAsync();
            var labels = data
                .Select(d => d.ProductName)
                .ToArray();
            var values = data
                .Select(d => d.Count)
                .ToArray();
            ViewBag.Months = months;
            ViewBag.Years = years;
            ViewBag.Labels = labels;
            ViewBag.Values = values;
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> TopCategories(MonthYearDTO request)
        {
            var currentYear = DateTime.Now.Year;


            var months = new List<SelectListItem>();
            for (int i = 1; i <= 12; i++)
            {
                months.Add(new SelectListItem
                {
                    Value = i.ToString(),
                    Text = new DateTime(2000, i, 1).ToString("MMMM")
                });
            }

            var years = new List<SelectListItem>();
            for (int i = 2000; i <= currentYear; i++)
            {
                years.Add(new SelectListItem
                {
                    Value = i.ToString(),
                    Text = i.ToString()
                });
            }

            var data = await (from o in _context.Orders
                              join od in _context.OrderDetails on o.Id equals od.OrderId
                              join p in _context.Products on od.ProductId equals p.Id
                              join c in _context.Categories on p.CategoryId equals c.Id
                              where o.OrderDate.Month == request.Month && o.OrderDate.Year == request.Year && o.OrderStatusId != 5
                              group od by new { c.Id, c.CategoryName } into g
                              orderby g.Sum(s => s.Quantity) descending
                              select new
                              {
                                  g.Key.CategoryName,
                                  g.Key.Id,
                                  Total = g.Sum(s => s.Quantity),
                              })
                              .ToListAsync();
                              ;
            var labels = data
                .Select(d => d.CategoryName)
                .ToArray();
            var values = data
                .Select(d => d.Total)
                .ToArray();
            ViewBag.Months = months;
            ViewBag.Years = years;
            ViewBag.Labels = labels;
            ViewBag.Values = values;
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> TotalAmount(int year)
        {
            var currentYear = DateTime.Now.Year;



            var years = new List<SelectListItem>();
            for (int i = 2000; i <= currentYear; i++)
            {
                years.Add(new SelectListItem
                {
                    Value = i.ToString(),
                    Text = i.ToString()
                });
            }

            var startDate = new DateTime(year, 1, 1);
            var endDate = new DateTime(year, 12, 31);
            var months = Enumerable.Range(0, (endDate.Year - startDate.Year) * 12 + endDate.Month - startDate.Month + 1)
                                   .Select(offset => startDate.AddMonths(offset));

            var data = (from month in months
                        join o in _context.Orders on month equals new DateTime(o.OrderDate.Year, o.OrderDate.Month, 1) into gj
                        from subOrder in gj.DefaultIfEmpty()
                        join t in _context.Transactions on (subOrder != null ? subOrder.Id : (int?)null) equals t.OrderId into gj2
                        from subTransaction in gj2.DefaultIfEmpty()
                        group subTransaction by month into grouped
                        select new
                        {
                            Month = grouped.Key.ToString("MMMM"),
                            Total = grouped.Sum(x => x != null ? x.Total : 0)
                        }).ToList();

  






            var labels = data
                .Select(d => d.Month)
                .ToArray();
            var values = data
                .Select(d => d.Total)
                .ToArray();
            ViewBag.Years = years;
            ViewBag.Labels = labels;
            ViewBag.Values = values;
            return View();
        }
    }
}
