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

    // Report 1: Sales by Year and Month
    public ActionResult SalesByYearAndMonth()
    {
        var report = db.SalesOrderDetails
            .GroupBy(o => new { Year = o.ModifiedDate.Year, Month = o.ModifiedDate.Month })
            .Select(g => new
            {
                Year = g.Key.Year,
                Month = g.Key.Month,
                TotalSales = g.Sum(o => o.OrderQty)
            })
            .ToList();

        return View(report);
    }
    
    
    public ActionResult SalesByProduct()
    {
        var report = db.SalesOrderDetails
            .Join(db.Products, sod => sod.ProductId, p => p.ProductId, (sod, p) => new { sod, p })
            .GroupBy(x => new { x.p.ProductId, x.p.Name }) // Include ProductID in the group by
            .Select(g => new
            {
                ProductID = g.Key.ProductId,   // Accessing ProductID
                ProductName = g.Key.Name, // Accessing ProductName
                TotalSales = g.Sum(x => x.sod.LineTotal) // Sum of LineTotal
            })
            .ToList();

        return View(report);
    }

    // Report 3: Sales by Product Categories
    public ActionResult SalesByCategory()
    {
        var report = db.SalesOrderDetails
            .Join(db.Products, sod => sod.ProductId, p => p.ProductId, (sod, p) => new { sod, p })
            .Join(db.ProductCategories, pp => pp.p.ProductCategoryId, pc => pc.ProductCategoryId, (pp, pc) => new { pp.sod, pp.p, pc })
            .GroupBy(x => new { x.pc.ProductCategoryId, x.pc.Name })  // Group by CategoryID and CategoryName
            .Select(g => new
            {
                CategoryID = g.Key.ProductCategoryId,
                CategoryName = g.Key.Name, // Accessing CategoryName
                TotalSales = g.Sum(x => x.sod.LineTotal) // Summing sales
            })
            .ToList();

        return View(report);
    }
/* //
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
    }#1#*/
}
