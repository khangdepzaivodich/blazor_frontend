using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using blazor_frontend.Models.BackendDTOs;

namespace blazor_frontend.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;

        public UserService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("IdentityAPI");
        }

        public async Task<UserPaginatedResult?> GetAllUsersAsync(int page, int pageSize)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/user?page={page}&pageSize={pageSize}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<UserPaginatedResult>();
                }
                return null;
            }
            catch { return null; }
        }

        public async Task<UserDto?> GetUserByIdAsync(Guid id)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<UserDto>($"api/user/{id}");
            }
            catch { return null; }
        }

        public async Task<Guid?> CreateUserAsync(CreateUserRequest request)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/user", request);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<Guid>();
                }
                return null;
            }
            catch { return null; }
        }

        public async Task<bool> UpdateUserAsync(Guid id, UpdateUserByAdminRequest request)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/user/{id}", request);
                return response.IsSuccessStatusCode;
            }
            catch { return false; }
        }

        public async Task<bool> DeleteUserAsync(Guid id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/user/{id}");
                return response.IsSuccessStatusCode;
            }
            catch { return false; }
        }

        public async Task<bool> LockUserAsync(Guid id)
        {
            try
            {
                var response = await _httpClient.PutAsync($"api/user/{id}/lock", null);
                return response.IsSuccessStatusCode;
            }
            catch { return false; }
        }

        public async Task<bool> UnlockUserAsync(Guid id)
        {
            try
            {
                var response = await _httpClient.PutAsync($"api/user/{id}/unlock", null);
                return response.IsSuccessStatusCode;
            }
            catch { return false; }
        }
    }
}
