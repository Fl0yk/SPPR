using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WEB_153501_Kosach.API.Data;
using WEB_153501_Kosach.Domain.Entities;
using WEB_153501_Kosach.Domain.Models;
using WEB_153501_Kosach.Services.FurnitureCategoryService;
using WEB_153501_Kosach.Services.FurnitureServices;

namespace WEB_153501_Kosach.Areas.Admin
{
    public class IndexModel : PageModel
    {
        private readonly IFurnitureService _furnitureService;
        private readonly IFurnitureCategoryService _categoryService;

        public IndexModel(IFurnitureService service, IFurnitureCategoryService categoryService)
        {
            _furnitureService = service;
            _categoryService = categoryService;
        }

        public IList<FurnitureCategory> Categories { get; set; } = default!;

        public IList<Furniture> Furniture { get;set; } = default!;

        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public string? Category { get; set; }
        public string CurCategory { get; set; }

        public async Task OnGetAsync(int? pageno, string? category)
        {
            var requestFurnitures = await _furnitureService.GetFurnitureListAsync(category, pageno ?? 1);
            ResponseData<List<FurnitureCategory>> requestCategories = await _categoryService.GetCategoryListAsync();
            
            if (requestFurnitures.Success 
                    && requestCategories.Success)
            {
                Furniture = requestFurnitures.Data.Items;
                TotalPages = requestFurnitures.Data.TotalPages;
                CurrentPage = requestFurnitures.Data.CurrentPage;

                Categories = requestCategories.Data;
                Category = Request.Query["category"].ToString();
                CurCategory = Categories
                            .FirstOrDefault(c => c.NormalizedName == Category)?
                            .Name ?? "Все";
            }
        }
    }
}
