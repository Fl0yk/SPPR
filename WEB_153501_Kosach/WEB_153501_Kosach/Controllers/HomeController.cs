using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WEB_153501_Kosach.Models;

namespace WEB_153501_Kosach.Controllers
{
    public class HomeController : Controller
    {
        private SelectList selectList = new SelectList(new List<ListDemo>()
        {
            new ListDemo(){Id = 1, Name = "Element 1"},
            new ListDemo(){Id = 2, Name = "Element 2"},
            new ListDemo(){Id = 3, Name = "Element 3"}
        }, "Id", "Name");

        public IActionResult Index()
        {
            return View(selectList);
        }
    }
}
