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
        private static string _url;
        private static string _webRootPath;

        public FurnitureService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _furnitures = dbContext.Furnitures;
        }

        public static void SetPath(WebApplication app)
        {
            _url = app.Configuration["ApiUrl"] ?? "" + "/Images/";
            _webRootPath = app.Environment.WebRootPath;
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
                _furnitures.Remove(elem);
                _dbContext.SaveChanges();
            }
        }

        public async Task<ResponseData<Furniture>> GetProductByIdAsync(int id)
        {
            var elem = await _furnitures.FirstOrDefaultAsync(x => x.Id == id);
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
            var elem = await _furnitures.FirstOrDefaultAsync(f => f.Id == id);
            string path = _webRootPath + "/Images/" + formFile.Name;
            

            if (elem is null)
            {
                return new ResponseData<string>
                {
                    Success = false,
                    ErrorMessage = "Элемент не найден"
                };
            }

            try
            {
                using (FileStream fs = new FileStream(path, FileMode.CreateNew))
                {
                    await formFile.CopyToAsync(fs);
                }

                var fullPath = _url + formFile.FileName;
                elem.Image = fullPath;
                _dbContext.SaveChanges();

                return new ResponseData<string> { Data = fullPath };
            }
            catch (Exception ex)
            {
                return new ResponseData<string> { Success = false,
                                                    ErrorMessage = ex.Message };
            }
        }

        public async Task UpdateProductAsync(int id, Furniture product)
        {
            _furnitures.Entry(product).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
    }
}
