using System.Text;
using System.Text.Json;
using WEB_153501_Kosach.Domain.Entities;
using WEB_153501_Kosach.Domain.Models;
using WEB_153501_Kosach.Services.FurnitureServices;

namespace WEB_153501_Kosach.Services.FurnitureCategoryService
{
    public class ApiCategoryService : IFurnitureCategoryService
    {
        private HttpClient _httpClient;
        private ILogger<ApiFurnitureService> _logger;
        private JsonSerializerOptions _serializerOptions;
        public ApiCategoryService(HttpClient httpClient,
                                     ILogger<ApiFurnitureService> logger)
        {
            _httpClient = httpClient;
            _serializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            _logger = logger;

        }
        public async Task<ResponseData<List<FurnitureCategory>>> GetCategoryListAsync()
        {
            // подготовка URL запроса
            var urlString = new StringBuilder($"{_httpClient.BaseAddress.AbsoluteUri}furniturecategories/");

            // отправить запрос к API
            var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));


            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return await response.Content
                    .ReadFromJsonAsync<ResponseData<List<FurnitureCategory>>>
                    (_serializerOptions);
                }
                catch (JsonException ex)
                {
                    _logger.LogError($"-----> Ошибка: {ex.Message}");
                    return new ResponseData<List<FurnitureCategory>>
                    {
                        Success = false,
                        ErrorMessage = $"Ошибка: {ex.Message}"
                    };
                }
            }
            _logger.LogError($"-----> Данные не получены от сервера(Категории). Error: {response.StatusCode}");
            return new ResponseData<List<FurnitureCategory>>
            {
                Success = false,
                ErrorMessage = $"Данные не получены от сервера. Error:{response.StatusCode}"
            };
        }
    }
}
