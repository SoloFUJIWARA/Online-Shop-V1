using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Models;
using OnlineShop.ViewModels;
using System;
using System.Linq;

namespace OnlineShop.Controllers
{
    public class ProductCategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductCategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var categories = _context.ProductCategories
                .Include(c => c.Products)  
                .Select(c => new ProductCategoryViewModel
                {
                    Id = c.ProductCategoryId,
                    Name = c.Name,
                    ModifiedDate = c.ModifiedDate,
                    ProductCount = c.Products.Count() 
                })
                .ToList();

            return View(categories);
        }

        public IActionResult Details(int id)
        {
            var category = _context.ProductCategories
                .Include(c => c.Products)  
                .ThenInclude(p => p.SalesOrderDetails) 
                .FirstOrDefault(c => c.ProductCategoryId == id);

            if (category == null)
            {
                return NotFound();
            }

            var products = category.Products.Select(p => new ProductViewModel
            {
                ProductId = p.ProductId,
                Name = p.Name,
                ProductNumber = p.ProductNumber,
                Color = p.Color,
                ListPrice = p.ListPrice,
                ModifiedDate = p.ModifiedDate,
                OrderCount = p.SalesOrderDetails.Count() 
            }).ToList();

            ViewBag.Products = products;

            return View(category);
        }

        public IActionResult Create()
        {
            var category = new ProductCategory();
            return View(category);
        }

        [HttpPost]
        public IActionResult Create([Bind("Name")] ProductCategory category)
        {
            if (ModelState.IsValid)
            {
                category.Rowguid = Guid.NewGuid();
                category.ModifiedDate = DateTime.Now;
                _context.Add(category);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        public IActionResult Edit(int id)
        {
            var category = _context.ProductCategories.Find(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        public IActionResult Edit([Bind("ProductCategoryId,Name")] ProductCategory category)
        {
            if (ModelState.IsValid)
            {
                var existingCategory = _context.ProductCategories.Find(category.ProductCategoryId);
                if (existingCategory != null)
                {
                    existingCategory.Name = category.Name;
                    existingCategory.ModifiedDate = DateTime.Now;
                    _context.Update(existingCategory);
                    _context.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
                return NotFound();
            }
            return View(category);
        }

        public IActionResult Delete(int id)
        {
            var category = _context.ProductCategories
                .Include(c => c.InverseParentProductCategory) 
                .FirstOrDefault(c => c.ProductCategoryId == id);

            if (category == null)
            {
                return NotFound();
            }

            if (category.InverseParentProductCategory.Any()) 
            {
                ModelState.AddModelError("", "Cannot delete this category because it has subcategories. Please delete or reassign them first.");
                return View(category);
            }

            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var category = _context.ProductCategories
                .Include(c => c.InverseParentProductCategory) 
                .FirstOrDefault(c => c.ProductCategoryId == id);

            if (category == null)
            {
                return NotFound();
            }

            foreach (var subCategory in category.InverseParentProductCategory)
            {
                subCategory.ParentProductCategoryId = null; 
            }

            _context.ProductCategories.Remove(category);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
