using System.Net.Http;
using System.Text;
using System.Text.Json;
using WEB_153501_Kosach.Domain.Entities;
using WEB_153501_Kosach.Domain.Models;

namespace WEB_153501_Kosach.Services.FurnitureServices
{
    public class ApiFurnitureService : IFurnitureService
    {
        private HttpClient _httpClient;
        private ILogger<ApiFurnitureService> _logger;
        private string _pageSize;
        private JsonSerializerOptions _serializerOptions;
        public ApiFurnitureService(HttpClient httpClient,
                                     IConfiguration configuration,
                                     ILogger<ApiFurnitureService> logger) 
        {
            _httpClient = httpClient;
            _pageSize = configuration.GetSection("ItemsPerPage").Value;
            _serializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            _logger = logger;
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

        public async Task<ResponseData<ListModel<Furniture>>> GetFurnitureListAsync(string? categoryNormalizedName, int pageNo = 1)
        {
            // подготовка URL запроса
            var urlString  = new StringBuilder($"{_httpClient.BaseAddress.AbsoluteUri}furnitures/");
            // добавить категорию в маршрут
            if (categoryNormalizedName != null)
            {
                urlString.Append($"{categoryNormalizedName}/");
            };
            // добавить номер страницы в маршрут
            if (pageNo > 1)
            {
                urlString.Append($"pageno={pageNo}/");
            };
            // добавить размер страницы в строку запроса
            if (!_pageSize.Equals("3"))
            {
                urlString.Append(QueryString.Create("pageSize", _pageSize));
            }

            // отправить запрос к API
            var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return await response.Content
                    .ReadFromJsonAsync<ResponseData<ListModel<Furniture>>>
                    (_serializerOptions);
                }
                catch (JsonException ex)
                {
                    _logger.LogError($"-----> Ошибка: {ex.Message}");
                    return new ResponseData<ListModel<Furniture>>
                    {
                        Success = false,
                        ErrorMessage = $"Ошибка: {ex.Message}"
                    };
                }
            }
            _logger.LogError($"-----> Данные не получены от сервера(Мебель). Error: { response.StatusCode}");
                return new ResponseData<ListModel<Furniture>>
                {
                    Success = false,
                    ErrorMessage = $"Данные не получены от сервера(Мебель). Error:{ response.StatusCode}"
                };
        }

        public Task UpdateFurnitureAsync(int id, Furniture furniture, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }
    }
}
