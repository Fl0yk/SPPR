using Microsoft.EntityFrameworkCore;
using WEB_153501_Kosach.Domain.Entities;

namespace WEB_153501_Kosach.API.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Furniture> Furnitures => Set<Furniture>();
        public DbSet<FurnitureCategory> FurnitureCategories => Set<FurnitureCategory>();

        public AppDbContext(DbContextOptions<AppDbContext> options) :
            base(options)
        {
            //Database.EnsureDeleted();
            //Database.Migrate();
            Database.EnsureCreated();
            
        }
    }
}
