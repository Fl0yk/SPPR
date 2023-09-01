using Azure.Core;
using System.Data.SqlTypes;
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
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                IncludeFields = true
            };
            _logger = logger;
        }


        public async Task<ResponseData<Furniture>> CreateFurnitureAsync(Furniture furniture, IFormFile? formFile)
        {
            var response = await _httpClient.PostAsJsonAsync(new Uri($"{_httpClient.BaseAddress.AbsoluteUri}furnitures"),
                                                                                                        furniture,
                                                                                                        _serializerOptions);

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var desFurniture = await response.Content
                                                .ReadFromJsonAsync<ResponseData<Furniture>>
                                                (_serializerOptions);

                    if (formFile != null)
                    {
                        await SaveImageAsync(desFurniture!.Data.Id, formFile);
                    }

                    return desFurniture;
                }
                catch (JsonException ex)
                {
                    _logger.LogError($"-----> Ошибка: {ex.Message}");
                    return new ResponseData<Furniture>
                    {
                        Success = false,
                        ErrorMessage = $"Ошибка: {ex.Message}"
                    };
                }
            }
            _logger.LogError($"-----> Данные не получены от сервера(Создание мебели). Error: {response.StatusCode}");

            return new ResponseData<Furniture>
            {
                Success = false,
                ErrorMessage = $"Данные не получены от сервера(Создание мебели). Error:{response.StatusCode}"
            };
        }

        public async Task DeleteFurnitureAsync(int id)
        {
            var response = await _httpClient.DeleteAsync(new Uri($"{_httpClient.BaseAddress.AbsoluteUri}furnitures/{id}"));

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"-----> Данные не получены от сервера(Мебель по id). Error: {response.StatusCode}");
            }
        }

        public async Task<ResponseData<Furniture>> GetFurnitureByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync(new Uri($"{_httpClient.BaseAddress.AbsoluteUri}furnitures/{id}"));
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return await response.Content
                                        .ReadFromJsonAsync<ResponseData<Furniture>>
                                        (_serializerOptions);
                }
                catch (JsonException ex)
                {
                    _logger.LogError($"-----> Ошибка: {ex.Message}");
                    return new ResponseData<Furniture>
                    {
                        Success = false,
                        ErrorMessage = $"Ошибка: {ex.Message}"
                    };
                }
            }
            _logger.LogError($"-----> Данные не получены от сервера(Мебель по id). Error: {response.StatusCode}");

            return new ResponseData<Furniture>
            {
                Success = false,
                ErrorMessage = $"Данные не получены от сервера(Мебель по id). Error:{response.StatusCode}"
            };
        }

        public async Task<ResponseData<ListModel<Furniture>>> GetFurnitureListAsync(string? categoryNormalizedName, int pageNo = 1)
        {
            // подготовка URL запроса
            var urlString = new StringBuilder($"{_httpClient.BaseAddress.AbsoluteUri}furnitures/");
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
            _logger.LogError($"-----> Данные не получены от сервера(Мебель). Error: {response.StatusCode}");

            return new ResponseData<ListModel<Furniture>>
            {
                Success = false,
                ErrorMessage = $"Данные не получены от сервера(Мебель). Error:{response.StatusCode}"
            };
        }

        public async Task UpdateFurnitureAsync(int id, Furniture furniture, IFormFile? formFile)
        {
            var response = await _httpClient.PutAsJsonAsync(new Uri($"{_httpClient.BaseAddress.AbsoluteUri}furnitures/{id}"),
                                                                                                    furniture,
                                                                                                    _serializerOptions);
            if(formFile  != null)
            {
                await SaveImageAsync(id, formFile);
            }

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"-----> Ответ не получен лт сервера(изменение мебели). Error: {response.StatusCode}");
            }
        }

        private async Task SaveImageAsync(int id, IFormFile image)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{_httpClient.BaseAddress.AbsoluteUri}Furnitures/{id}")
            };

            var content = new MultipartFormDataContent();
            var streamContent = new StreamContent(image.OpenReadStream());

            content.Add(streamContent, "formFile", image.FileName);
            request.Content = content;
            await _httpClient.SendAsync(request);
        }

    }
}
