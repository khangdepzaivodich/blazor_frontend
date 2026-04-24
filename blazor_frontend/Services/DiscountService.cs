using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using blazor_frontend.Models.BackendDTOs;

namespace blazor_frontend.Services
{
    public interface IDiscountService
    {
        Task<IEnumerable<MaGiamGiaDto>> GetDiscountsAsync();
        Task<MaGiamGiaDto?> GetDiscountByCodeAsync(string code);
        Task<bool> ApplyDiscountAsync(string code);
        Task<MaGiamGiaDto?> CreateDiscountAsync(CreateMaGiamGiaRequest request);
        Task<MaGiamGiaDto?> UpdateDiscountAsync(Guid id, CreateMaGiamGiaRequest request);
    }

    public class DiscountService : IDiscountService
    {
        private readonly HttpClient _httpClient;

        public DiscountService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("DiscountAPI");
        }

        public async Task<MaGiamGiaDto?> GetDiscountByCodeAsync(string code)
        {
            var response = await _httpClient.GetAsync($"api/magiamgia/code/{code}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<MaGiamGiaDto>();
            }
            return null;
        }

        public async Task<IEnumerable<MaGiamGiaDto>> GetDiscountsAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<MaGiamGiaDto>>("api/magiamgia") ?? new List<MaGiamGiaDto>();
        }

        public async Task<bool> ApplyDiscountAsync(string code)
        {
            var response = await _httpClient.PatchAsync($"api/magiamgia/use/{code}", null);
            return response.IsSuccessStatusCode;
        }

        public async Task<MaGiamGiaDto?> CreateDiscountAsync(CreateMaGiamGiaRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("api/magiamgia", request);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadFromJsonAsync<MaGiamGiaDto>();
        }

        public async Task<MaGiamGiaDto?> UpdateDiscountAsync(Guid id, CreateMaGiamGiaRequest request)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/magiamgia/{id}", request);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadFromJsonAsync<MaGiamGiaDto>();
        }
    }
}