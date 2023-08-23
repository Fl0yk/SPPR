using Microsoft.AspNetCore.Mvc;

namespace WEB_153501_Kosach.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
