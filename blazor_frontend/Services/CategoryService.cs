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

        // Loại Danh Mục
        Task<IEnumerable<LoaiDanhMucDto>> GetAllLoaiDanhMucAsync();
        Task<LoaiDanhMucDto?> CreateLoaiDanhMucAsync(LoaiDanhMucCreateUpdateRequest request);
        Task<bool> UpdateLoaiDanhMucAsync(Guid id, LoaiDanhMucCreateUpdateRequest request);
        Task<bool> DeleteLoaiDanhMucAsync(Guid id);
    }

    public class CategoryService : ICategoryService
    {
        private readonly HttpClient _httpClient;

        public CategoryService(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("CatalogAPI");
        }

        // --- Danh Mục (CHILD) ---
        public async Task<IEnumerable<DanhMucDto>> GetAllAsync()
        {
            try
            {
                var children = await _httpClient.GetFromJsonAsync<IEnumerable<DanhMucDto>>("api/danhmuc");
                var parents = await _httpClient.GetFromJsonAsync<IEnumerable<LoaiDanhMucDto>>("api/loaidanhmuc");

                if (children == null) return new List<DanhMucDto>();

                return children.Select(c => {
                    var parentName = parents?.FirstOrDefault(p => p.MaLDM == c.MaLDM)?.TenLDM ?? string.Empty;
                    c.TenLDM = parentName;
                    c.Slug = GenerateSlug(c.TenDM);
                    return c;
                }).ToList();
            }
            catch { return new List<DanhMucDto>(); }
        }

        public async Task<DanhMucDto?> GetByIdAsync(Guid id)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<DanhMucDto>($"api/danhmuc/{id}");
            }
            catch { return null; }
        }

        public async Task<DanhMucDto?> CreateAsync(DanhMucCreateUpdateRequest request)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/danhmuc", request);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<DanhMucDto>();
                }
                return null;
            }
            catch { return null; }
        }

        public async Task<bool> UpdateAsync(Guid id, DanhMucCreateUpdateRequest request)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/danhmuc/{id}", request);
                return response.IsSuccessStatusCode;
            }
            catch { return false; }
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/danhmuc/{id}");
                return response.IsSuccessStatusCode;
            }
            catch { return false; }
        }

        // --- Loại Danh Mục (PARENT) ---
        public async Task<IEnumerable<LoaiDanhMucDto>> GetAllLoaiDanhMucAsync()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<IEnumerable<LoaiDanhMucDto>>("api/loaidanhmuc") ?? new List<LoaiDanhMucDto>();
            }
            catch { return new List<LoaiDanhMucDto>(); }
        }

        public async Task<LoaiDanhMucDto?> CreateLoaiDanhMucAsync(LoaiDanhMucCreateUpdateRequest request)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/loaidanhmuc", request);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<LoaiDanhMucDto>();
                }
                return null;
            }
            catch { return null; }
        }

        public async Task<bool> UpdateLoaiDanhMucAsync(Guid id, LoaiDanhMucCreateUpdateRequest request)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/loaidanhmuc/{id}", request);
                return response.IsSuccessStatusCode;
            }
            catch { return false; }
        }

        public async Task<bool> DeleteLoaiDanhMucAsync(Guid id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/loaidanhmuc/{id}");
                return response.IsSuccessStatusCode;
            }
            catch { return false; }
        }

        private string GenerateSlug(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return "";
            text = text.ToLowerInvariant().Trim();
            
            var normalizedString = text.Normalize(System.Text.NormalizationForm.FormD);
            var stringBuilder = new System.Text.StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != System.Globalization.UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            text = stringBuilder.ToString().Normalize(System.Text.NormalizationForm.FormC);
            text = text.Replace("đ", "d");
            text = System.Text.RegularExpressions.Regex.Replace(text, @"[^a-z0-9\s-]", "");
            text = System.Text.RegularExpressions.Regex.Replace(text, @"\s+", " ").Trim();
            text = text.Replace(" ", "-");
            return text;
        }
    }
}
