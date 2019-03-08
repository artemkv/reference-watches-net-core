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
        public async Task GetWatch_ReturnsWatch()
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
            var result = Assert.IsType<ActionResult<WatchDto>>(response);
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

        [Fact]
        public async Task CreateWatch_CreatesWatchAndReturnsNewWatch()
        {
            // Arrange
            var watchToPost = GetWatchToPost();
            var watchPosted = GetWatchPosted();
            var mockWatchService = new Mock<IWatchService>();
            mockWatchService.Setup(svc => svc.CreateWatchAsync(
                It.Is<Watch>(
                    x => x.Id == 0 && 
                    x.Model.Equals(watchToPost.Model) &&
                    x.Title.Equals(watchToPost.Title) &&
                    x.Gender.Equals(watchToPost.Gender) &&
                    x.CaseSize.Equals(watchToPost.CaseSize) &&
                    x.CaseMaterial.Equals(watchToPost.CaseMaterial) &&
                    x.BrandId.Equals(watchToPost.BrandId) &&
                    x.MovementId.Equals(watchToPost.MovementId)))).ReturnsAsync(watchPosted);
            var mockApiConfiguration = new Mock<IApiConfiguration>();
            var controller = new WatchController(mockWatchService.Object, mockApiConfiguration.Object);

            // Act
            var response = await controller.CreateWatchAsync(watchToPost);

            // Assert
            var result = Assert.IsType<ActionResult<WatchDto>>(response);
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(response.Result);
            var model = Assert.IsAssignableFrom<WatchDto>(createdAtActionResult.Value);
            Assert.Equal(watchPosted.Id, model.Id);
            Assert.Equal(watchPosted.Model, model.Model);
            Assert.Equal(watchPosted.Title, model.Title);
        }

        [Fact]
        public async Task UpdateWatch_UpdatesWatch()
        {
            // Arrange
            var watchToPut = GetWatchToPut();
            var mockWatchService = new Mock<IWatchService>();
            mockWatchService.Setup(svc => svc.UpdateWatchAsync(
                watchToPut.Id, watchToPut.Model, watchToPut.Title, watchToPut.Gender, watchToPut.CaseSize,
                watchToPut.CaseMaterial, watchToPut.BrandId, watchToPut.MovementId)).ReturnsAsync(true);
            var mockApiConfiguration = new Mock<IApiConfiguration>();
            var controller = new WatchController(mockWatchService.Object, mockApiConfiguration.Object);

            // Act
            var response = await controller.UpdateWatchAsync(watchToPut.Id, watchToPut);

            // Assert
            Assert.IsType<NoContentResult>(response);
        }

        [Fact]
        public async Task UpdateWatch_ReturnsNotFound_ForInvalidId()
        {
            // Arrange
            var watchToPut = GetWatchToPut();
            var mockWatchService = new Mock<IWatchService>();
            mockWatchService.Setup(svc => svc.UpdateWatchAsync(
                watchToPut.Id, watchToPut.Model, watchToPut.Title, watchToPut.Gender, watchToPut.CaseSize,
                watchToPut.CaseMaterial, watchToPut.BrandId, watchToPut.MovementId))
                .ReturnsAsync(false).Verifiable();
            var mockApiConfiguration = new Mock<IApiConfiguration>();
            var controller = new WatchController(mockWatchService.Object, mockApiConfiguration.Object);

            // Act
            var response = await controller.UpdateWatchAsync(watchToPut.Id, watchToPut);

            // Assert
            mockWatchService.Verify();
            var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(response);
            Assert.Equal("Watch with id 15 cannot be found.", notFoundObjectResult.Value);
        }

        [Fact]
        public async Task UpdateWatch_ThrowsBadRequest_ForMismatchingIds()
        {
            // Arrange
            var watchToPut = GetWatchToPut();
            var mockWatchService = new Mock<IWatchService>();
            var mockApiConfiguration = new Mock<IApiConfiguration>();
            var controller = new WatchController(mockWatchService.Object, mockApiConfiguration.Object);

            // Act
            var ex = await Assert.ThrowsAsync<BadRequestException>(
                () => controller.UpdateWatchAsync(10, watchToPut));

            // Assert
            Assert.Equal("id", ex.Key);
            Assert.Equal("Watch id 15 does not match the id in the route: 10.", ex.Message);
        }

        [Fact]
        public async Task DeleteWatch_DeletesWatch()
        {
            // Arrange
            var mockWatchService = new Mock<IWatchService>();
            mockWatchService.Setup(svc => svc.DeleteWatchAsync(15)).ReturnsAsync(true);
            var mockApiConfiguration = new Mock<IApiConfiguration>();
            var controller = new WatchController(mockWatchService.Object, mockApiConfiguration.Object);

            // Act
            var response = await controller.DeleteWatchAsync(15);

            // Assert
            Assert.IsType<NoContentResult>(response);
        }

        [Fact]
        public async Task DeleteWatch_ReturnsNotFound_ForInvalidId()
        {
            // Arrange
            var mockWatchService = new Mock<IWatchService>();
            mockWatchService.Setup(svc => svc.DeleteWatchAsync(15)).ReturnsAsync(false).Verifiable();
            var mockApiConfiguration = new Mock<IApiConfiguration>();
            var controller = new WatchController(mockWatchService.Object, mockApiConfiguration.Object);

            // Act
            var response = await controller.DeleteWatchAsync(15);

            // Assert
            mockWatchService.Verify();
            var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(response);
            Assert.Equal("Watch with id 15 cannot be found.", notFoundObjectResult.Value);
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

        private WatchToPostDto GetWatchToPost()
        {
            return new WatchToPostDto
            {
                Model = "15450BA.OO.1256BA.01",
                Title = "Royal Oak",
                Gender = Gender.Ladies,
                CaseSize = 37,
                CaseMaterial = CaseMaterial.Gold,
                BrandId = 8,
                MovementId = 2
            };
        }

        private Watch GetWatchPosted()
        {
            return new Watch
            {
                Id = 15,
                Model = "15450BA.OO.1256BA.01",
                Title = "Royal Oak",
                Gender = Gender.Ladies,
                CaseSize = 37,
                CaseMaterial = CaseMaterial.Gold,
                DateCreated = DateTime.UtcNow,
                BrandId = 8,
                Brand = new Brand
                {
                    Id = 8,
                    Title = "Audemars Piguet",
                    YearFounded = 1875,
                    Description = "Swiss manufacturer of luxury mechanical watches and clocks",
                    DateCreated = DateTime.UtcNow
                },
                MovementId = 2,
                Movement = new Movement
                {
                    Id = 2,
                    Title = "Automatic"
                }
            };
        }

        private WatchToPutDto GetWatchToPut()
        {
            return new WatchToPutDto
            {
                Id = 15,
                Model = "15450BA.OO.1256BA.01",
                Title = "Royal Oak",
                Gender = Gender.Ladies,
                CaseSize = 37,
                CaseMaterial = CaseMaterial.Gold,
                BrandId = 8,
                MovementId = 2
            };
        }
    }
}
