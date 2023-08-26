using WEB_153501_Kosach.Domain.Entities;
using WEB_153501_Kosach.Domain.Models;

namespace WEB_153501_Kosach.Services.FurnitureCategoryService
{
    public class MemoryFurnitureCategoryService : IFurnitureCategoryService
    {
        public Task<ResponseData<List<FurnitureCategory>>> GetCategoryListAsync()
        {
            var categories = new List<FurnitureCategory>
            {
                new FurnitureCategory {Id=1, Name="Стулья",
            NormalizedName="chairs"},
                new FurnitureCategory {Id=2, Name="Кровати",
            NormalizedName="beds"},
                new FurnitureCategory {Id=3, Name="Столы",
            NormalizedName="tables"},
                new FurnitureCategory {Id=4, Name="Диваны",
            NormalizedName="sofas"}
            };

            var result = new ResponseData<List<FurnitureCategory>>();
            result.Data = categories;
            return Task.FromResult(result);
        }
    }
}
