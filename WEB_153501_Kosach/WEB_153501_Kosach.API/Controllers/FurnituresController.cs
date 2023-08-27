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
    public class FurnituresController : ControllerBase
    {
        private IFurnitureService _furnitureService;

        public FurnituresController(IFurnitureService furnitureService)
        {
            _furnitureService = furnitureService;
        }

        // GET: api/Furnitures
        [HttpGet]
        [Route("")]
        [Route("{pageno=1}/{pagesize=3}/")]
        public async Task<ActionResult<ResponseData<ListModel<Furniture>>>> GetFurnitures(string? category, 
                                                                                int pageNo = 1, 
                                                                                int pageSize = 3)
        {
            return Ok(await _furnitureService.GetProductListAsync(category, pageNo, pageSize));
        }

        // GET: api/Furnitures/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseData<Furniture>>> GetFurniture(int id)
        {
            return Ok(await _furnitureService.GetProductByIdAsync(id));
        }

        // PUT: api/Furnitures/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<ResponseData<Furniture>>> PutFurniture(int id, Furniture furniture)
        {
            if (id != furniture.Id)
            {
                return BadRequest();
            }

            await _furnitureService.UpdateProductAsync(id, furniture);
            return NoContent();
        }

        // POST: api/Furnitures
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ResponseData<Furniture>>> PostFurniture(Furniture furniture)
        {
          if (_furnitureService is null)
          {
              return Problem("Entity set 'FurnitureService'  is null.");
          }
          await _furnitureService.CreateProductAsync(furniture);

            return CreatedAtAction("GetFurniture", new { id = furniture.Id }, furniture);
        }

        // DELETE: api/Furnitures/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ResponseData<Furniture>>> DeleteFurniture(int id)
        {
            try
            {
                await _furnitureService.DeleteProductAsync(id);

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
