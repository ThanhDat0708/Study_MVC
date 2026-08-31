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
                Stock = model.Stock
            };
            _db.Products.Add(products);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
           
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
    }
}
