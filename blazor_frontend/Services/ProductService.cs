using System.Net.Http.Json;
using blazor_frontend.Models.BackendDTOs;

namespace blazor_frontend.Services
{
    public interface IProductService
    {
        Task<IEnumerable<SanPhamDto>> GetAllAsync();
        Task<SanPhamDto?> GetByIdAsync(Guid id);
        Task<SanPhamDto?> CreateAsync(SanPhamCreateRequest request);
        Task<bool> UpdateAsync(Guid id, SanPhamCreateRequest request);
        Task<bool> DeleteAsync(Guid id);
    }

    public class ProductService : IProductService
    {
        private readonly HttpClient _httpClient;

        private sealed class PagedSanPhamResponse
        {
            public int TotalCount { get; set; }
            public int PageNumber { get; set; }
            public int PageSize { get; set; }
            public int TotalPages { get; set; }
            public List<SanPhamDto> Data { get; set; } = new();
        }

        public ProductService(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("CatalogAPI");
        }

        public async Task<IEnumerable<SanPhamDto>> GetAllAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<PagedSanPhamResponse>("api/sanpham?pageNumber=1&pageSize=1000");
            return response?.Data ?? new List<SanPhamDto>();
        }

        public async Task<SanPhamDto?> GetByIdAsync(Guid id)
        {
            return await _httpClient.GetFromJsonAsync<SanPhamDto>($"api/sanpham/{id}");
        }

        public async Task<SanPhamDto?> CreateAsync(SanPhamCreateRequest request)
        {
            var res = await _httpClient.PostAsJsonAsync("api/sanpham", request);
            if (res.IsSuccessStatusCode) return await res.Content.ReadFromJsonAsync<SanPhamDto>();
            return null;
        }

        public async Task<bool> UpdateAsync(Guid id, SanPhamCreateRequest request)
        {
            var res = await _httpClient.PutAsJsonAsync($"api/sanpham/{id}", request);
            return res.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var res = await _httpClient.DeleteAsync($"api/sanpham/{id}");
            return res.IsSuccessStatusCode;
        }
    }
}
