using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEB_153501_Kosach.Domain.Entities;

namespace WEB_153501_Kosach.Domain.Models
{
    public class Cart
    {
        /// <summary>
        /// Список объектов в корзине
        /// key - идентификатор объекта
        /// </summary>
        public Dictionary<int, CartItem> CartItems { get; set; } = new();

        /// <summary>
        /// Добавить объект в корзину
        /// </summary>
        /// <param name="furniture">Добавляемый объект</param>
        public virtual void AddToCart(Furniture furniture)
        {
            if (CartItems.ContainsKey(furniture.Id))
            {
                CartItems[furniture.Id].Count++;
            }
            else
            {
                CartItems.Add(furniture.Id,
                    new CartItem() { Item = furniture, Count = 1 }
                    );
            }
        }

        /// <summary>
        /// Удалить объект из корзины
        /// </summary>
        /// <param name="id"> id удаляемого объекта</param>
        public virtual void RemoveItems(int id)
        {
            CartItems.Remove(id);
        }

        /// <summary>
        /// Очистить корзину
        /// </summary>
        public virtual void ClearAll()
        {
            CartItems.Clear();
        }

        /// <summary>
        /// Количество объектов в корзине
        /// </summary>
        public int Count { get => CartItems.Sum(item => item.Value.Count); }

        /// <summary>
        /// Общее количество калорий
        /// </summary>
        public decimal TotalPrice
        {
            get => CartItems.Sum(item => item.Value.Item.Price * item.Value.Count);
        }
    }
}
