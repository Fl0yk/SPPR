﻿using Microsoft.AspNetCore.Mvc;
using WEB_153501_Kosach.Domain.Entities;
using WEB_153501_Kosach.Domain.Models;
using WEB_153501_Kosach.Services.FurnitureCategoryService;

namespace WEB_153501_Kosach.Services.FurnitureServices
{
    public class MemoryFurnitureService : IFurnitureService
    {
        private readonly List<FurnitureCategory> _categories;
        private List<Furniture> _furnitures;
        private readonly IConfiguration _configuration;

        public MemoryFurnitureService([FromServices] IConfiguration config, 
                                        IFurnitureCategoryService categoryService)
        {
            _configuration = config;
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
                                  Price = 780, Category = _categories.Find(c => c.NormalizedName.Equals("sofas")) },
                new Furniture() { Id = 2, Name = "Mio Tesoro",
                                  Image = "Images/sofas_2.jpeg",
                                  Price = 1150, Category = _categories.Find(c => c.NormalizedName.Equals("sofas")) },
                new Furniture() { Id = 3, Name = "WoodCraft Фишер",
                                  Image = "Images/sofas_3.jpeg",
                                  Price = 1090, Category = _categories.Find(c => c.NormalizedName.Equals("sofas")) },
                new Furniture() { Id = 4, Name = "KRONES Клио",
                                  Image = "Images/sofas_4.jpeg",
                                  Price = 595.24m, Category = _categories.Find(c => c.NormalizedName.Equals("sofas")) },
                //Стулья
                new Furniture() { Id = 5, Name = "M-City Desert 603",
                                  Image = "Images/chairs_5.jpeg",
                                  Price = 92.92m, Category = _categories.Find(c => c.NormalizedName.Equals("chairs")) },
                new Furniture() { Id = 6, Name = "Mio Tesoro Donato SC-264F",
                                  Image = "Images/chairs_6.jpeg",
                                  Price = 247.85m, Category = _categories.Find(c => c.NormalizedName.Equals("chairs")) },
                new Furniture() { Id = 7, Name = "Sheffilton SHT-ST19/S29",
                                  Image = "Images/chairs_7.jpeg",
                                  Price = 116.39m, Category = _categories.Find(c => c.NormalizedName.Equals("chairs")) },
                new Furniture() { Id = 8, Name = "Ника ССН1/1",
                                  Image = "Images/chairs_8.jpeg",
                                  Price = 53.22m, Category = _categories.Find(c => c.NormalizedName.Equals("chairs")) },
                //Столы
                new Furniture() { Id = 9, Name = "ГМЦ Paprika 100x60",
                                  Image = "Images/tables_9.jpeg",
                                  Price = 119.47m, Category = _categories.Find(c => c.NormalizedName.Equals("tables")) },
                new Furniture() { Id = 10, Name = "Eligard Black / СОБ",
                                  Image = "Images/tables_10.jpeg",
                                  Price = 362.69m, Category = _categories.Find(c => c.NormalizedName.Equals("tables")) },
                new Furniture() { Id = 11, Name = "Артём-Мебель СН-005.011",
                                  Image = "Images/tables_11.jpeg",
                                  Price = 83.10m, Category = _categories.Find(c => c.NormalizedName.Equals("tables")) },
                new Furniture() { Id = 12, Name = "Mio Tesoro ST-011",
                                  Image = "Images/tables_12.jpeg",
                                  Price = 119.47m, Category = _categories.Find(c => c.NormalizedName.Equals("tables")) },
                //Кровати
                new Furniture() { Id = 13, Name = "Горизонт Мебель Юнона 1.6м",
                                  Image = "Images/beds_13.jpeg",
                                  Price = 179.00m, Category = _categories.Find(c => c.NormalizedName.Equals("beds")) },
                new Furniture() { Id = 14, Name = "Гармония КР-601 160x200",
                                  Image = "Images/beds_14.jpeg",
                                  Price = 239.78m, Category = _categories.Find(c => c.NormalizedName.Equals("beds")) },
                new Furniture() { Id = 15, Name = "Домаклево Канапе 160x200",
                                  Image = "Images/beds_15.jpeg",
                                  Price = 229.00m, Category = _categories.Find(c => c.NormalizedName.Equals("beds")) },
                new Furniture() { Id = 16, Name = "Домаклево Лофт 160x200",
                                  Image = "Images/beds_16.jpeg",
                                  Price = 349.00m, Category = _categories.Find(c => c.NormalizedName.Equals("beds")) }
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
                List<Furniture> filterData;
                if(categoryNormalizedName is null)
                {
                    filterData = _furnitures;
                }
                else
                {
                    filterData = _furnitures.Where(
                        f => f.Category?.NormalizedName.Equals(categoryNormalizedName) ?? false)
                        .ToList();
                }

                response.Data.Items  = SelectPageElements(filterData, pageNo, out int maxPage);
                response.Data.TotalPages = maxPage;
                response.Data.CurrentPage = pageNo;
            }
            catch(Exception ex)
            {
                response.Success = false;
                response.ErrorMessage = ex.Message;
            }


            return Task.FromResult(response);
        }

        private List<Furniture> SelectPageElements(List<Furniture> filterData, int pageNo, out int maxPage)
        {
            int.TryParse(_configuration["ItemsPerPage"], out int pageSize);
            maxPage = filterData.Count / pageSize + (filterData.Count % pageSize == 0 ? 0 : 1);

            if (pageNo > maxPage)
                throw new IndexOutOfRangeException("Данной страницы не существует");

            return filterData.Skip(--pageNo * pageSize).Take(pageSize).ToList();
        }

        public Task UpdateFurnitureAsync(int id, Furniture furniture, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }
    }
}
