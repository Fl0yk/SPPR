using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WEB_153501_Kosach.Domain.Models;
using WEB_153501_Kosach.Services.FurnitureServices;

namespace WEB_153501_Kosach.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly IFurnitureService _furnitureService;
        private readonly Cart _cart;
        public CartController(IFurnitureService productService,
                                    Cart cart)
        {
            _furnitureService = productService;
            _cart = cart ?? throw new ArgumentNullException();
        }

        public ActionResult Index()
        {

            return View(_cart.CartItems);
        }

        public ActionResult Delete(int id)
        {
            _cart.RemoveItems(id);

            return RedirectToAction(nameof(Index));
        }

        public ActionResult ClearAll()
        {
            _cart.ClearAll();

            return RedirectToAction(nameof(Index));
        }

        [Route("[controller]/add/{id:int}")]
        public async Task<ActionResult> Add(int id, string returnUrl)
        {
            var data = await _furnitureService.GetFurnitureByIdAsync(id);
            if (data.Success)
            {
                _cart.AddToCart(data.Data);
            }

            return Redirect(returnUrl);
        }
    }
}
