using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Watches.Controllers;
using Watches.Exceptions;
using Watches.Models;
using Watches.Services;
using Watches.ViewModels;
using Xunit;

namespace Watches.Tests
{
    public class WatchControllerTests
    {
        [Fact]
        public async Task GetWatch_CallsWatchServiceWithId()
        {
            // Arrange
            var today = DateTime.UtcNow;
            var expected = GetSingleWatch(today);

            var mockWatchService = new Mock<IWatchService>();
            mockWatchService.Setup(svc => svc.GetWatchAsync(5)).ReturnsAsync(expected);
            var mockApiConfiguration = new Mock<IApiConfiguration>();
            var controller = new WatchController(mockWatchService.Object, mockApiConfiguration.Object);

            // Act
            var response = await controller.GetWatchAsync(5);

            // Assert
            var actionResult = Assert.IsType<ActionResult<WatchDto>>(response);
            var model = Assert.IsAssignableFrom<WatchDto>(actionResult.Value);
            Assert.Equal(expected.Id, model.Id);
            Assert.Equal(expected.Model, model.Model);
            Assert.Equal(expected.Title, model.Title);
            Assert.Equal(expected.Gender, model.Gender);
            Assert.Equal(expected.CaseSize, model.CaseSize);
            Assert.Equal(expected.CaseMaterial, model.CaseMaterial);
            Assert.Equal(expected.DateCreated, model.DateCreated);
            Assert.Equal(expected.Brand.Id, model.Brand.Id);
            Assert.Equal(expected.Brand.Title, model.Brand.Title);
            Assert.Equal(expected.Brand.YearFounded, model.Brand.YearFounded);
            Assert.Equal(expected.Brand.Description, model.Brand.Description);
            Assert.Equal(expected.Brand.DateCreated, model.Brand.DateCreated);
            Assert.Equal(expected.MovementId, model.MovementId);
        }

        [Fact]
        public async Task GetWatch_ReturnsNotFound_ForInvalidId()
        {
            // Arrange
            var mockWatchService = new Mock<IWatchService>();
            mockWatchService.Setup(svc => svc.GetWatchAsync(55)).ReturnsAsync((Watch)null);
            var mockApiConfiguration = new Mock<IApiConfiguration>();
            var controller = new WatchController(mockWatchService.Object, mockApiConfiguration.Object);

            // Act
            var response = await controller.GetWatchAsync(55);

            // Assert
            var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(response.Result);
            Assert.Equal("Watch with id 55 cannot be found.", notFoundObjectResult.Value);
        }

        [Fact]
        public async Task GetWatches_CallsWatchServiceWithDefaultValues()
        {
            // Arrange
            var expected = GetWatches();

            var mockWatchService = new Mock<IWatchService>();
            mockWatchService.Setup(svc => svc.GetWatchesAsync("", null, null, 0, 20)).ReturnsAsync(expected);
            var mockApiConfiguration = new Mock<IApiConfiguration>();
            mockApiConfiguration.Setup(config => config.ApiPageSizeLimit).Returns(100);
            mockApiConfiguration.Setup(config => config.ApiDefaultPageSize).Returns(20);
            var controller = new WatchController(mockWatchService.Object, mockApiConfiguration.Object);

            // Act
            var response = await controller.GetWatchesAsync();

            // Assert
            var actionResult = Assert.IsType<ActionResult<GetListResponse<WatchDto>>>(response);
            var listResponse = Assert.IsAssignableFrom<GetListResponse<WatchDto>>(actionResult.Value);
            Assert.Equal(expected.Total, listResponse.Total);
            Assert.Equal(expected.Count, listResponse.Count);
            Assert.Equal(expected.PageNumber, listResponse.PageNumber);
            Assert.Equal(expected.PageSize, listResponse.PageSize);
            Assert.Equal(expected.Results.Count, listResponse.Results.Count);
        }

        [Fact]
        public async Task GetWatches_CallsWatchServiceWithCorrectFilters()
        {
            // Arrange
            var expected = GetWatches();

            var mockWatchService = new Mock<IWatchService>();
            mockWatchService.Setup(svc => svc.GetWatchesAsync("t1", Gender.Mens, 123, 3, 80)).ReturnsAsync(expected);
            var mockApiConfiguration = new Mock<IApiConfiguration>();
            mockApiConfiguration.Setup(config => config.ApiPageSizeLimit).Returns(100);
            mockApiConfiguration.Setup(config => config.ApiDefaultPageSize).Returns(20);
            var controller = new WatchController(mockWatchService.Object, mockApiConfiguration.Object);

            // Act
            var response = await controller.GetWatchesAsync("t1", Gender.Mens, 123, 3, 80);

            // Assert
            var actionResult = Assert.IsType<ActionResult<GetListResponse<WatchDto>>>(response);
            var listResponse = Assert.IsAssignableFrom<GetListResponse<WatchDto>>(actionResult.Value);
            Assert.Equal(expected.Total, listResponse.Total);
            Assert.Equal(expected.Count, listResponse.Count);
            Assert.Equal(expected.PageNumber, listResponse.PageNumber);
            Assert.Equal(expected.PageSize, listResponse.PageSize);
            Assert.Equal(expected.Results.Count, listResponse.Results.Count);
        }

        [Fact]
        public async Task GetWatches_ThrowsBadRequest_ForInvalidPageNumber()
        {
            // Arrange
            var mockWatchService = new Mock<IWatchService>();
            var mockApiConfiguration = new Mock<IApiConfiguration>();
            mockApiConfiguration.Setup(config => config.ApiPageSizeLimit).Returns(10);
            var controller = new WatchController(mockWatchService.Object, mockApiConfiguration.Object);

            // Act
            var ex = await Assert.ThrowsAsync<BadRequestException>(() => controller.GetWatchesAsync(pageNumber: -1));

            // Assert
            Assert.Equal("pageNumber", ex.Key);
            Assert.Equal("Wrong value for page number: -1. Page number is expected to be greater than 0.", ex.Message);
        }

        [Fact]
        public async Task GetWatches_ThrowsBadRequest_ForInvalidPageSize()
        {
            // Arrange
            var mockWatchService = new Mock<IWatchService>();
            var mockApiConfiguration = new Mock<IApiConfiguration>();
            mockApiConfiguration.Setup(config => config.ApiPageSizeLimit).Returns(10);
            var controller = new WatchController(mockWatchService.Object, mockApiConfiguration.Object);

            // Act
            var ex = await Assert.ThrowsAsync<BadRequestException>(() => controller.GetWatchesAsync(pageSize: 11));

            // Assert
            Assert.Equal("pageSize", ex.Key);
            Assert.Equal("Wrong value for page size: 11. Page size is expected to be in 1-10 range.", ex.Message);
        }

        private Watch GetSingleWatch(DateTime date)
        {
            return new Watch
            {
                Id = 5,
                Model = "114060",
                Title = "Submariner",
                Gender = Gender.Mens,
                CaseSize = 40,
                CaseMaterial = CaseMaterial.StainlessSteel,
                DateCreated = date,
                BrandId = 4,
                Brand = new Brand
                {
                    Id = 4,
                    Title = "Rolex",
                    YearFounded = 1915,
                    Description = "Swiss luxury watch manufacturer",
                    DateCreated = date
                },
                MovementId = 2,
                Movement = new Movement
                {
                    Id = 2,
                    Title = "Automatic"
                }
            };
        }

        private ResultsPage<Watch> GetWatches()
        {
            return new ResultsPage<Watch>
            {
                Total = 2,
                Count = 2,
                PageNumber = 0,
                PageSize = 20,
                Results = new List<Watch>()
                {
                    new Watch
                    {
                        Id = 5
                    },
                    new Watch
                    {
                        Id = 6
                    }
                }
            };
        }
    }
}
