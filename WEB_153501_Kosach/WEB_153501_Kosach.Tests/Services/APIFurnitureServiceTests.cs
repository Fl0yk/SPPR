using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using WEB_153501_Kosach.API.Data;
using WEB_153501_Kosach.API.Services;
using WEB_153501_Kosach.Domain.Entities;
using WEB_153501_Kosach.Domain.Models;

namespace WEB_153501_Kosach.Tests.Services
{
    public class SqliteInMemoryAPIFurnitureServiceTests : IDisposable
    {
        private readonly DbConnection _connection;
        private readonly DbContextOptions<AppDbContext> _options;

        public SqliteInMemoryAPIFurnitureServiceTests()
        {
            _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();

            _options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(_connection)
                .Options;

            using var context = new AppDbContext(_options);
            context.Database.EnsureCreated();

            context.FurnitureCategories.AddRange(new FurnitureCategory[]
            {
                new FurnitureCategory {Name="Стулья",
                NormalizedName="chairs"},
                    new FurnitureCategory {Name="Кровати",
                NormalizedName="beds"},
            });

            context.Furnitures.AddRange( new Furniture[] {
                //Диваны
                new Furniture()
                {
                    Name = "Stoline Олимп",
                    Image = null,
                    Price = 780,
                    CategoryId = 1,
                },
                new Furniture()
                {
                    Name = "Mio Tesoro",
                    Image = null,
                    Price = 1150,
                    CategoryId = 1,
                },
                new Furniture()
                {
                    Name = "WoodCraft Фишер",
                    Image = null,
                    Price = 1090,
                    CategoryId = 1,
                },
                new Furniture()
                {
                    Name = "KRONES Клио",
                    Image = null,
                    Price = 595.24m,
                    CategoryId = 1,
                },
                //Стулья
                new Furniture()
                {
                    Name = "M-City Desert 603",
                    Image = null,
                    Price = 92.92m,
                    CategoryId = 2,
                },
                new Furniture()
                {
                    Name = "Mio Tesoro Donato SC-264F",
                    Image = null,
                    Price = 247.85m,
                    CategoryId = 2,
                },
                new Furniture()
                {
                    Name = "Sheffilton SHT-ST19/S29",
                    Image = null,
                    Price = 116.39m,
                    CategoryId = 2,
                },
                new Furniture()
                {
                    Name = "Ника ССН1/1",
                    Image = null,
                    Price = 53.22m,
                    CategoryId = 2
                } });

            context.SaveChanges();
        }

        private AppDbContext CreateContext() => new(_options);

        public void Dispose() => _connection.Dispose();

        [Fact]
        public void GetProductList_ReturnsFirstPageOf3Items()
        {
            using var context = CreateContext();
            var service = new FurnitureService(context, null, null);
            var result = service.GetProductListAsync(null).Result;
            Assert.IsType<ResponseData<ListModel<Furniture>>>(result);
            Assert.True(result.Success);
            Assert.Equal(1, result.Data.CurrentPage);
            Assert.Equal(3, result.Data.Items.Count);
            Assert.Equal(3, result.Data.TotalPages);
            Assert.Equal(context.Furnitures.First(), result.Data.Items[0]);
        }

        [Fact]
        public void GetProductList_ReturnsCorrectPage()
        {
            using var context = CreateContext();
            var service = new FurnitureService(context, null, null);
            var result = service.GetProductListAsync(null, 2).Result;

            Assert.IsType<ResponseData<ListModel<Furniture>>>(result);
            Assert.True(result.Success);
            Assert.Equal(2, result.Data.CurrentPage);
        }

        [Fact]
        public void GetProductList_ReturnsCorrectCategoryFurnituries()
        {
            using var context = CreateContext();
            var service = new FurnitureService(context, null, null);
            var result = service.GetProductListAsync("chairs").Result;

            Assert.IsType<ResponseData<ListModel<Furniture>>>(result);
            Assert.True(result.Success);
            Assert.Equal(2, result.Data.TotalPages);
            Assert.True(result.Data.Items.All(f => f.Category.NormalizedName.Equals("chairs")));
        }

        [Fact]
        public void GetProductList_NotEditMaxPageSize()
        {
            using var context = CreateContext();
            var service = new FurnitureService(context, null, null);
            var result = service.GetProductListAsync(null, 1, 25).Result;
            Assert.IsType<ResponseData<ListModel<Furniture>>>(result);
            Assert.True(result.Success);
            Assert.Equal(context.Furnitures.Count(), result.Data.Items.Count);
        }

        [Fact]
        public void GetProductList_ReturnsFalseSuccess()
        {
            using var context = CreateContext();
            var service = new FurnitureService(context, null, null);
            var result = service.GetProductListAsync(null, 10).Result;
            Assert.IsType<ResponseData<ListModel<Furniture>>>(result);
            Assert.False(result.Success);
        }
    }
}
