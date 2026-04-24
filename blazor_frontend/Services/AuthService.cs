using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using blazor_frontend.Models.BackendDTOs;

namespace blazor_frontend.Services
{
    public interface IAuthService
    {
        Task<LoginResponse?> LoginAsync(LoginRequest request);
        Task<RegisterResponse?> RegisterAsync(RegisterRequest request);
        Task<bool> ForgotPasswordAsync(ForgotPasswordRequest request);
        Task<bool> ResetPasswordAsync(ResetPasswordRequest request);
        Task LogoutAsync();
    }

    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly IJSRuntime _jsRuntime;
        private readonly AuthState _authState;

        public AuthService(IHttpClientFactory httpClientFactory, IJSRuntime jsRuntime, AuthState authState)
        {
            _httpClient = httpClientFactory.CreateClient("IdentityAPI");
            _jsRuntime = jsRuntime;
            _authState = authState;
        }

        public async Task<LoginResponse?> LoginAsync(LoginRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", request);
            if (response.IsSuccessStatusCode)
            {
                var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();
                if (loginResponse != null)
                {
                    _authState.SetUser(
                        loginResponse.Token,
                        loginResponse.UserId,
                        loginResponse.Email,
                        loginResponse.Role,
                        loginResponse.HoTen
                    );
                }
                return loginResponse;
            }
            return null;
        }

        public async Task<RegisterResponse?> RegisterAsync(RegisterRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/register", request);
            var body = await response.Content.ReadAsStringAsync();

            Console.WriteLine($"STATUS: {response.StatusCode}");
            Console.WriteLine($"BODY: {body}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<RegisterResponse>();
            }
            return null;
        }

        public async Task<bool> ForgotPasswordAsync(ForgotPasswordRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/forgot-password", request);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ResetPasswordAsync(ResetPasswordRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/reset-password", request);
            return response.IsSuccessStatusCode;
        }

        public async Task LogoutAsync()
        {
            _authState.Clear();
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "auth_token");
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "auth_user_id");
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "auth_ho_ten");
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "auth_role");
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "user_account");
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "user_avatar");
        }
    }
}