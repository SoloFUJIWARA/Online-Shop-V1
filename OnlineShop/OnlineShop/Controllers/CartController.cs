using Microsoft.AspNetCore.Mvc;
using OnlineShop.Models;
using OnlineShop.Data;
using Newtonsoft.Json;

namespace OnlineShop.Controllers
{
    public class CartController : Controller
    {  
        private readonly ApplicationDbContext _context;  // Declare DbContext
        
        // Constructor to inject ApplicationDbContext
        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Helper Method to Get Cart from Session
        private List<CartItem> GetCartFromSession()
        {
            var cartJson = HttpContext.Session.GetString("Cart");
            return string.IsNullOrEmpty(cartJson)
                ? new List<CartItem>()
                : JsonConvert.DeserializeObject<List<CartItem>>(cartJson);
        }

        // Helper Method to Save Cart to Session
        private void SaveCartToSession(List<CartItem> cart)
        {
            HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cart));
        }

        // View Cart Action
        public IActionResult Cart()
        {
            var cart = GetCartFromSession();  // Get the cart from the session
            return View("/Views/Cart Views/Cart.cshtml", cart);  // Specify the correct path and pass the model (cart)
        }


        // Add to Cart Action
        public IActionResult AddToCart(int productId)
        {
            // Fetch the product from the database using the productId
            var product = _context.Products.FirstOrDefault(p => p.ProductId == productId);

            if (product == null)
            {
                // If the product doesn't exist, return a NotFound response
                return NotFound();
            }

            // Get the current cart from the session
            var cart = GetCartFromSession();

            // Find if the product is already in the cart
            var cartItem = cart.FirstOrDefault(c => c.ProductId == productId);

            if (cartItem != null)
            {
                // If the item exists in the cart, increment the quantity
                cartItem.Quantity += 1;
            }
            else
            {
                // If the item doesn't exist, add it to the cart
                cart.Add(new CartItem
                {
                    ProductId = product.ProductId,
                    ProductName = product.Name,
                    Price = product.ListPrice,
                    Quantity = 1
                });
            }

            // Save the updated cart back to the session
            SaveCartToSession(cart);

            // Redirect to the Cart view to show updated cart
            return RedirectToAction("Cart");
        }

        // Remove from Cart Action
        public IActionResult RemoveFromCart(int productId)
        {
            // Get the cart from the session
            var cart = GetCartFromSession();

            // Find the cart item that matches the productId
            var cartItem = cart.FirstOrDefault(c => c.ProductId == productId);

            if (cartItem != null)
            {
                // Remove the item from the cart
                cart.Remove(cartItem);
            }

            // Save the updated cart back to the session
            SaveCartToSession(cart);

            // Redirect to the Cart view to show updated cart
            return RedirectToAction("Cart");
        }
    }
}
