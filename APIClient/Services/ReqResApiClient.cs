using RaftLabs.APIClient.Interfaces;
using RaftLabs.APIClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RaftLabs.APIClient.Services
{
    public class ReqResApiClient : IReqResApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUri;
        private readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };
        public ReqResApiClient(HttpClient httpClient, string baseUri)
        {
            _httpClient = httpClient;
            _baseUri = baseUri.TrimEnd('/');
        }
        public async Task<UserData> GetUserByIdAsync(int userId)
        {
            try
            {
                var response = await _httpClient.GetAsync("api/users/"+userId);
                if (response.StatusCode == HttpStatusCode.NotFound)
                    return null;

                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<UserWrapper>(content, _jsonOptions);

                return result.Data;
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Failed to fetch user with ID {userId}: {ex.Message}", ex);
            }
            catch (JsonException ex)
            {
                throw new Exception("Failed to parse user data", ex);
            }
        }
        public async Task<UserResponse> GetUsersByPageAsync(int pageNumber)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/users?page={pageNumber}");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<UserResponse>(content, _jsonOptions);
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Failed to fetch users on page {pageNumber}: {ex.Message}", ex);
            }
            catch (JsonException ex)
            {
                throw new Exception("Failed to parse users data", ex);
            }

        }

    }
}
