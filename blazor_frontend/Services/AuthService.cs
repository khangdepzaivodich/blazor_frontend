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
        Task InitializeAsync();
    }

    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly IJSRuntime _jsRuntime;
        private readonly AuthState _authState;
        private Task? _initializeTask;

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
                var rawJson = await response.Content.ReadAsStringAsync();
                var loginResponse = System.Text.Json.JsonSerializer.Deserialize<LoginResponse>(rawJson);
                if (loginResponse != null)
                {
                    _authState.SetUser(
                        loginResponse.Token,
                        loginResponse.UserId,
                        loginResponse.Email,
                        loginResponse.Role,
                        loginResponse.HoTen,
                        loginResponse.Avatar ?? ""
                    );

                    await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "auth_token", loginResponse.Token);
                    await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "auth_user_id", loginResponse.UserId.ToString());
                    await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "auth_email", loginResponse.Email);
                    await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "auth_role", loginResponse.Role);
                    await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "auth_ho_ten", loginResponse.HoTen);
                    
                    // Always set auth_avatar, even if empty, to overwrite old data
                    await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "auth_avatar", loginResponse.Avatar ?? "");
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
            _initializeTask = null; // Allow re-initialization for next user
            
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "auth_token");
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "auth_user_id");
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "auth_email");
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "auth_ho_ten");
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "auth_role");
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "auth_avatar");
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "user_account");
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "user_avatar");

            // Clear Chat sessions
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "chat_session_id");
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "chat_guest_id");

            // Clear pending discount
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "pending_discount_code");
        }

        public Task InitializeAsync()
        {
            if (_initializeTask != null) return _initializeTask;

            _initializeTask = DoInitializeAsync();
            return _initializeTask;
        }

        private async Task DoInitializeAsync()
        {
            try
            {
                var token = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", "auth_token");
                if (string.IsNullOrEmpty(token)) return;

                var userIdStr = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", "auth_user_id");
                var email = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", "auth_email");
                var role = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", "auth_role");
                var hoTen = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", "auth_ho_ten");
                var avatar = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", "auth_avatar");

                if (Guid.TryParse(userIdStr, out var userId))
                {
                    _authState.SetUser(token, userId, email ?? "", role ?? "", hoTen ?? "", avatar ?? "");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DEBUG] InitializeAsync ERROR: {ex.Message}");
            }
        }
    }
}