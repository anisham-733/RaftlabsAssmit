using Microsoft.Extensions.Caching.Memory;
using RaftLabs.APIClient.Services;

namespace ReqResApiClient.Tests
{
    public class ExternalUserServiceTests
    {
        private readonly ExternalUserService _service;
        private readonly MemoryCache _cache;
        public ExternalUserServiceTests()
        {
            var httpClient = new HttpClient { BaseAddress = new Uri("https://reqres.in/api") };
            var apiClient = new RaftLabs.APIClient.Services.ReqResApiClient(httpClient, "https://reqres.in/api");
            _cache = new MemoryCache(new MemoryCacheOptions());
            _service = new ExternalUserService(apiClient, _cache);
        }

        [Fact]
        public async Task GetUserByIdAsync_ValidUserId_ReturnsUser()
        {
            var httpClient = new HttpClient { BaseAddress = new Uri("https://reqres.in/api") };
            var apiClient = new RaftLabs.APIClient.Services.ReqResApiClient(httpClient, "https://reqres.in/api");
            var service = new ExternalUserService(apiClient, _cache);

            var user = await service.GetUserByIdAsync(2);

            Assert.NotNull(user);
            Assert.Equal(2, user.Id);
        }

        [Fact]
        public async Task GetAllUsersAsync_ReturnsMultipleUsers()
        {
            var httpClient = new HttpClient { BaseAddress = new Uri("https://reqres.in/api") };
            var apiClient = new RaftLabs.APIClient.Services.ReqResApiClient(httpClient, "https://reqres.in/api");
            var service = new ExternalUserService(apiClient, _cache);

            var users = await service.GetAllUsersAsync();

            Assert.NotEmpty(users);
        }
    }
}