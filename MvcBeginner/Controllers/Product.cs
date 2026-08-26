using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcBeginner.Models;
using System.Threading.Tasks;
namespace MvcBeginner.Controllers
{
    public class Product : Controller
    {
        private readonly AppDataContext _db;
        public Product(AppDataContext db)
        {
            _db = db;
        }
        
        public async Task<IActionResult> Index()
        {
            var products = await _db.Products.ToListAsync();

            return View(model: products);
        }
    }
}
