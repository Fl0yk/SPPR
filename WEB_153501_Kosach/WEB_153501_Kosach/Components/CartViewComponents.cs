using Microsoft.AspNetCore.Mvc;

namespace WEB_153501_Kosach.Components
{
    public class CartViewComponent : ViewComponent
    {
        private decimal balance;
        private int countOfProducts;

        public CartViewComponent()
        {
            balance = 0;
            countOfProducts = 0;
        }

        public IViewComponentResult Invoke()
        {
            ViewBag.Balance = balance.ToString("F2");
            ViewBag.CountOfProducts = countOfProducts;
            return View();
        }
    }
}
