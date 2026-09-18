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
        //private readonly AppDataContext _db;
        public readonly ProductService _productService;
        public ProductController(AppDataContext db, ProductService productService)
        {
            //_db = db;
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
                ViewBag.Categories = await _productService.GetCategoriesAsync();
                ViewBag.Suppliers = await _productService.GetSuppliersAsync();
                return View(model);
            }
            if(await _productService.IsNameCreateAsync(model.Name))
            {
                ModelState.AddModelError("Name", "Tên sản phẩm đã tồn tại");
                ViewBag.Categories = await _productService.GetCategoriesAsync();
                ViewBag.Suppliers = await _productService.GetSuppliersAsync();
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
            //ViewBag.Categories = await _db.Categories.ToListAsync();
            //ViewBag.Suppliers = await _db.Suppliers.ToListAsync();
            ViewBag.Categories = await _productService.GetCategoriesAsync();
            ViewBag.Suppliers = await _productService.GetSuppliersAsync();
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            //            var product = await _db.Products.FindAsync(Id); khi chua dung service
            var product = await _productService.GetByIdAsync(Id);
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
             
            //ViewBag.Categories = await _db.Categories.ToListAsync();
            //ViewBag.Suppliers = await _db.Suppliers.ToListAsync();
            ViewBag.Categories = await _productService.GetCategoriesAsync();
            ViewBag.Suppliers = await _productService.GetSuppliersAsync();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(ProductEditViewModels model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _productService.GetCategoriesAsync();
                ViewBag.Suppliers = await _productService.GetSuppliersAsync();
                return View(model);
            }
           if (await _productService.IsNameExistsAsync(model.Name, model.Id))
            {
                ModelState.AddModelError("Name", "Tên sản phẩm đã tồn tại");
                ViewBag.Categories = await _productService.GetCategoriesAsync();
                ViewBag.Suppliers = await _productService.GetSuppliersAsync();
                return View(model);

            }
            var product = new Product
            {
                Id = model.Id,
                Name = model.Name,
                Price = model.Price!.Value,
                Stock = model.Stock!.Value,
                CategoryId = model.CategoryId!.Value,
                SupplierId = model.SupplierId!.Value
            };

            var result = await _productService.EditAsync(product);

            if (!result)
            {
                return NotFound();
            }
           
            return RedirectToAction("Index");

        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            if(product == null)
            {
                return NotFound();
            }
            return View(product);
        }
        [HttpPost]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            var result = await _productService.DeleteAsync(id);
            if (!result)
            {
                return NotFound();
            }
            
            return RedirectToAction("Index");

        }
    }
    
}
