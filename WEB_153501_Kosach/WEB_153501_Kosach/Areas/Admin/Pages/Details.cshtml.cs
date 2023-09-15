using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WEB_153501_Kosach.API.Data;
using WEB_153501_Kosach.Domain.Entities;
using WEB_153501_Kosach.Services.FurnitureServices;

namespace WEB_153501_Kosach.Areas.Admin
{
    public class DetailsModel : PageModel
    {
        private readonly IFurnitureService _furnitureService;

        public DetailsModel(IFurnitureService service)
        {
            _furnitureService = service;
        }

        public Furniture Furniture { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _furnitureService == null)
            {
                return NotFound();
            }

            var furniture = await _furnitureService.GetFurnitureByIdAsync(id ?? -1);
            if (furniture.Success)
            {
                Furniture = furniture.Data;
            }
            else 
            {
                return NotFound();
            }

            return Page();
        }
    }
}
