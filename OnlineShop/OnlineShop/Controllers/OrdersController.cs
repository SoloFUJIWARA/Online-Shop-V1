using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data; 
using OnlineShop.Models; 

namespace OnlineShop.Controllers
{
    public class OrdersController : Controller
    {
     
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        public ActionResult Orders()
        {
            var orders = _context.InCarts
                .Select(x => new SalesOrderViewModel
                {
                    ProductId = x.ProductId,
                    ProductName = x.ProductName,
                    Price = x.Price,
                    Quantity = x.Quantity,
                    UserId = x.UserId, 
                    LineTotal = x.Quantity * x.Price,  
                    TotalDue = x.Quantity * x.Price   
                })
                .ToList();

            return View("Index", orders); 
        }




    }
}