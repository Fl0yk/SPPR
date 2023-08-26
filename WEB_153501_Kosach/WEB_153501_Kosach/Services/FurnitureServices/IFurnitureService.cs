﻿using WEB_153501_Kosach.Domain.Entities;
using WEB_153501_Kosach.Domain.Models;

namespace WEB_153501_Kosach.Services.FurnitureServices
{
    public interface IFurnitureService
    {
        /// <summary>
        /// Получение списка всех объектов
        /// </summary>
        /// <param name="categoryNormalizedName">нормализованное имя категории для фильтрации</param>
        /// <param name="pageNo">номер страницы списка</param>
        /// <returns></returns>
        public Task<ResponseData<ListModel<Furniture>>> GetFurnitureListAsync(string? categoryNormalizedName, int pageNo = 1);

        /// <summary>
        /// Поиск объекта по Id
        /// </summary>
        /// <param name="id">Идентификатор объекта</param>
        /// <returns>Найденный объект или null, если объект не найден</returns>
        public Task<ResponseData<Furniture>> GetFurnitureByIdAsync(int id);

        /// <summary>
        /// Обновление объекта
        /// </summary>
        /// <param name="id">Id изменяемомго объекта</param>
        /// <param name="furniture">объект с новыми параметрами</param>
        /// <param name="formFile">Файл изображения</param>
        /// <returns></returns>
        public Task UpdateFurnitureAsync(int id, Furniture furniture, IFormFile? formFile);

        /// <summary>
        /// Удаление объекта
        /// </summary>
        /// <param name="id">Id удаляемомго объекта</param>
        /// <returns></returns>
        public Task DeleteFurnitureAsync(int id);

        /// <summary>
        /// Создание объекта
        /// </summary>
        /// <param name="furniture">Новый объект</param>
        /// <param name="formFile">Файл изображения</param>
        /// <returns>Созданный объект</returns>
        public Task<ResponseData<Furniture>> CreateFurnitureAsync(Furniture furniture, IFormFile? formFile);

    }
}
