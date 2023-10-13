using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using WEB_153501_Kosach.Domain.Models;

namespace WEB_153501_Kosach.Components
{
    public class CartViewComponent : ViewComponent
    {
        private readonly Cart _cart;

        public CartViewComponent(Cart cart)
        {
            _cart = cart;
        }

        public IViewComponentResult Invoke()
        {
            return View(_cart);
        }
    }
}
