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

        [Route("Test")]
        [Route("Test/{name}/{age:int}")]
        [Route("Test/{name}")]
        [Route("Test/age={age:int}")]
        
        public string Test(string? name, int age)
        {
            string output;
            if (age > 0 && name is not null)
                output = name + ", age " + age;
            else if (age > 0)
                output = "Age " + age.ToString();
            else if (name is not null)
                output = name;
            else
                output = "Test";

            return output;
        }
    }
}
