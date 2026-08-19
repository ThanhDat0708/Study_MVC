using Microsoft.AspNetCore.Mvc;

namespace MvcBeginner.Controllers
{
    public class Product : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
