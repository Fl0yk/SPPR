using WEB_153501_Kosach.Domain.Entities;
using WEB_153501_Kosach.Domain.Models;
using WEB_153501_Kosach.Services.FurnitureCategoryService;

namespace WEB_153501_Kosach.Services.FurnitureServices
{
    public class MemoryFurnitureService : IFurnitureService
    {
        private readonly List<FurnitureCategory> _categories;
        private List<Furniture> _furnitures;
        public MemoryFurnitureService(IFurnitureCategoryService categoryService)
        {
            _categories = categoryService.GetCategoryListAsync()
                                                .Result
                                                .Data;
            SetupData();
        }

        private void SetupData()
        {
            _furnitures = new List<Furniture>() { 
                //Диваны
                new Furniture() { Id = 1, Name = "Stoline Олимп",
                                  Image = "Images/sofas_1.jpeg",
                                  Price = 780, CategoryId = _categories.Find(c => c.NormalizedName.Equals("sofas")) },
                new Furniture() { Id = 2, Name = "Mio Tesoro",
                                  Image = "Images/sofas_2.jpeg",
                                  Price = 1150, CategoryId = _categories.Find(c => c.NormalizedName.Equals("sofas")) },
                new Furniture() { Id = 3, Name = "WoodCraft Фишер",
                                  Image = "Images/sofas_3.jpeg",
                                  Price = 1090, CategoryId = _categories.Find(c => c.NormalizedName.Equals("sofas")) },
                new Furniture() { Id = 4, Name = "KRONES Клио",
                                  Image = "Images/sofas_4.jpeg",
                                  Price = 595.24m, CategoryId = _categories.Find(c => c.NormalizedName.Equals("sofas")) },
                //Стулья
                new Furniture() { Id = 5, Name = "M-City Desert 603",
                                  Image = "Images/chairs_5.jpeg",
                                  Price = 92.92m, CategoryId = _categories.Find(c => c.NormalizedName.Equals("chairs")) },
                new Furniture() { Id = 6, Name = "Mio Tesoro Donato SC-264F",
                                  Image = "Images/chairs_6.jpeg",
                                  Price = 247.85m, CategoryId = _categories.Find(c => c.NormalizedName.Equals("chairs")) },
                new Furniture() { Id = 7, Name = "Sheffilton SHT-ST19/S29",
                                  Image = "Images/chairs_7.jpeg",
                                  Price = 116.39m, CategoryId = _categories.Find(c => c.NormalizedName.Equals("chairs")) },
                new Furniture() { Id = 8, Name = "Ника ССН1/1",
                                  Image = "Images/chairs_8.jpeg",
                                  Price = 53.22m, CategoryId = _categories.Find(c => c.NormalizedName.Equals("chairs")) },
                //Столы
                new Furniture() { Id = 9, Name = "ГМЦ Paprika 100x60",
                                  Image = "Images/tables_9.jpeg",
                                  Price = 119.47m, CategoryId = _categories.Find(c => c.NormalizedName.Equals("tables")) },
                new Furniture() { Id = 10, Name = "Eligard Black / СОБ",
                                  Image = "Images/tables_10.jpeg",
                                  Price = 362.69m, CategoryId = _categories.Find(c => c.NormalizedName.Equals("tables")) },
                new Furniture() { Id = 11, Name = "Артём-Мебель СН-005.011",
                                  Image = "Images/tables_11.jpeg",
                                  Price = 83.10m, CategoryId = _categories.Find(c => c.NormalizedName.Equals("tables")) },
                new Furniture() { Id = 12, Name = "Mio Tesoro ST-011",
                                  Image = "Images/tables_12.jpeg",
                                  Price = 119.47m, CategoryId = _categories.Find(c => c.NormalizedName.Equals("tables")) },
                //Кровати
                new Furniture() { Id = 13, Name = "Горизонт Мебель Юнона 1.6м",
                                  Image = "Images/beds_13.jpeg",
                                  Price = 179.00m, CategoryId = _categories.Find(c => c.NormalizedName.Equals("beds")) },
                new Furniture() { Id = 14, Name = "Гармония КР-601 160x200",
                                  Image = "Images/beds_14.jpeg",
                                  Price = 239.78m, CategoryId = _categories.Find(c => c.NormalizedName.Equals("beds")) },
                new Furniture() { Id = 15, Name = "Домаклево Канапе 160x200",
                                  Image = "Images/beds_15.jpeg",
                                  Price = 229.00m, CategoryId = _categories.Find(c => c.NormalizedName.Equals("beds")) },
                new Furniture() { Id = 16, Name = "Домаклево Лофт 160x200",
                                  Image = "Images/beds_16.jpeg",
                                  Price = 349.00m, CategoryId = _categories.Find(c => c.NormalizedName.Equals("beds")) }
            };
        }

        public Task<ResponseData<Furniture>> CreateFurnitureAsync(Furniture furniture, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }

        public Task DeleteFurnitureAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseData<Furniture>> GetFurnitureByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseData<ListModel<Furniture>>> GetFurnitureListAsync(string? categoryNormalizedName, int pageNo = 1)
        {
            ResponseData<ListModel<Furniture>> response = new() { Data = new() };

            try
            {
                if(categoryNormalizedName is null)
                {
                    response.Data.Items = _furnitures;
                }
                else
                {
                    response.Data.Items = _furnitures.Where(
                        f => f.CategoryId?.NormalizedName.Equals(categoryNormalizedName) ?? false)
                        .ToList();
                }
            }
            catch(Exception ex)
            {
                response.Success = false;
                response.ErrorMessage = ex.Message;
            }


            return Task.FromResult(response);
        }

        public Task UpdateFurnitureAsync(int id, Furniture furniture, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }
    }
}
