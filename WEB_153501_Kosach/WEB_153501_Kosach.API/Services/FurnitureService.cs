using Azure;
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
        //private static string _url;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private string _imagesPath;

        public FurnitureService(AppDbContext dbContext,
                                    IWebHostEnvironment environment,
                                    IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _furnitures = dbContext.Furnitures;
            //_url = configuration.GetSection("ApiUrl").Value!;
            _imagesPath = Path.Combine(environment?.WebRootPath ?? "", "Images");
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ResponseData<Furniture>> CreateProductAsync(Furniture product)
        {
            await _furnitures.AddAsync(product);

            _dbContext.SaveChanges();

            return new ResponseData<Furniture>() { Data = product };
        }

        public async Task DeleteProductAsync(int id)
        {
            var elem = await _furnitures.FirstOrDefaultAsync(x => x.Id == id);

            if (elem is not null)
            {
                string fullPath = Path.Combine(_imagesPath, elem.Image ?? "");

                if(File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }

                _furnitures.Remove(elem);
                _dbContext.SaveChanges();
            }
        }

        public async Task<ResponseData<Furniture>> GetProductByIdAsync(int id)
        {
            var query = _furnitures.AsQueryable().Include(f => f.Category);
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
            var query = _furnitures.Include(f => f.Category).AsQueryable();
            //var k = _furnitures.ToList();
            var dataList = new ListModel<Furniture>();

            query = query.Where(d => categoryNormalizedName == null 
                                || d.Category!.NormalizedName.Equals(categoryNormalizedName));

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
                                .Take(pageSize)
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
            var host = "https://" + _httpContextAccessor.HttpContext.Request.Host;
            
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
                furniture.Image = $"{host}/Images/{fName}";
                await _dbContext.SaveChangesAsync();
            }

            responseData.Data = furniture.Image;
            return responseData;

        }

        public async Task UpdateProductAsync(int id, Furniture product)
        {
            var furniture = await _furnitures
                                    .Include(f => f.Category)
                                    .FirstOrDefaultAsync(f => f.Id == id);

            if (furniture is null)
                return;
            furniture.Price = product.Price;
            furniture.Name = product.Name;
            if(!String.IsNullOrEmpty(product.Image))
                furniture.Image = product.Image;
            if (product.Category is not null)
                furniture.CategoryId = product.CategoryId;
            //_furnitures.Entry(furniture).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
    }
}
