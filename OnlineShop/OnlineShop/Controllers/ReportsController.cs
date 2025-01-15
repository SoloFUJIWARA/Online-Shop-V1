using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OnlineShop.Data;
using OnlineShop.Models;

namespace OnlineShop.Controllers;

public class ReportsController : Controller
{
    private ApplicationDbContext db = new ApplicationDbContext();

    public ActionResult Index()
    {
        return View();
    }

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
            .GroupBy(x => new { x.p.ProductId, x.p.Name })
            .Select(g => new
            {
                ProductID = g.Key.ProductId,  
                ProductName = g.Key.Name, 
                TotalSales = g.Sum(x => x.sod.LineTotal) 
            })
            .ToList();

        return View(report);
    }

    public ActionResult SalesByCategory()
    {
        var report = db.SalesOrderDetails
            .Join(db.Products, sod => sod.ProductId, p => p.ProductId, (sod, p) => new { sod, p })
            .Join(db.ProductCategories, pp => pp.p.ProductCategoryId, pc => pc.ProductCategoryId, (pp, pc) => new { pp.sod, pp.p, pc })
            .GroupBy(x => new { x.pc.ProductCategoryId, x.pc.Name })  
            .Select(g => new
            {
                CategoryID = g.Key.ProductCategoryId,
                CategoryName = g.Key.Name, 
                TotalSales = g.Sum(x => x.sod.LineTotal) 
            })
            .ToList();

        return View(report);
    }

    public ActionResult SalesByCustomerAndYear()
    {
        var report = db.SalesOrderDetails
            .Include(a => a.Product) 
            .Join(
                db.SalesOrderHeaders, 
                salesDetail => salesDetail.SalesOrderId, 
                salesHeader => salesHeader.SalesOrderId,
                (salesDetail, salesHeader) => new
                {
                    salesDetail,
                    salesHeader.CustomerId
                }
            )
            .Join(
                db.Customers,
                salesWithHeader => salesWithHeader.CustomerId, 
                customer => customer.CustomerId,
                (salesWithHeader, customer) => new
                {
                    salesWithHeader.salesDetail,
                    CustomerName = customer.CompanyName, 
                    CustomerID = customer.CustomerId,
                    salesWithHeader.salesDetail.ModifiedDate
                }
            )
            .GroupBy(s => new { s.CustomerID, Year = s.ModifiedDate.Year }) 
            .Select(g => new
            {
                CustomerID = g.Key.CustomerID,
                CustomerName = g.First().CustomerName,
                Year = g.Key.Year,
                TotalSales = g.Sum(x => x.salesDetail.UnitPrice * x.salesDetail.OrderQty) 
            })
            .ToList();

        return View(report);

    }
    
    public ActionResult SalesByCity()
    {
        var report = db.SalesOrderDetails
            .Join(db.Addresses, 
                sod => sod.ProductId, 
                addr => addr.AddressId, 
                (sod, addr) => new { sod, addr })
            .GroupBy(x => x.addr.City) 
            .Select(g => new
            {
                City = g.Key, 
                TotalSales = g.Sum(x => x.sod.LineTotal) 
            })
            .ToList();

        return View(report);
    }

    public IActionResult Top10CustomersBySales()
    {
        var topCustomers = db.Customers
            .Select(customer => new
            {
                CustomerName = customer.FirstName + " " + customer.LastName, 
                TotalSales = customer.SalesOrderHeaders
                    .SelectMany(order => order.SalesOrderDetails)
                    .Sum(detail => detail.LineTotal)
                    .ToString() + " $"
            })
            .OrderByDescending(c => c.TotalSales)
            .Take(10)
            .ToList();


        return View(topCustomers);
    }

    public ActionResult Top10CustomersBySalesForYear(int year = 2008)
    {
        var topCustomers = db.Customers
            .Select(customer => new
            {
                CustomerName = customer.FirstName + " " + customer.LastName, 
                TotalSales = customer.SalesOrderHeaders
                    .Where(order => order.OrderDate.Year == year)  
                    .SelectMany(order => order.SalesOrderDetails)  
                    .Sum(detail => detail.LineTotal)  
            })
            .Where(c => c.TotalSales > 0) 
            .OrderByDescending(c => c.TotalSales) 
            .Take(10) 
            .ToList();

        var formattedTopCustomers = topCustomers.Select(c => new
        {
            CustomerName = c.CustomerName,
            TotalSales = c.TotalSales.ToString("C")
        }).ToList();

        return View(formattedTopCustomers); 
    }



    public ActionResult Top10ProductsBySales()
    {
        var topProducts = db.SalesOrderDetails
            .Join(db.Products, sod => sod.ProductId, p => p.ProductId, (sod, p) => new { sod, p })
            .GroupBy(x => new { x.sod.ProductId, x.p.Name }) 
            .Select(g => new
            {
                ProductName = g.Key.Name,
                TotalSales = g.Sum(x => x.sod.LineTotal)
            })
            .OrderByDescending(g => g.TotalSales)  
            .Take(10)
            .ToList();

        var formattedTopProducts = topProducts.Select(p => new
        {
            ProductName = p.ProductName,
            TotalSales = p.TotalSales.ToString("C") 
        }).ToList();

        return View(formattedTopProducts); 
    }


/* //
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
