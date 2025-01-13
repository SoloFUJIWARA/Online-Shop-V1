using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data; // Ensure this is included for EF Core features
using OnlineShop.Models; // Add this namespace for your actual DbContext

namespace OnlineShop.Controllers
{
    public class OrdersController : Controller
    {
     
        // Inject the DbContext into the controller
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context; // Assign it to the context variable
        }

        // Action to display the orders
        public ActionResult Orders()
        {
            // Query the database to get all orders from the InCart table
            var orders = _context.InCarts
                .Select(x => new SalesOrderViewModel
                {
                    ProductId = x.ProductId,
                    ProductName = x.ProductName,
                    Price = x.Price,
                    Quantity = x.Quantity,
                    UserId = x.UserId, // If you still want to show the UserId for each product
                    LineTotal = x.Quantity * x.Price,  // Calculate line total for the product
                    TotalDue = x.Quantity * x.Price   // Calculate total due for this product
                })
                .ToList();

            // Return the orders to the view
            return View("Index", orders);  // Pass orders to the view
        }




    }
}