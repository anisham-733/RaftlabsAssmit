using RaftLabs.APIClient.Interfaces;
using RaftLabs.APIClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using  RaftLabs.APIClient.Models;
using RaftLabs.APIClient.Services;

namespace RaftLabs.APIClient.Services
{
    public class ExternalUserService : IExternalUserService
    {
        private readonly IReqResApiClient _reqResApiClient;
        private readonly IMemoryCache _cache;

        public ExternalUserService(IReqResApiClient reqResApiClient, IMemoryCache cache)
        {
            _reqResApiClient = reqResApiClient;
            _cache = cache;            
        }
        public async Task<IEnumerable<UserData>> GetAllUsersAsync()
        {
            string cacheKey = "all_users";

            if (_cache.TryGetValue(cacheKey, out IEnumerable<UserData> cachedUsers))
            {
                return cachedUsers;
            }

            var result = new List<UserData>();
            int page = 1;
            UserResponse response;

            do
            {
                response = await _reqResApiClient.GetUsersByPageAsync(page++);
                foreach (var dto in response.Data)
                {
                    result.Add(dto.ToDomain());
                }
            } while (page <= response.Total_Pages);

            _cache.Set(cacheKey, result, TimeSpan.FromSeconds(6000));
            return result;
        }


        public async Task<UserData> GetUserByIdAsync(int id)
        {
            string cacheKey = $"user_{id}";

            if (_cache.TryGetValue(cacheKey, out UserData cachedUser))
            {
                return cachedUser;
            }

            var userData = await _reqResApiClient.GetUserByIdAsync(id);
            if (userData != null)
            {
                var user = userData.ToDomain();
                _cache.Set(cacheKey, user, TimeSpan.FromSeconds(6000));
                return user;
            }

            return null;
        }

    }
}
