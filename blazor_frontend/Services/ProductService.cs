using System.Net.Http.Json;
using blazor_frontend.Models.BackendDTOs;

namespace blazor_frontend.Services
{
    public interface IProductService
    {
        Task<IEnumerable<SanPhamDto>> GetAllAsync();
        Task<PagedSanPhamResponse?> GetPagedAsync(int pageNumber, int pageSize, Guid? categoryTypeId = null, Guid? categoryId = null, string? keyword = null, decimal? minPrice = null, decimal? maxPrice = null);
        Task<SanPhamDto?> GetByIdAsync(Guid id);
        Task<SanPhamDto?> CreateAsync(SanPhamCreateRequest request);
        Task<bool> UpdateAsync(Guid id, SanPhamCreateRequest request);
        Task<bool> DeleteAsync(Guid id);
    }

    public class PagedSanPhamResponse
    {
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public List<SanPhamDto> Data { get; set; } = new();
    }

    public class ProductService : IProductService
    {
        private readonly HttpClient _httpClient;

        public ProductService(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("CatalogAPI");
        }

        public async Task<IEnumerable<SanPhamDto>> GetAllAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<PagedSanPhamResponse>("api/sanpham?pageNumber=1&pageSize=1000");
            return response?.Data ?? new List<SanPhamDto>();
        }

        public async Task<PagedSanPhamResponse?> GetPagedAsync(int pageNumber, int pageSize, Guid? categoryTypeId = null, Guid? categoryId = null, string? keyword = null, decimal? minPrice = null, decimal? maxPrice = null)
        {
            var url = $"api/sanpham?pageNumber={pageNumber}&pageSize={pageSize}";
            if (categoryTypeId.HasValue) url += $"&maLDM={categoryTypeId.Value}";
            if (categoryId.HasValue) url += $"&maDM={categoryId.Value}";
            if (!string.IsNullOrWhiteSpace(keyword)) url += $"&keyword={Uri.EscapeDataString(keyword)}";
            
            if (minPrice.HasValue) 
                url += $"&minPrice={minPrice.Value.ToString(System.Globalization.CultureInfo.InvariantCulture)}";
            if (maxPrice.HasValue) 
                url += $"&maxPrice={maxPrice.Value.ToString(System.Globalization.CultureInfo.InvariantCulture)}";
            
            return await _httpClient.GetFromJsonAsync<PagedSanPhamResponse>(url);
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
