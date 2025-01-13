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
        private List<InCart> GetCartFromSession()
        {
            var cartJson = HttpContext.Session.GetString("Cart");
            return string.IsNullOrEmpty(cartJson)
                ? new List<InCart>()
                : JsonConvert.DeserializeObject<List<InCart>>(cartJson);
        }

        // Helper Method to Save Cart to Session
        private void SaveCartToSession(List<InCart> cart)
        {
            HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cart));
        }

        // View Cart Action
        public IActionResult Cart()
        {
            var cart = GetCartFromSession();  // Get the cart from the session
            return View("/Views/Cart Views/Cart.cshtml", cart);  // Specify the correct path and pass the model (cart)
        }

        public IActionResult ConfirmCart()
        {
            // Get the current cart from the session
            var cart = GetCartFromSession();

            // Loop through each item in the cart and add it to the InCarts table
            foreach (var item in cart)
            {
                // Create a new InCart object based on the CartItem data
                var inCart = new InCart
                {
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    Price = item.Price,
                    Quantity = item.Quantity,
                    UserId = "SomeUserId" // Replace this with actual UserId if needed
                };

                // Add the new InCart object to the database
                _context.InCarts.Add(inCart);
            }

            // Save the changes to the database
            _context.SaveChanges();

            // Optionally, clear the cart after confirming the order
            SaveCartToSession(new List<InCart>());

            // Redirect the user to a confirmation page or order summary
            return RedirectToAction("Cart");
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
                cart.Add(new InCart
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
