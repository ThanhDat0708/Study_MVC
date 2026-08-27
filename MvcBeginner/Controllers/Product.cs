using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcBeginner.Models;
using System.Threading.Tasks;
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
                .Include(x => x.Category)
                .Include(x=>x.Supplier)
                .ToListAsync();

            return View(model: products);
        }
    }
}
