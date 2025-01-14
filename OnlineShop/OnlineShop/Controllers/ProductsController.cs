using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Models;
using OnlineShop.ViewModels;
using System.Linq;

namespace OnlineShop.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var products = _context.Products
                .Include(p => p.ProductCategory)
                .Select(p => new ProductViewModel
                {
                    ProductId = p.ProductId,
                    Name = p.Name,
                    ProductNumber = p.ProductNumber,
                    Color = p.Color,
                    ListPrice = p.ListPrice,
                    ModifiedDate = p.ModifiedDate,
                    OrderCount = _context.SalesOrderDetails
                        .Count(o => o.ProductId == p.ProductId) // Get the number of orders
                })
                .ToList();

            return View(products);
        }

        public IActionResult Details(int id)
        {
            var product = _context.Products
                .Include(p => p.ProductCategory)
                .Include(p => p.SalesOrderDetails) // Load related SalesOrderDetails
                .FirstOrDefault(p => p.ProductId == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        public IActionResult Create()
        {
            ViewBag.Categories = _context.ProductCategories.ToList();
            return View(new Product { SellStartDate = DateTime.Now });
        }

        [HttpPost]
        public IActionResult Create([Bind("Name,ProductNumber,Color,StandardCost,ListPrice,SellStartDate,ProductCategoryId")] Product product)
        {
            if (ModelState.IsValid)
            {
                product.Rowguid = Guid.NewGuid();
                product.ModifiedDate = DateTime.Now;
                _context.Add(product);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        public IActionResult Edit(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            ViewBag.Categories = _context.ProductCategories.ToList();
            return View(product);
        }

        [HttpPost]
        public IActionResult Edit([Bind("ProductId,Name,ProductNumber,Color,ListPrice,Size,Weight,SellStartDate,SellEndDate,DiscontinuedDate,ProductCategoryId")] Product product)
        {
            if (ModelState.IsValid)
            {
                var existingProduct = _context.Products.Find(product.ProductId);
                if (existingProduct != null)
                {
                    _context.Entry(existingProduct).State = EntityState.Detached;
                    product.Rowguid = existingProduct.Rowguid;
                    product.ModifiedDate = DateTime.Now;
                    _context.Update(product);
                    _context.SaveChanges();

                    return RedirectToAction(nameof(Index));
                }
                return NotFound();
            }
            return View(product);
        }

        public IActionResult Delete(int id)
        {
            var product = _context.Products
                .Include(p => p.SalesOrderDetails) // Load sales orders
                .FirstOrDefault(p => p.ProductId == id);

            if (product == null)
            {
                return NotFound();
            }

            if (product.SalesOrderDetails.Any()) // Check if the product has sales
            {
                ModelState.AddModelError("", "Cannot delete this product because it has existing sales orders.");
                return View(product);
            }

            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var product = _context.Products
                .Include(p => p.SalesOrderDetails)
                .FirstOrDefault(p => p.ProductId == id);

            if (product == null)
            {
                return NotFound();
            }

            // 🚨 Check if the product has purchases in `InCarts`
            bool hasPurchases = _context.InCarts.Any(c => c.ProductId == id);
            if (hasPurchases)
            {
                ModelState.AddModelError("", "❌ This product has confirmed purchases and cannot be deleted.");
                return View("Delete", product); // Show error message on the delete page
            }

            // 🚀 If no purchases, allow deletion
            _context.Products.Remove(product);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

    }
}
