using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data; // Ensure this is included for EF Core features
using OnlineShop.Models; // Add this namespace for your actual DbContext

namespace OnlineShop.Controllers
{
    public class OrdersController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        // Inject the DbContext into the controller
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context; // Assign it to the context variable
        }

        // Action to display the orders
        public IActionResult Orders()
        {
            // Retrieve the orders data by joining SalesOrderDetails and SalesOrderHeaders
            var orders = _context.SalesOrderDetails
                .Join(
                    _context.SalesOrderHeaders,  // Join SalesOrderDetails with SalesOrderHeaders
                    sod => sod.SalesOrderId,     // Match SalesOrderId in both tables
                    soh => soh.SalesOrderId,    // Match SalesOrderId in both tables
                    (sod, soh) => new SalesOrderViewModel
                    {
                        SalesOrderId = soh.SalesOrderId,    // Order ID from SalesOrderHeader
                        OrderDate = soh.OrderDate,          // Order Date from SalesOrderHeader
                        SalesOrderNumber = soh.SalesOrderNumber,  // Sales Order Number
                        Status = soh.Status,                // Order Status
                        ProductId = sod.ProductId,          // Product ID from SalesOrderDetail
                        OrderQty = sod.OrderQty,            // Quantity ordered
                        LineTotal = sod.LineTotal,          // Line total for the product
                        TotalDue = soh.TotalDue             // Total amount due for the order
                    })
                .OrderByDescending(o => o.OrderDate)  // Sort by OrderDate descending
                .ToList();  // Execute the query and get the result

            // Check if orders are null or empty before passing to the view
            if (orders == null || !orders.Any())
            {
                // Handle scenario where there are no orders
                ViewBag.Message = "No orders available.";
                return View(new List<SalesOrderViewModel>());  // Return an empty list if no data
            }

            // Return the view with the orders data
            return View(orders);
        }


    }
}