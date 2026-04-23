using System.Net.Http.Json;
using blazor_frontend.Models.BackendDTOs;

namespace blazor_frontend.Services
{
    public interface ICategoryService
    {
        // Loai Danh Muc
        Task<IEnumerable<LoaiDanhMucDto>> GetCategoryTypesAsync();
        Task<IEnumerable<LoaiDanhMucDto>> GetAllLoaiDanhMucAsync();
        Task<LoaiDanhMucDto?> GetCategoryTypeByIdAsync(Guid id);
        Task<bool> CreateLoaiDanhMucAsync(LoaiDanhMucCreateUpdateRequest request);
        Task<bool> UpdateLoaiDanhMucAsync(Guid id, LoaiDanhMucCreateUpdateRequest request);
        Task<bool> DeleteLoaiDanhMucAsync(Guid id);

        // Danh Muc
        Task<IEnumerable<DanhMucDto>> GetAllAsync();
        Task<DanhMucDto?> GetByIdAsync(Guid id);
        Task<bool> CreateAsync(DanhMucCreateUpdateRequest request);
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

        // --- LOAI DANH MUC ---
        public async Task<IEnumerable<LoaiDanhMucDto>> GetCategoryTypesAsync() => await GetAllLoaiDanhMucAsync();

        public async Task<IEnumerable<LoaiDanhMucDto>> GetAllLoaiDanhMucAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<IEnumerable<LoaiDanhMucDto>>("api/LoaiDanhMuc");
                return response ?? new List<LoaiDanhMucDto>();
            }
            catch { return new List<LoaiDanhMucDto>(); }
        }

        public async Task<LoaiDanhMucDto?> GetCategoryTypeByIdAsync(Guid id)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<LoaiDanhMucDto>($"api/LoaiDanhMuc/{id}");
            }
            catch { return null; }
        }

        public async Task<bool> CreateLoaiDanhMucAsync(LoaiDanhMucCreateUpdateRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("api/LoaiDanhMuc", request);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateLoaiDanhMucAsync(Guid id, LoaiDanhMucCreateUpdateRequest request)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/LoaiDanhMuc/{id}", request);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteLoaiDanhMucAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"api/LoaiDanhMuc/{id}");
            return response.IsSuccessStatusCode;
        }

        // --- DANH MUC ---
        public async Task<IEnumerable<DanhMucDto>> GetAllAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<IEnumerable<DanhMucDto>>("api/DanhMuc");
                return response ?? new List<DanhMucDto>();
            }
            catch { return new List<DanhMucDto>(); }
        }

        public async Task<DanhMucDto?> GetByIdAsync(Guid id)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<DanhMucDto>($"api/DanhMuc/{id}");
            }
            catch { return null; }
        }

        public async Task<bool> CreateAsync(DanhMucCreateUpdateRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("api/DanhMuc", request);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAsync(Guid id, DanhMucCreateUpdateRequest request)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/DanhMuc/{id}", request);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"api/DanhMuc/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
