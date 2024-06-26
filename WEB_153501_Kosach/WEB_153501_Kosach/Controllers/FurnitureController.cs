﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WEB_153501_Kosach.Domain.Entities;
using WEB_153501_Kosach.Domain.Models;
using WEB_153501_Kosach.Extensions;
using WEB_153501_Kosach.Services.FurnitureCategoryService;
using WEB_153501_Kosach.Services.FurnitureServices;

namespace WEB_153501_Kosach.Controllers
{
    public class FurnitureController : Controller
    {
        private readonly IFurnitureService _furnitureService;
        private readonly IFurnitureCategoryService _furnitureCategoryService;
        public FurnitureController(IFurnitureService furnitureService, 
                                    IFurnitureCategoryService categoryService) 
        {
            _furnitureService = furnitureService;
            _furnitureCategoryService = categoryService;
        }

        [Route("Catalog")]
        [Route("Catalog/{category}")]
        public  async Task<ActionResult> Index(int? pageno, string? category)
        {
            var productResponse = await _furnitureService.GetFurnitureListAsync(category, pageno ?? 1);
            ResponseData<List<FurnitureCategory>> categories = await _furnitureCategoryService.GetCategoryListAsync();

            if (!categories.Success)
                return NotFound(categories.ErrorMessage);

            ViewData["currentCategory"] = categories.Data
                                            .FirstOrDefault(c => c.NormalizedName.Equals(category))?
                                            .Name ?? "Все";

            ViewData["category"] = category;

            ViewData["categories"] = categories.Data;

            if (!productResponse.Success)
                return NotFound(productResponse.ErrorMessage);

            if(Request.IsAjaxRequest())
            {
                return PartialView("_FurnituriesPartial", productResponse.Data);
            }

            return View("CartView", productResponse.Data);
        }
    }
}
