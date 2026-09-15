using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcBeginner.Models;
using System.Threading.Tasks;
using MvcBeginner.Models.ViewModels;
using System.Runtime.InteropServices;
using MvcBeginner.Services;
namespace MvcBeginner.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDataContext _db;
        public readonly ProductService _productService;
        public ProductController(AppDataContext db, ProductService productService)
        {
            _db = db;
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            ////var products = await _db.Products.ToListAsync();
            //var products = await _db.Products
            //   .Select(x => new ProductViewModels
            //   {
            //       Id = x.Id,
            //       Name = x.Name,
            //       Price = x.Price,
            //       Stock = x.Stock,
            //       CategoryName = x.Category.Name,
            //       SupplierName = x.Supplier.Name
            //   })
            //.ToListAsync();
            //return View(products);
            var products = await _productService.GetAllAsync();
            return View(products);
        }
        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateViewModel model)
        {
            if(!ModelState.IsValid)
            {
                ViewBag.Categories = await _db.Categories.ToListAsync();
                ViewBag.Suppliers = await _db.Suppliers.ToListAsync();
                return View(model);
            }
            if(await _db.Products.AnyAsync(x=> x.Name == model.Name))
            {
                ModelState.AddModelError("Name", "Tên sản phẩm đã tồn tại");
                ViewBag.Categories = await _db.Categories.ToListAsync();
                ViewBag.Suppliers = await _db.Suppliers.ToListAsync();
                return View(model);
            }
          
            var products = new Product
            {
                Name = model.Name,
                Price = model.Price!.Value,
                Stock = model.Stock!.Value,
                CategoryId = model.CategoryId!.Value,
                SupplierId = model.SupplierId!.Value
            };
            await _productService.CreateAsync(products);
            TempData["Success"] = "Thêm sản phẩm thành công";
          
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
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _db.Categories.ToListAsync();
                ViewBag.Suppliers = await _db.Suppliers.ToListAsync();
                return View(model);
            }
           if (await _db.Products.AnyAsync(x=>x.Name == model.Name && x.Id != model.Id))
            {
                ModelState.AddModelError("Name", "Tên sản phẩm đã tồn tại");
                ViewBag.Categories = await _db.Categories.ToListAsync();
                ViewBag.Suppliers = await _db.Suppliers.ToListAsync();
                return View(model);

            }
            var product = await _db.Products.FindAsync(model.Id);
            if (product == null)
            {
                return NotFound();
            }
            product.Name = model.Name;
            product.Price = model.Price ?? 0;
            product.Stock = model.Stock ?? 0;
            product.CategoryId = model.CategoryId ?? 0;
            product.SupplierId = model.SupplierId ?? 0;
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
