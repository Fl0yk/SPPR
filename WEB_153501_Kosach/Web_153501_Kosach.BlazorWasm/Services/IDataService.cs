using WEB_153501_Kosach.Domain.Entities;

namespace Web_153501_Kosach.BlazorWasm.Services
{
    public interface IDataService
    {
        event Action DataLoaded;
        // Список категорий объектов
        List<FurnitureCategory> Categories { get; set; }
        //Список объектов
        List<Furniture> ObjectsList { get; set; }

        //Текущий объект
        Furniture CurObject { get; set; }

        // Признак успешного ответа на запрос к Api
        bool Success { get; set; }
        // Сообщение об ошибке
        string ErrorMessage { get; set; }
        // Количество страниц списка
        int TotalPages { get; set; }
        // Номер текущей страницы
        int CurrentPage { get; set; }

        /// <summary>
        /// Получение списка всех объектов
        /// </summary>
        /// <param name="categoryNormalizedName">нормализованное имя категории для фильтрации</param>
         /// <param name="pageNo">номер страницы списка</param>
         /// <returns></returns>
         public Task GetProductListAsync(string? categoryNormalizedName, int pageNo = 1);

        /// <summary>
        /// Поиск объекта по Id
        /// </summary>
        /// <param name="id">Идентификатор объекта</param>
        /// <returns>Найденный объект или null, если объект не найден</returns>
         public Task GetProductByIdAsync(int id);

        /// <summary>
        /// Получение списка категорий
        /// </summary>
        /// <returns></returns>
        public Task GetCategoryListAsync();

    }
}
