using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WEB_153501_Kosach.API.Data;
using WEB_153501_Kosach.Domain.Entities;
using WEB_153501_Kosach.Services.FurnitureServices;

namespace WEB_153501_Kosach.Areas.Admin
{
    public class EditModel : PageModel
    {
        private readonly IFurnitureService _furnitureService;

        public EditModel(IFurnitureService service)
        {
            _furnitureService = service;
        }

        [BindProperty]
        public Furniture Furniture { get; set; } = default!;

        [BindProperty]
        public IFormFile? Image { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _furnitureService == null)
            {
                return NotFound();
            }

            var furniture =  await _furnitureService.GetFurnitureByIdAsync(id ?? -1);
            if (!furniture.Success)
            {
                return NotFound();
            }
            Furniture = furniture.Data;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _furnitureService.UpdateFurnitureAsync(Furniture.Id, Furniture, Image);

            return RedirectToPage("./Index");
        }
    }
}
