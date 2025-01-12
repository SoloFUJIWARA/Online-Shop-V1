using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Models;
using OnlineShop.ViewModels;

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
                .Include(p => p.ProductCategory) // Include related category data
                .Select(p => new ProductViewModel
                {
                    ProductId = p.ProductId,
                    Name = p.Name,
                    ProductNumber = p.ProductNumber,
                    Color = p.Color,
                    ListPrice = p.ListPrice,
                    ModifiedDate = p.ModifiedDate,
                    OrderCount = _context.SalesOrderDetails
                        .Where(o => o.ProductId == p.ProductId)
                        .Count() // Efficiently count orders for each product
                })
                .ToList();

            return View(products);
        }

        public IActionResult Details(int id)
        {
            var product = _context.Products
                .Include(p => p.ProductCategory) // Include related category data
                .FirstOrDefault(p => p.ProductId == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        public IActionResult Create()
        {
            var categories = _context.ProductCategories.ToList();
            ViewBag.Categories = categories;

            var product = new Product
            {
                SellStartDate = DateTime.Now
            };
            
            return View(product);
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
            var categories = _context.ProductCategories.ToList();
            ViewBag.Categories = categories;
            var models = _context.ProductModels.ToList();
            ViewBag.Models = models;

            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost]
        public IActionResult Edit([Bind("ProductId,Name,ProductNumber,Color,ListPrice,Size,Weight," +
                                        "SellStartDate,SellEndDate,DiscontinuedDate,ProductCategoryId,ProductModelId")] Product product)
        {
            if (ModelState.IsValid)
            {
                var existingProduct = _context.Products.Find(product.ProductId);
                if (existingProduct != null)
                {
                    _context.Entry(existingProduct).State = EntityState.Detached; // Detach previous entity to avoid conflict
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
            var orders = _context.SalesOrderDetails
                .Where(o => o.ProductId == id)
                .Count(); // Efficiently count orders for this product

            ViewBag.Orders = orders;

            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var product = _context.Products.Find(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }
        
    }
}
