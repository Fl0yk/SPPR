using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WEB_153501_Kosach.API.Data;
using WEB_153501_Kosach.API.Services;
using WEB_153501_Kosach.Domain.Entities;
using WEB_153501_Kosach.Domain.Models;

namespace WEB_153501_Kosach.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FurnitureCategoriesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private IFurnitureCategoryService _categoryService;

        public FurnitureCategoriesController(IFurnitureCategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // GET: api/FurnitureCategories
        [HttpGet]
        public async Task<ActionResult<ResponseData<List<FurnitureCategory>>>> GetFurnitureCategories()
        {
          if (_categoryService is null)
          {
              return NotFound();
          }
            return Ok(await _categoryService.GetCategoryListAsync());
        }

        //// GET: api/FurnitureCategories/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<FurnitureCategory>> GetFurnitureCategory(int id)
        //{
        //  if (_context.FurnitureCategories == null)
        //  {
        //      return NotFound();
        //  }
        //    var furnitureCategory = await _context.FurnitureCategories.FindAsync(id);

        //    if (furnitureCategory == null)
        //    {
        //        return NotFound();
        //    }

        //    return furnitureCategory;
        //}

        //// PUT: api/FurnitureCategories/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutFurnitureCategory(int id, FurnitureCategory furnitureCategory)
        //{
        //    if (id != furnitureCategory.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(furnitureCategory).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!FurnitureCategoryExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //// POST: api/FurnitureCategories
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<FurnitureCategory>> PostFurnitureCategory(FurnitureCategory furnitureCategory)
        //{
        //  if (_context.FurnitureCategories == null)
        //  {
        //      return Problem("Entity set 'AppDbContext.FurnitureCategories'  is null.");
        //  }
        //    _context.FurnitureCategories.Add(furnitureCategory);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetFurnitureCategory", new { id = furnitureCategory.Id }, furnitureCategory);
        //}

        //// DELETE: api/FurnitureCategories/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteFurnitureCategory(int id)
        //{
        //    if (_context.FurnitureCategories == null)
        //    {
        //        return NotFound();
        //    }
        //    var furnitureCategory = await _context.FurnitureCategories.FindAsync(id);
        //    if (furnitureCategory == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.FurnitureCategories.Remove(furnitureCategory);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        //private bool FurnitureCategoryExists(int id)
        //{
        //    return (_context.FurnitureCategories?.Any(e => e.Id == id)).GetValueOrDefault();
        //}
    }
}
