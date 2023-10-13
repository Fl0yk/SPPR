using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;
using WEB_153501_Kosach.Controllers;
using WEB_153501_Kosach.Domain.Entities;
using WEB_153501_Kosach.Domain.Models;
using WEB_153501_Kosach.Services.FurnitureCategoryService;
using WEB_153501_Kosach.Services.FurnitureServices;

namespace WEB_153501_Kosach.Tests.Controllers
{
    public class FurnitureControllerTests
    {
        private readonly Mock<IFurnitureService> _mockFurnitureService;
        private readonly Mock<IFurnitureCategoryService> _mockCategoryService;
        private readonly FurnitureController _controller;

        public FurnitureControllerTests()
        {
            _mockFurnitureService = new();
            _mockCategoryService = new();
            _controller = new(_mockFurnitureService.Object,
                                _mockCategoryService.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                }
            };
        }

        [Fact]
        public void Index_InvalidCategoryList_ReturnsNotFound()
        {
            _mockCategoryService.Setup(service => service.GetCategoryListAsync().Result)
                .Returns(new Domain.Models.ResponseData<List<Domain.Entities.FurnitureCategory>>() { Success = false });

            var result = _controller.Index(null, null).Result;

            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.NotFound, ((NotFoundObjectResult)result).StatusCode);
        }

        [Fact]
        public void Index_InvalidFurnitureList_ReturnsNotFound()
        {
            _mockCategoryService.Setup(service => service.GetCategoryListAsync().Result)
                .Returns(new Domain.Models.ResponseData<List<Domain.Entities.FurnitureCategory>>()
                {
                    Success = true,
                    Data = new List<Domain.Entities.FurnitureCategory>()
                    {
                        new Domain.Entities.FurnitureCategory() { Id = 1, Name = "Test 1", NormalizedName = "test1"},
                        new Domain.Entities.FurnitureCategory() { Id = 2, Name = "Test 2", NormalizedName = "test2"},
                    }
                });

            _mockFurnitureService.Setup(service => service.GetFurnitureListAsync(null, 1).Result)
                .Returns(new Domain.Models.ResponseData<Domain.Models.ListModel<Domain.Entities.Furniture>>() { Success = false});

            var result = _controller.Index(1, null).Result;

            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.NotFound, ((NotFoundObjectResult)result).StatusCode);
        }

        [Fact]
        public void Index_CorrectFurnituriesAndCategories_ViewDataContainsCategories()
        {
            _mockCategoryService.Setup(service => service.GetCategoryListAsync().Result)
                .Returns(new Domain.Models.ResponseData<List<Domain.Entities.FurnitureCategory>>()
                {
                    Success = true,
                    Data = new List<Domain.Entities.FurnitureCategory>()
                    {
                        new Domain.Entities.FurnitureCategory() { Id = 1, Name = "Test 1", NormalizedName = "test1"},
                        new Domain.Entities.FurnitureCategory() { Id = 2, Name = "Test 2", NormalizedName = "test2"},
                    }
                });

            _mockFurnitureService.Setup(service => service.GetFurnitureListAsync(null, 1).Result)
                .Returns(new Domain.Models.ResponseData<Domain.Models.ListModel<Domain.Entities.Furniture>>() 
                {
                    Success = true,
                    Data = new ListModel<Furniture>()
                    {
                        CurrentPage = 1,
                        TotalPages = 1,
                        Items = new List<Furniture>()
                        {
                            new Furniture() { Id = 1, CategoryId = 1, Name = "Furniture 1", Price = 1},
                            new Furniture() { Id = 2, CategoryId = 1, Name = "Furniture 2", Price = 1},
                            new Furniture() { Id = 3, CategoryId = 1, Name = "Furniture 3", Price = 1},
                        }

                    }

                });



            var result = _controller.Index(1, null).Result;

            Assert.NotNull(result);

            var viewResult = Assert.IsType<ViewResult>(result);
            Type targetType = typeof(List<FurnitureCategory>);
            Assert.Contains(viewResult.ViewData.Values, val => targetType.IsInstanceOfType(val));
  
        }

        [Fact]
        public void Index_CorrectFurnituriesAndCategories_ViewDataContainsCurrectCategory()
        {
            _mockCategoryService.Setup(service => service.GetCategoryListAsync().Result)
                .Returns(new Domain.Models.ResponseData<List<Domain.Entities.FurnitureCategory>>()
                {
                    Success = true,
                    Data = new List<Domain.Entities.FurnitureCategory>()
                    {
                        new Domain.Entities.FurnitureCategory() { Id = 1, Name = "Test 1", NormalizedName = "test1"},
                        new Domain.Entities.FurnitureCategory() { Id = 2, Name = "Test 2", NormalizedName = "test2"},
                    }
                });

            _mockFurnitureService.Setup(service => service.GetFurnitureListAsync(null, 1).Result)
                .Returns(new Domain.Models.ResponseData<Domain.Models.ListModel<Domain.Entities.Furniture>>()
                {
                    Success = true,
                    Data = new ListModel<Furniture>()
                    {
                        CurrentPage = 1,
                        TotalPages = 1,
                        Items = new List<Furniture>()
                        {
                            new Furniture() { Id = 1, CategoryId = 1, Name = "Furniture 1", Price = 1},
                            new Furniture() { Id = 2, CategoryId = 1, Name = "Furniture 2", Price = 1},
                            new Furniture() { Id = 3, CategoryId = 1, Name = "Furniture 3", Price = 1},
                        }

                    }

                });



            var result = _controller.Index(1, null).Result;

            Assert.NotNull(result);

            var viewResult = Assert.IsType<ViewResult>(result);
            Type targetType = typeof(List<FurnitureCategory>);
            Assert.Contains(viewResult.ViewData.Values, 
                        val => (val is string str) && (str == "Все"));

        }

        [Fact]
        public void Index_CorrectFurnituriesAndCategories_ModelIsFurnitureModelList()
        {
            _mockCategoryService.Setup(service => service.GetCategoryListAsync().Result)
                .Returns(new Domain.Models.ResponseData<List<Domain.Entities.FurnitureCategory>>()
                {
                    Success = true,
                    Data = new List<Domain.Entities.FurnitureCategory>()
                    {
                        new Domain.Entities.FurnitureCategory() { Id = 1, Name = "Test 1", NormalizedName = "test1"},
                        new Domain.Entities.FurnitureCategory() { Id = 2, Name = "Test 2", NormalizedName = "test2"},
                    }
                });

            _mockFurnitureService.Setup(service => service.GetFurnitureListAsync(null, 1).Result)
                .Returns(new Domain.Models.ResponseData<Domain.Models.ListModel<Domain.Entities.Furniture>>()
                {
                    Success = true,
                    Data = new ListModel<Furniture>()
                    {
                        CurrentPage = 1,
                        TotalPages = 1,
                        Items = new List<Furniture>()
                        {
                            new Furniture { Id = 1, CategoryId = 1, Name = "Furniture 1", Price = 1},
                            new Furniture { Id = 2, CategoryId = 1, Name = "Furniture 2", Price = 1},
                            new Furniture { Id = 3, CategoryId = 1, Name = "Furniture 3", Price = 1},
                        }

                    }

                });

            var result = _controller.Index(1, null).Result;

            Assert.NotNull(result);

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<ListModel<Furniture>>(viewResult.Model);
        }
    }
}
