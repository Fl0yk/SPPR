using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEB_153501_Kosach.Domain.Entities
{
    public class FurnitureCategory
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string NormalizedName { get; set; } = null!;
    }
}
