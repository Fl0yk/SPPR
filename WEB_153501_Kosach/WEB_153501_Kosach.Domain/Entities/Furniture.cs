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
        public string Description { get; set; } = null!;
        public Category? CategoryId { get; set; }

        public string? Image { get; set; }
    }
}
