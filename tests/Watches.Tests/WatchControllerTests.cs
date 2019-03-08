using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Threading.Tasks;
using Watches.Controllers;
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
            var mockWatchService = new Mock<IWatchService>();
            mockWatchService.Setup(svc => svc.GetWatchAsync(5)).ReturnsAsync(GetSingleWatch(today));
            var mockApiConfiguration = new Mock<IApiConfiguration>();
            var controller = new WatchController(mockWatchService.Object, mockApiConfiguration.Object);

            // Act
            var response = await controller.GetWatchAsync(5);

            // Assert
            var actionResult = Assert.IsType<ActionResult<WatchDto>>(response);
            var model = Assert.IsAssignableFrom<WatchDto>(actionResult.Value);
            Assert.Equal(5, model.Id);
            Assert.Equal("114060", model.Model);
            Assert.Equal("Submariner", model.Title);
            Assert.Equal(Gender.Mens, model.Gender);
            Assert.Equal(40, model.CaseSize);
            Assert.Equal(CaseMaterial.StainlessSteel, model.CaseMaterial);
            Assert.Equal(today, model.DateCreated);
            Assert.Equal(4, model.Brand.Id);
            Assert.Equal("Rolex", model.Brand.Title);
            Assert.Equal(1915, model.Brand.YearFounded);
            Assert.Equal("Swiss luxury watch manufacturer", model.Brand.Description);
            Assert.Equal(today, model.Brand.DateCreated);
            Assert.Equal(2, model.MovementId);
        }

        [Fact]
        public async Task GetWatch_ReturnsNotFount_ForInvalidId()
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
    }
}
