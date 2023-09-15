using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEB_153501_Kosach.Domain.Entities
{
    public class Furniture
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }

        public int CategoryId { get; set; }

        public FurnitureCategory? Category { get; set; }

        public string? Image { get; set; }
    }
}
