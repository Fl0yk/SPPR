using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEB_153501_Kosach.Domain.Entities;

namespace WEB_153501_Kosach.Domain.Models
{
    public class CartItem
    {
        public Furniture Item { get; set; } = null!;

        public int Count { get; set; }
    }
}
