using Microsoft.AspNetCore.Mvc;

namespace MvcBeginner.Controllers
{
    public class Product : Controller
    {
        public IActionResult Index()
        {
           string mes = "Đây là danh sách các sản phẩm";
            return View(model:mes);
        }
    }
}
