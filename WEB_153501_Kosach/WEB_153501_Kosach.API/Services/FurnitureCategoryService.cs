using Microsoft.EntityFrameworkCore;
using WEB_153501_Kosach.API.Data;
using WEB_153501_Kosach.Domain.Entities;
using WEB_153501_Kosach.Domain.Models;

namespace WEB_153501_Kosach.API.Services
{
    public class FurnitureCategoryService : IFurnitureCategoryService
    {
        private readonly DbSet<FurnitureCategory> _categories;

        public FurnitureCategoryService(AppDbContext context)
        {
            _categories = context.FurnitureCategories;
        }

        public async Task<ResponseData<List<FurnitureCategory>>> GetCategoryListAsync()
        {
            ResponseData<List<FurnitureCategory>> response = new();
            try
            {
                response.Data = await _categories.ToListAsync();
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessage = ex.Message;
            }

            return response;
        }
    }
}
