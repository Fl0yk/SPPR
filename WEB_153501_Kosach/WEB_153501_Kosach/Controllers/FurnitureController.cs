using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WEB_153501_Kosach.Services.FurnitureCategoryService;
using WEB_153501_Kosach.Services.FurnitureServices;

namespace WEB_153501_Kosach.Controllers
{
    public class FurnitureController : Controller
    {
        private readonly IFurnitureService _furnitureService;
        private readonly IFurnitureCategoryService _furnitureCategoryService;
        public FurnitureController(IFurnitureService furnitureService, IFurnitureCategoryService categoryService) {
            _furnitureService = furnitureService;
            _furnitureCategoryService = categoryService;
        }

        public  async Task<ActionResult> Index()
        {
            var productResponse = await _furnitureService.GetFurnitureListAsync(null);

            if (!productResponse.Success)
                return NotFound(productResponse.ErrorMessage);

            return View(productResponse.Data.Items);
        }
    }
}
