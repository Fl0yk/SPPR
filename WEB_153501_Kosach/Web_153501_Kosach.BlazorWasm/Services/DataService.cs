using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using WEB_153501_Kosach.Domain.Entities;
using WEB_153501_Kosach.Domain.Models;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System.Net.Http.Json;

namespace Web_153501_Kosach.BlazorWasm.Services
{
    public class DataService : IDataService
    {
        public List<FurnitureCategory> Categories { get ; set; }
        public List<Furniture> ObjectsList { get; set; }
        public Furniture CurObject { get; set; }
        public bool Success { get; set; } = false;
        public string ErrorMessage { get; set; } = "";
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }

        private readonly HttpClient _httpClient;
        private readonly IAccessTokenProvider _tokenProvider;
        private readonly JsonSerializerOptions _serializerOptions;
        private readonly string _pageSize;

        public event Action DataLoaded;

        public DataService(HttpClient httpClient,
                            IAccessTokenProvider tokenProvider,
                            IConfiguration configuration)
        {
            _httpClient = httpClient;
            _tokenProvider = tokenProvider;
            _pageSize = configuration.GetSection("ItemsPerPage").Value;
            _serializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                IncludeFields = true
            };
        } 

        public async Task GetCategoryListAsync()
        {
            // подготовка URL запроса
            var urlString = new StringBuilder($"{_httpClient.BaseAddress.AbsoluteUri}furniturecategories/");

            var tokenRequest = await _tokenProvider.RequestAccessToken();
            if (tokenRequest.TryGetToken(out var token))
            {
                _httpClient.DefaultRequestHeaders.Authorization
                            = new AuthenticationHeaderValue("bearer", token.Value);
                // отправить запрос к API
                var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));

                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        ResponseData<List<FurnitureCategory>> result = await response.Content
                                        .ReadFromJsonAsync<ResponseData<List<FurnitureCategory>>>
                                        (_serializerOptions) ?? throw new ArgumentNullException();
                        Categories = result.Data;
                        Console.WriteLine("Init categories");
                        Success = true;
                        DataLoaded?.Invoke();
                    }
                    catch (JsonException ex)
                    {

                        Success = false;
                        ErrorMessage = $"Ошибка: {ex.Message}";
                    }
                }
                else
                {
                    Success = false;
                    ErrorMessage = $"Данные не получены от сервера(Категории). Error:{response.StatusCode}";
                }
            }
            else
            {
                Success = false;
                ErrorMessage = $"Не удалось получить токен";
            }
        }

        public async Task GetProductByIdAsync(int id)
        {
            // подготовка URL запроса
            var urlString = new StringBuilder($"{_httpClient.BaseAddress.AbsoluteUri}furnitures/{id}");

            var tokenRequest = await _tokenProvider.RequestAccessToken();
            if (tokenRequest.TryGetToken(out var token))
            {
                _httpClient.DefaultRequestHeaders.Authorization
                            = new AuthenticationHeaderValue("bearer", token.Value);
                // отправить запрос к API
                var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));

                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var result = await response.Content
                                        .ReadFromJsonAsync<ResponseData<Furniture>>
                                        (_serializerOptions) ?? throw new ArgumentNullException();
                        CurObject = result.Data;
                        //Categories = result.Data;
                        Success = true;
                        DataLoaded?.Invoke();
                    }
                    catch (JsonException ex)
                    {

                        Success = false;
                        ErrorMessage = $"Ошибка: {ex.Message}";
                    }
                }
                else
                {
                    Success = false;
                    ErrorMessage = $"Данные не получены от сервера(Мебель по id). Error:{response.StatusCode}";
                }
            }
            else
            {
                Success = false;
                ErrorMessage = $"Не удалось получить токен";
            }
        }

        public async Task GetProductListAsync(string? categoryNormalizedName, int pageNo = 1)
        {
            // подготовка URL запроса
            var urlString = new StringBuilder($"{_httpClient.BaseAddress.AbsoluteUri}furnitures/");
            // добавить категорию в маршрут
            if (categoryNormalizedName != null)
            {
                urlString.Append($"{categoryNormalizedName}/");
            }
            // добавить номер страницы в маршрут
            if (pageNo > 1)
            {
                urlString.Append($"pageno={pageNo}/");
            }
            // добавить размер страницы в строку запроса
            if (!_pageSize.Equals("3"))
            {
                urlString.Append($"pagesize={_pageSize}/");
            }

            var tokenRequest = await _tokenProvider.RequestAccessToken();
            if (tokenRequest.TryGetToken(out var token))
            {
                _httpClient.DefaultRequestHeaders.Authorization
                            = new AuthenticationHeaderValue("bearer", token.Value);
                //отправить запрос к API
                
                var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));

                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        ResponseData<ListModel<Furniture>> result = await response.Content
                                        .ReadFromJsonAsync<ResponseData<ListModel<Furniture>>>
                                        (_serializerOptions) ?? throw new ArgumentNullException();
                        ObjectsList = result.Data.Items;
                        Success = true;
                        TotalPages = result.Data.TotalPages;
                        CurrentPage = result.Data.CurrentPage;
                        DataLoaded?.Invoke();
                    }
                    catch (JsonException ex)
                    {

                        Success = false;
                        ErrorMessage = $"Ошибка1: {ex.Message}";
                    }
                    catch(ArgumentNullException ex)
                    {
                        Success = false;
                        ErrorMessage = $"Ошибка2: {ex.Message}";
                    }
                }
                else
                {
                    Success = false;
                    ErrorMessage = $"Данные не получены от сервера(Мебель). Error:{response.StatusCode}";
                }

            }
            else
            {
                Success = false;
                ErrorMessage = $"Не удалось получить токен";
            }
        }
    }
}
