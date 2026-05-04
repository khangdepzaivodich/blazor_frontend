using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using blazor_frontend.Models.BackendDTOs;

namespace blazor_frontend.Services
{
    public interface IDiscountService
    {
        Task<PagedResult<MaGiamGiaDto>> GetDiscountsAsync(DiscountPaginationRequest request);
        Task<IEnumerable<MaGiamGiaDto>> GetDiscountsAsync(); // Keep old one for backward compatibility if needed, or update it
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

        public async Task<PagedResult<MaGiamGiaDto>> GetDiscountsAsync(DiscountPaginationRequest request)
        {
            var queryString = $"?pageNumber={request.PageNumber}&pageSize={request.PageSize}";
            if (!string.IsNullOrEmpty(request.Keyword)) queryString += $"&keyword={request.Keyword}";
            
            return await _httpClient.GetFromJsonAsync<PagedResult<MaGiamGiaDto>>($"api/magiamgia{queryString}") 
                   ?? new PagedResult<MaGiamGiaDto>();
        }

        public async Task<IEnumerable<MaGiamGiaDto>> GetDiscountsAsync()
        {
            // For backward compatibility, get first 100 items or similar
            var result = await GetDiscountsAsync(new DiscountPaginationRequest { PageNumber = 1, PageSize = 100 });
            return result.Items;
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
                var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                throw new Exception(error?.Message ?? "Có lỗi xảy ra khi tạo mã giảm giá.");
            }

            return await response.Content.ReadFromJsonAsync<MaGiamGiaDto>();
        }

        public async Task<MaGiamGiaDto?> UpdateDiscountAsync(Guid id, CreateMaGiamGiaRequest request)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/magiamgia/{id}", request);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                throw new Exception(error?.Message ?? "Có lỗi xảy ra khi cập nhật mã giảm giá.");
            }

            return await response.Content.ReadFromJsonAsync<MaGiamGiaDto>();
        }

        private class ErrorResponse
        {
            [System.Text.Json.Serialization.JsonPropertyName("message")]
            public string Message { get; set; } = string.Empty;
        }
    }
}