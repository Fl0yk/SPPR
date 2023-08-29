using Microsoft.EntityFrameworkCore;
using WEB_153501_Kosach.Domain.Entities;

namespace WEB_153501_Kosach.API.Data
{
    public static class DbInitializercs
    {
        public static async Task SeedData(WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            // Выполнение миграций
            await context.Database.MigrateAsync();

            string pathImage = app.Configuration["ApiUrl"] ?? "";
            FurnitureCategory[] categories = new FurnitureCategory[]
            {
                new FurnitureCategory {Name="Стулья",
                NormalizedName="chairs"},
                    new FurnitureCategory {Name="Кровати",
                NormalizedName="beds"},
                    new FurnitureCategory {Name="Столы",
                NormalizedName="tables"},
                    new FurnitureCategory {Name="Диваны",
                NormalizedName="sofas"}
            };
            context.FurnitureCategories.AddRange(categories);


            await context.AddRangeAsync(new Furniture[]
            {
                //Диваны
                new Furniture() { Name = "Stoline Олимп",
                                  Image = pathImage + "/Images/sofas_1.jpeg",
                                  Price = 780, CategoryId = categories.First(c => c.NormalizedName.Equals("sofas")) },
                new Furniture() { Name = "Mio Tesoro",
                                  Image = pathImage + "/Images/sofas_2.jpeg",
                                  Price = 1150, CategoryId = categories.First(c => c.NormalizedName.Equals("sofas")) },
                new Furniture() { Name = "WoodCraft Фишер",
                                  Image = pathImage + "/Images/sofas_3.jpeg",
                                  Price = 1090, CategoryId = categories.First(c => c.NormalizedName.Equals("sofas")) },
                new Furniture() { Name = "KRONES Клио",
                                  Image = pathImage + "/Images/sofas_4.jpeg",
                                  Price = 595.24m, CategoryId = categories.First(c => c.NormalizedName.Equals("sofas")) },
                //Стулья
                new Furniture() { Name = "M-City Desert 603",
                                  Image = pathImage + "/Images/chairs_5.jpeg",
                                  Price = 92.92m, CategoryId = categories.First(c => c.NormalizedName.Equals("chairs")) },
                new Furniture() { Name = "Mio Tesoro Donato SC-264F",
                                  Image = pathImage + "/Images/chairs_6.jpeg",
                                  Price = 247.85m, CategoryId = categories.First(c => c.NormalizedName.Equals("chairs")) },
                new Furniture() { Name = "Sheffilton SHT-ST19/S29",
                                  Image = pathImage + "/Images/chairs_7.jpeg",
                                  Price = 116.39m, CategoryId = categories.First(c => c.NormalizedName.Equals("chairs")) },
                new Furniture() { Name = "Ника ССН1/1",
                                  Image = pathImage + "/Images/chairs_8.jpeg",
                                  Price = 53.22m, CategoryId = categories.First(c => c.NormalizedName.Equals("chairs")) },
                //Столы
                new Furniture() { Name = "ГМЦ Paprika 100x60",
                                  Image = pathImage + "/Images/tables_9.jpeg",
                                  Price = 119.47m, CategoryId = categories.First(c => c.NormalizedName.Equals("tables")) },
                new Furniture() { Name = "Eligard Black / СОБ",
                                  Image = pathImage + "/Images/tables_10.jpeg",
                                  Price = 362.69m, CategoryId = categories.First(c => c.NormalizedName.Equals("tables")) },
                new Furniture() { Name = "Артём-Мебель СН-005.011",
                                  Image = pathImage + "/Images/tables_11.jpeg",
                                  Price = 83.10m, CategoryId = categories.First(c => c.NormalizedName.Equals("tables")) },
                new Furniture() { Name = "Mio Tesoro ST-011",
                                  Image = pathImage + "/Images/tables_12.jpeg",
                                  Price = 119.47m, CategoryId = categories.First(c => c.NormalizedName.Equals("tables")) },
                //Кровати
                new Furniture() { Name = "Горизонт Мебель Юнона 1.6м",
                                  Image = pathImage + "/Images/beds_13.jpeg",
                                  Price = 179.00m, CategoryId = categories.First(c => c.NormalizedName.Equals("beds")) },
                new Furniture() { Name = "Гармония КР-601 160x200",
                                  Image = pathImage + "/Images/beds_14.jpeg",
                                  Price = 239.78m, CategoryId = categories.First(c => c.NormalizedName.Equals("beds")) },
                new Furniture() { Name = "Домаклево Канапе 160x200",
                                  Image = pathImage + "/Images/beds_15.jpeg",
                                  Price = 229.00m, CategoryId = categories.First(c => c.NormalizedName.Equals("beds")) },
                new Furniture() { Name = "Домаклево Лофт 160x200",
                                  Image = pathImage + "/Images/beds_16.jpeg",
                                  Price = 349.00m, CategoryId = categories.First(c => c.NormalizedName.Equals("beds")) }
            });

            await context.SaveChangesAsync();
        }
    }
}
