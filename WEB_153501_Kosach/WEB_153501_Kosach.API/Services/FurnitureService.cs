﻿using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using WEB_153501_Kosach.API.Data;
using WEB_153501_Kosach.Domain.Entities;
using WEB_153501_Kosach.Domain.Models;

namespace WEB_153501_Kosach.API.Services
{
    public class FurnitureService : IFurnitureService
    {
        private readonly int _maxPageSize = 20;
        private DbSet<Furniture> _furnitures;
        private readonly AppDbContext _dbContext;
        private static string _url;
        private static string _imagesPath;

        public FurnitureService(AppDbContext dbContext, 
                                    IConfiguration configuration,
                                    IWebHostEnvironment environment)
        {
            _dbContext = dbContext;
            _furnitures = dbContext.Furnitures;
            _url = configuration.GetSection("ApiUrl").Value!;
            _imagesPath = Path.Combine(environment.WebRootPath, "Images");
        }

        public async Task<ResponseData<Furniture>> CreateProductAsync(Furniture product)
        {
            if(product.CategoryId is null)
            {
                var tmp = _furnitures.AsQueryable().Include(p => p.CategoryId).First();
                product.CategoryId = tmp.CategoryId;
            }
            await _furnitures.AddAsync(product);
            var k = _furnitures.ToList();
            //Как будто ошибка при сохранении из-за категории
            _dbContext.SaveChanges();

            k = _furnitures.ToList();
            return new ResponseData<Furniture>() { Data = product };
        }

        public async Task DeleteProductAsync(int id)
        {
            var elem = await _furnitures.FirstOrDefaultAsync(x => x.Id == id);

            if (elem is not null)
            {
                _furnitures.Remove(elem);
                _dbContext.SaveChanges();
            }
        }

        public async Task<ResponseData<Furniture>> GetProductByIdAsync(int id)
        {
            var query = _furnitures.AsQueryable().Include(p => p.CategoryId);
            var elem = await query.FirstOrDefaultAsync(x => x.Id == id);
            ResponseData<Furniture> response = new ResponseData<Furniture>();

            if(elem is not null)
            {
                response.Data = elem;
            }
            else
            {
                response.Success = false;
                response.ErrorMessage = "Элемента с таким id не существет";
            }

            return response;
        }

        public async Task<ResponseData<ListModel<Furniture>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1, int pageSize = 3)
        {
            if (pageSize > _maxPageSize)
                pageSize = _maxPageSize;
            var query = _furnitures.AsQueryable();
            var dataList = new ListModel<Furniture>();

            query = query.Where(d => categoryNormalizedName == null 
                                || d.CategoryId!.NormalizedName.Equals(categoryNormalizedName));

            // количество элементов в списке
            var count = query.Count();
            if (count == 0)
            {
                return new ResponseData<ListModel<Furniture>>
                {
                    Data = dataList
                };
            }
            // количество страниц
            int totalPages = (int)Math.Ceiling(count / (double)pageSize);
            if (pageNo > totalPages)
                return new ResponseData<ListModel<Furniture>>
                {
                    Data = null,
                    Success = false,
                    ErrorMessage = "No such page"
                };

            dataList.Items = await query
                                .Skip((pageNo - 1) * pageSize)
                                .Take(pageSize).Include(p => p.CategoryId)
                               .ToListAsync();

            dataList.CurrentPage = pageNo;
            dataList.TotalPages = totalPages;

            var response = new ResponseData<ListModel<Furniture>>
            {
                Data = dataList
            };
            return response;

        }

        public async Task<ResponseData<string>> SaveImageAsync(int id, IFormFile formFile)
        {
            var responseData = new ResponseData<string>();
            var furniture = await _furnitures.FindAsync(id);
            if (furniture is null)
            {
                responseData.Success = false;
                responseData.ErrorMessage = "No item found";
                return responseData;
            }
            //var host = "https://" + _httpContextAccessor.HttpContext.Request.Host;
            
            if (formFile != null)
            {
                // Удалить предыдущее изображение
                if (!String.IsNullOrEmpty(furniture.Image))
                {
                    var prevImage = Path.GetFileName(furniture.Image);

                    File.Delete(Path.Combine(_imagesPath, prevImage));
                }

                // Создать имя файла
                var ext = Path.GetExtension(formFile.FileName);
                var fName = Path.ChangeExtension(Path.GetRandomFileName(), ext);

                // Сохранить файл
                using (var fs = new FileStream(Path.Combine(_imagesPath, fName), FileMode.Create))
                {
                    await formFile.CopyToAsync(fs);
                }
 
                 // Указать имя файла в объекте
                furniture.Image = $"{_url}/Images/{fName}";
                await _dbContext.SaveChangesAsync();
            }

            responseData.Data = furniture.Image;
            return responseData;

        }

        public async Task UpdateProductAsync(int id, Furniture product)
        {
            var furniture = await _furnitures.FirstOrDefaultAsync(f => f.Id == id);

            if (furniture is null)
                return;
            furniture.Price = product.Price;
            furniture.Name = product.Name;
            furniture.Image = product.Image;
            furniture.CategoryId = product.CategoryId;
            //_furnitures.Entry(furniture).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
    }
}
