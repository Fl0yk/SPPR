using Microsoft.AspNetCore.Mvc;

namespace WEB_153501_Kosach.Components
{
    public class Cart: ViewComponent
    {
        private decimal balance;
        private int countOfProducts;

        public async Task<IViewComponentResult> InvokeAsync(string nameOfData) => nameOfData.ToLower() switch
        {
            "balance" => Content(balance.ToString("F2")),
            "count" => Content(countOfProducts.ToString()),
            _ => throw new NotImplementedException()
        };
    }
}
