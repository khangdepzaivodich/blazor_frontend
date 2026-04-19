using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using blazor_frontend.Models.BackendDTOs;

namespace blazor_frontend.Services
{
    public interface IDiscountService
    {
        Task<MaGiamGiaDto?> GetDiscountByCodeAsync(string code);
        Task<bool> ApplyDiscountAsync(string code);
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

        public async Task<bool> ApplyDiscountAsync(string code)
        {
            var response = await _httpClient.PatchAsync($"api/magiamgia/use/{code}", null);
            return response.IsSuccessStatusCode;
        }
    }
}