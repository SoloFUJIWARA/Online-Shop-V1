using Microsoft.AspNetCore.Mvc;
using OnlineShop.Data;
using OnlineShop.Models;
using OnlineShop.ViewModels;

namespace OnlineShop.Controllers;

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
            .AsEnumerable()
            .Select(c => new ProductCategoryViewModel
            {
                Id = c.ProductCategoryId,
                Name = c.Name,
                ModifiedDate = c.ModifiedDate,
                ProductCount = _context.Products.AsEnumerable()
                    .Count(p => p.ProductCategoryId == c.ProductCategoryId)
            })
            .ToList();
        
        return View(categories);
    }
    
    public IActionResult Details(int id)
    {
        var category = _context.ProductCategories.Find(id);
        if (category == null)
        {
            return NotFound();
        }
        
        var products = _context.Products.AsEnumerable()
            .Where(p => p.ProductCategoryId == category.ProductCategoryId)
            .Select(p => new ProductViewModel
            {
                ProductId = p.ProductId,
                Name = p.Name,
                ProductNumber = p.ProductNumber,
                Color = p.Color,
                ListPrice = p.ListPrice,
                ModifiedDate = p.ModifiedDate,
                OrderCount = _context.SalesOrderDetails.AsEnumerable()
                    .Count(o => o.ProductId == p.ProductId)
            })
            .ToList();
        
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
    public IActionResult Edit([Bind("Id,Name")] ProductCategory category)
    {
        if (ModelState.IsValid)
        {
            _context.Update(category);
            _context.SaveChanges();
            
            return RedirectToAction(nameof(Index));
        }
        return View(category);
    }
    
    
    public IActionResult Delete(int id)
    {
        var category = _context.ProductCategories.Find(id);
        if (category == null)
        {
            return NotFound();
        }
        return View(category);
    }
    
    [HttpPost, ActionName("Delete")]
    public IActionResult DeleteConfirmed(int id)
    {
        var category = _context.ProductCategories.Find(id);
        _context.ProductCategories.Remove(category);
        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }
}