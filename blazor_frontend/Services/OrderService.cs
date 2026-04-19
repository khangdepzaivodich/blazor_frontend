using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using blazor_frontend.Models.BackendDTOs;

namespace blazor_frontend.Services
{
    public interface IOrderService
    {
        Task<DonHangDto?> CreateOrderAsync(CreateDonHangRequest request);
        Task<DonHangDto?> GetOrderByIdAsync(Guid maDH);
        Task<IEnumerable<DonHangDto>> GetOrdersByUserIdAsync(Guid maTK);
    }

    public class OrderService : IOrderService
    {
        private readonly HttpClient _httpClient;

        public OrderService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("OrderingAPI");
        }

        public async Task<DonHangDto?> CreateOrderAsync(CreateDonHangRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("api/donhang", request);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<DonHangDto>();
            }
            return null;
        }

        public async Task<DonHangDto?> GetOrderByIdAsync(Guid maDH)
        {
            return await _httpClient.GetFromJsonAsync<DonHangDto>($"api/donhang/{maDH}");
        }

        public async Task<IEnumerable<DonHangDto>> GetOrdersByUserIdAsync(Guid maTK)
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<DonHangDto>>($"api/donhang/user/{maTK}") ?? new List<DonHangDto>();
        }
    }
}