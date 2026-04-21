using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using blazor_frontend.Models.BackendDTOs;

namespace blazor_frontend.Services
{
    public interface IBasketService
    {
        Task<BasketDto?> GetBasketAsync(string buyerId);
        Task<BasketDto?> UpdateBasketAsync(BasketDto basket);
        Task DeleteBasketAsync(string buyerId);
    }

    public class BasketService : IBasketService
    {
        private readonly HttpClient _httpClient;

        public BasketService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("BasketAPI");
        }

        public async Task<BasketDto?> GetBasketAsync(string buyerId)
        {
            return await _httpClient.GetFromJsonAsync<BasketDto>($"api/basket/{buyerId}");
        }

        public async Task<BasketDto?> UpdateBasketAsync(BasketDto basket)
        {
            var response = await _httpClient.PostAsJsonAsync("api/basket", new { cart = basket });
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<BasketDto>();
            }
            return null;
        }

        public async Task DeleteBasketAsync(string buyerId)
        {
            await _httpClient.DeleteAsync($"api/basket/{buyerId}");
        }
    }
}