using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using blazor_frontend.Models.BackendDTOs;

namespace blazor_frontend.Services
{
    public interface ICatalogService
    {
        Task<IEnumerable<ChiTietSanPhamDto>> GetProductsAsync(Guid maSP);
        Task<ChiTietSanPhamDto?> GetProductByIdAsync(Guid id);
    }

    public class CatalogService : ICatalogService
    {
        private readonly HttpClient _httpClient;

        public CatalogService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("CatalogAPI");
        }

        public async Task<IEnumerable<ChiTietSanPhamDto>> GetProductsAsync(Guid maSP)
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<ChiTietSanPhamDto>>($"api/ChiTietSanPham/by-sanpham/{maSP}")
                   ?? new List<ChiTietSanPhamDto>();
        }

        public async Task<ChiTietSanPhamDto?> GetProductByIdAsync(Guid id)
        {
            return await _httpClient.GetFromJsonAsync<ChiTietSanPhamDto>($"api/ChiTietSanPham/{id}");
        }
    }
}