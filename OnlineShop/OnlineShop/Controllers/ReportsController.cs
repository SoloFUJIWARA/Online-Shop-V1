using Microsoft.AspNetCore.Mvc;
using OnlineShop.Data;

namespace OnlineShop.Controllers;

public class ReportsController : Controller
{
    private ApplicationDbContext db = new ApplicationDbContext();

    // GET: Reports/Index
    public ActionResult Index()
    {
        return View();
    }

    /*// Report 1: Sales by Year and Month
    public ActionResult SalesByYearAndMonth()
    {
        var report = db.Orders
                       .GroupBy(o => new { o.OrderDate.Year, o.OrderDate.Month })
                       .Select(g => new
                       {
                           Year = g.Key.Year,
                           Month = g.Key.Month,
                           TotalSales = g.Sum(o => o.Amount)
                       }).ToList();
        return View(report);
    }

    // Report 2: Sales by Product
    public ActionResult SalesByProduct()
    {
        var report = db.Orders
                       .GroupBy(o => o.ProductId)
                       .Select(g => new
                       {
                           Product = g.Key,
                           TotalSales = g.Sum(o => o.Amount)
                       }).ToList();
        return View(report);
    }

    // Report 3: Sales by Product Categories
    public ActionResult SalesByCategory()
    {
        var report = db.Orders
                       .GroupBy(o => o.Category.Name)
                       .Select(g => new
                       {
                           Category = g.Key,
                           TotalSales = g.Sum(o => o.Amount)
                       }).ToList();
        return View(report);
    }

    // Report 4: Sales by Customers and Year
    public ActionResult SalesByCustomerAndYear()
    {
        var report = db.Orders
                       .GroupBy(o => new { o.CustomerId, o.OrderDate.Year })
                       .Select(g => new
                       {
                           CustomerId = g.Key.CustomerId,
                           Year = g.Key.Year,
                           TotalSales = g.Sum(o => o.Amount)
                       }).ToList();
        return View(report);
    }

    // Report 5: Sales by City
    public ActionResult SalesByCity()
    {
        var report = db.Orders
                       .GroupBy(o => o.City)
                       .Select(g => new
                       {
                           City = g.Key,
                           TotalSales = g.Sum(o => o.Amount)
                       }).ToList();
        return View(report);
    }

    // Report 6: Top 10 Customers by Sales Amount
    public ActionResult TopCustomersBySales()
    {
        var report = db.Orders
                       .GroupBy(o => o.CustomerId)
                       .OrderByDescending(g => g.Sum(o => o.Amount))
                       .Take(10)
                       .Select(g => new
                       {
                           Customer = g.Key,
                           TotalSales = g.Sum(o => o.Amount)
                       }).ToList();
        return View(report);
    }

    // Report 7: Top 10 Customers by Sales Amount for each year
    public ActionResult TopCustomersBySalesForYear(int year)
    {
        var report = db.Orders
                       .Where(o => o.OrderDate.Year == year)
                       .GroupBy(o => o.CustomerId)
                       .OrderByDescending(g => g.Sum(o => o.Amount))
                       .Take(10)
                       .Select(g => new
                       {
                           Customer = g.Key,
                           TotalSales = g.Sum(o => o.Amount)
                       }).ToList();
        return View(report);
    }

    // Report 8: Top 10 Products by Sales Amount
    public ActionResult TopProductsBySales()
    {
        var report = db.Orders
                       .GroupBy(o => o.ProductId)
                       .OrderByDescending(g => g.Sum(o => o.Amount))
                       .Take(10)
                       .Select(g => new
                       {
                           Product = g.Key,
                           TotalSales = g.Sum(o => o.Amount)
                       }).ToList();
        return View(report);
    }

    // Report 9: Top 10 Products by Sales Profit
    public ActionResult TopProductsByProfit()
    {
        var report = db.Orders
                       .GroupBy(o => o.ProductId)
                       .OrderByDescending(g => g.Sum(o => o.Profit))
                       .Take(10)
                       .Select(g => new
                       {
                           Product = g.Key,
                           TotalProfit = g.Sum(o => o.Profit)
                       }).ToList();
        return View(report);
    }

    // Report 10: Top 10 Products by Sales Amount for each year
    public ActionResult TopProductsBySalesForYear(int year)
    {
        var report = db.Orders
                       .Where(o => o.OrderDate.Year == year)
                       .GroupBy(o => o.ProductId)
                       .OrderByDescending(g => g.Sum(o => o.Amount))
                       .Take(10)
                       .Select(g => new
                       {
                           Product = g.Key,
                           TotalSales = g.Sum(o => o.Amount)
                       }).ToList();
        return View(report);
    }*/
}
