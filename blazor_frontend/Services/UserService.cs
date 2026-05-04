using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using blazor_frontend.Models.BackendDTOs;

namespace blazor_frontend.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthState _authState;

        public UserService(IHttpClientFactory httpClientFactory, AuthState authState)
        {
            _httpClient = httpClientFactory.CreateClient("IdentityAPI");
            _authState = authState;
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
            var response = await _httpClient.PostAsJsonAsync("api/user", request);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Guid>();
            }
            
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"Server error ({response.StatusCode}): {error}");
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

        public async Task<UserDto?> GetMeAsync()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<UserDto>("api/user/me");
            }
            catch { return null; }
        }

        public async Task<bool> UpdateMeAsync(UpdateMeRequest request)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync("api/user/me", request);
                return response.IsSuccessStatusCode;
            }
            catch { return false; }
        }

        public async Task<string?> UploadAvatarAsync(Stream fileStream, string fileName)
        {
            if (fileStream.CanSeek) fileStream.Position = 0;

            using var content = new MultipartFormDataContent();
            using var streamContent = new StreamContent(fileStream);
            streamContent.Headers.ContentType = new MediaTypeHeaderValue("image/png");
            content.Add(streamContent, "file", fileName);

            // Tạo request thủ công để kiểm soát Header, tránh lỗi AuthHandler với Multipart trong WASM
            var request = new HttpRequestMessage(HttpMethod.Post, "api/user/me/avatar");
            request.Content = content;

            // Gắn token thủ công nếu đã đăng nhập
            if (_authState.IsAuthenticated)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _authState.Token);
            }

            var response = await _httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<UploadResponse>();
                return result?.Url;
            }
            
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"Server error ({response.StatusCode}): {error}");
        }

        private class UploadResponse
        {
            [System.Text.Json.Serialization.JsonPropertyName("url")]
            public string Url { get; set; } = string.Empty;
        }
    }
}
