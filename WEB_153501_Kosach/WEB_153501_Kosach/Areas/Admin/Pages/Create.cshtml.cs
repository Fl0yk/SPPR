﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WEB_153501_Kosach.API.Data;
using WEB_153501_Kosach.Domain.Entities;
using WEB_153501_Kosach.Domain.Models;
using WEB_153501_Kosach.Services.FurnitureCategoryService;
using WEB_153501_Kosach.Services.FurnitureServices;

namespace WEB_153501_Kosach.Areas.Admin
{
    [Authorize(Roles = "admin")]
    public class CreateModel : PageModel
    {
        private readonly IFurnitureService _furnitureService;
        private readonly IFurnitureCategoryService _categoryService;

        public CreateModel(IFurnitureService service, IFurnitureCategoryService categoryService)
        {
            _furnitureService = service;
            _categoryService = categoryService;

        }

        public async Task<IActionResult> OnGet()
        {
            ResponseData<List<FurnitureCategory>> requestCategories = await _categoryService.GetCategoryListAsync();
            if(requestCategories.Success)
            {
                Categories = new SelectList(requestCategories.Data, "Id", "Name");
            }

            return Page();
        }

        public SelectList Categories { get; set; }

        [BindProperty]
        public Furniture Furniture { get; set; } = default!;

        [BindProperty]
        public IFormFile? Image { get; set; }


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            ResponseData<List<FurnitureCategory>> requestCategories = await _categoryService.GetCategoryListAsync();
            if (!ModelState.IsValid || _furnitureService is null || Furniture == null)
            {
                if (requestCategories.Success)
                {
                    Categories = new SelectList(requestCategories.Data, "Id", "Name");
                }
                return Page();
            }

            await _furnitureService.CreateFurnitureAsync(Furniture, Image);

            return RedirectToPage("./Index");
        }
    }
}
