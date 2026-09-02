using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcBeginner.Models;
using System.Threading.Tasks;
using MvcBeginner.Models.ViewModels;
namespace MvcBeginner.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDataContext _db;
        public ProductController(AppDataContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            //var products = await _db.Products.ToListAsync();
            var products = await _db.Products
               .Select(x => new ProductViewModels
               {
                   Id = x.Id,
                   Name = x.Name,
                   Price = x.Price,
                   Stock = x.Stock,
                   CategoryName = x.Category.Name,
                   SupplierName = x.Supplier.Name
               })
            .ToListAsync();
            return View(products);
        }
        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateViewModel model)
        {
            var products = new Product
            {
                Id = model.Id,
                Name = model.Name,
                Price = model.Price,
                Stock = model.Stock,
                CategoryId = model.CategoryId,
                SupplierId = model.SupplierId
            };
            _db.Products.Add(products);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _db.Categories.ToListAsync();
            ViewBag.Suppliers = await _db.Suppliers.ToListAsync();
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            var product = await _db.Products.FindAsync(Id);
            if (product == null)
            {
                return NotFound();
            }
            var model = new ProductEditViewModels
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock,
                CategoryId = product.CategoryId,
                SupplierId = product.SupplierId
            };
            ViewBag.Categories = await _db.Categories.ToListAsync();
            ViewBag.Suppliers = await _db.Suppliers.ToListAsync();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(ProductEditViewModels model)
        {
            var product = await _db.Products.FindAsync(model.Id);
            if (product == null)
            {
                return NotFound();
            }
            product.Name = model.Name;
            product.Price = model.Price;
            product.Stock = model.Stock;
            product.CategoryId = model.CategoryId;
            product.SupplierId = model.SupplierId;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");

        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _db.Products.FindAsync(id);
            if(product == null)
            {
                return NotFound();
            }
            return View(product);
        }
        [HttpPost]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            var product = await _db.Products.FindAsync(id);
            if(product == null)
            {
                return NotFound();
            }
            _db.Products.Remove(product);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");

        }
    }
    
}
