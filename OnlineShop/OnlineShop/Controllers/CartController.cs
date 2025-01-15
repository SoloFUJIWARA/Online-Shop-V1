using Microsoft.AspNetCore.Mvc;
using OnlineShop.Models;
using OnlineShop.Data;
using Newtonsoft.Json;

namespace OnlineShop.Controllers
{
    public class CartController : Controller
    {  
        private readonly ApplicationDbContext _context;  
        
        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        private List<InCart> GetCartFromSession()
        {
            var cartJson = HttpContext.Session.GetString("Cart");
            return string.IsNullOrEmpty(cartJson)
                ? new List<InCart>()
                : JsonConvert.DeserializeObject<List<InCart>>(cartJson);
        }

        private void SaveCartToSession(List<InCart> cart)
        {
            HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cart));
        }

        public IActionResult Cart()
        {
            var cart = GetCartFromSession();  
            return View("/Views/Cart Views/Cart.cshtml", cart); 
        }

        public IActionResult ConfirmCart()
        {
            var cart = GetCartFromSession();

            foreach (var item in cart)
            {
                var inCart = new InCart
                {
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    Price = item.Price,
                    Quantity = item.Quantity,
                    UserId = "SomeUserId"
                };

                _context.InCarts.Add(inCart);
            }

            _context.SaveChanges();

            SaveCartToSession(new List<InCart>());

            return RedirectToAction("Cart");
        }

        public IActionResult AddToCart(int productId)
        {
            var product = _context.Products.FirstOrDefault(p => p.ProductId == productId);

            if (product == null)
            {
                return NotFound();
            }

            var cart = GetCartFromSession();

            var cartItem = cart.FirstOrDefault(c => c.ProductId == productId);

            if (cartItem != null)
            {
                cartItem.Quantity += 1;
            }
            else
            {
                cart.Add(new InCart
                {
                    ProductId = product.ProductId,
                    ProductName = product.Name,
                    Price = product.ListPrice,
                    Quantity = 1
                });
            }

            SaveCartToSession(cart);

            return RedirectToAction("Cart");
        }

        public IActionResult RemoveFromCart(int productId)
        {
            var cart = GetCartFromSession();

            var cartItem = cart.FirstOrDefault(c => c.ProductId == productId);

            if (cartItem != null)
            {
                cart.Remove(cartItem);
            }

            SaveCartToSession(cart);

            return RedirectToAction("Cart");
        }
    }
}
