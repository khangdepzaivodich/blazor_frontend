using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using blazor_frontend.Models.BackendDTOs;

namespace blazor_frontend.Services
{
    public interface IAuthService
    {
        Task<LoginResponse?> LoginAsync(LoginRequest request);
        Task<RegisterResponse?> RegisterAsync(RegisterRequest request);
    }

    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;

        public AuthService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("IdentityAPI");
        }

        public async Task<LoginResponse?> LoginAsync(LoginRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", request);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<LoginResponse>();
            }
            return null;
        }

        public async Task<RegisterResponse?> RegisterAsync(RegisterRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/register", request);
            var body = await response.Content.ReadAsStringAsync();

            Console.WriteLine($"STATUS: {response.StatusCode}");
            Console.WriteLine($"BODY: {body}");
            if (response != null)
            {
                return await response.Content.ReadFromJsonAsync<RegisterResponse>();
            }
            return null;
        }
    }
}