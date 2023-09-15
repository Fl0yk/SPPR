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
        [Route("{category}/pageno={pageno:int}/pagesize={pagesize:int}")]
        [Route("{category}/pageno={pageno:int}")]
        [Route("{category}/pagesize={pagesize:int}")]
        [Route("pageno={pageno:int}/pagesize={pagesize:int}")]
        [Route("pageno={pageno:int}")]
        [Route("{category}")]
        public async Task<ActionResult<ResponseData<ListModel<Furniture>>>> GetFurnitures(string? category, 
                                                                                int pageNo = 1, 
                                                                                int pageSize = 3)
        {
            return Ok(await _furnitureService.GetProductListAsync(category, pageNo, pageSize));
        }

        // GET: api/Furnitures/5
        [HttpGet("{id:int}")]
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
              return Problem("Service 'FurnitureService'  is null.");
          }

            return Ok(await _furnitureService.CreateProductAsync(furniture));
        }

        // POST: api/Furnitures/5
        [HttpPost("{id}")]
        public async Task<ActionResult<ResponseData<string>>> PostImage(int id,
                                                                            IFormFile formFile)
        {
            var response = await _furnitureService.SaveImageAsync(id, formFile);
            if (response.Success)
            {
                return Ok(response);
            }
            return NotFound(response);
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
