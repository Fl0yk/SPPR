﻿using WEB_153501_Kosach.Domain.Entities;
using WEB_153501_Kosach.Domain.Models;

namespace WEB_153501_Kosach.Services.FurnitureCategoryService
{
    public interface IFurnitureCategoryService
    {

        /// <summary>
        /// Получение списка всех категорий
        /// </summary>
        /// <returns></returns>
        public Task<ResponseData<List<FurnitureCategory>>> GetCategoryListAsync();
    }
}
