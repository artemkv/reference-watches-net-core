using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Watches.Models;
using Xunit;

namespace Watches.IntegrationTests
{
    public class WatchControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        protected readonly HttpClient _client;

        public WatchControllerIntegrationTests(CustomWebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetWatch_ReturnsWatch()
        {
            // Arrange

            // Act
            var response = await _client.GetAsync($"api/watches/{WatchesDbContextSeeder.HAMILTON_KHAKI_FIELD}");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var watch = JsonConvert.DeserializeObject<WatchDto>(json);

            // Assert
            Assert.Equal(WatchesDbContextSeeder.HAMILTON_KHAKI_FIELD, watch.Id);
        }

        [Fact]
        public async Task GetWatch_Returns404WhenNotFound()
        {
            // Arrange

            // Act
            var response = await _client.GetAsync($"api/watches/150");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
