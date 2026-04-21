using System.Net.Http.Json;
using blazor_frontend.Models.BackendDTOs;

namespace blazor_frontend.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<DanhMucDto>> GetAllAsync();
        Task<DanhMucDto?> GetByIdAsync(Guid id);
        Task<DanhMucDto?> CreateAsync(DanhMucCreateUpdateRequest request);
        Task<bool> UpdateAsync(Guid id, DanhMucCreateUpdateRequest request);
        Task<bool> DeleteAsync(Guid id);
    }

    public class CategoryService : ICategoryService
    {
        private readonly HttpClient _httpClient;

        public CategoryService(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("CatalogAPI");
        }

        public async Task<IEnumerable<DanhMucDto>> GetAllAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<DanhMucDto>>("api/DanhMuc") ?? new List<DanhMucDto>();
        }

        public async Task<DanhMucDto?> GetByIdAsync(Guid id)
        {
            return await _httpClient.GetFromJsonAsync<DanhMucDto>($"api/DanhMuc/{id}");
        }

        public async Task<DanhMucDto?> CreateAsync(DanhMucCreateUpdateRequest request)
        {
            var res = await _httpClient.PostAsJsonAsync("api/DanhMuc", request);
            if (res.IsSuccessStatusCode) return await res.Content.ReadFromJsonAsync<DanhMucDto>();
            return null;
        }

        public async Task<bool> UpdateAsync(Guid id, DanhMucCreateUpdateRequest request)
        {
            var res = await _httpClient.PutAsJsonAsync($"api/DanhMuc/{id}", request);
            return res.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var res = await _httpClient.DeleteAsync($"api/DanhMuc/{id}");
            return res.IsSuccessStatusCode;
        }
    }
}
