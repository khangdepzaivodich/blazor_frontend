using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using blazor_frontend.Models.BackendDTOs;

namespace blazor_frontend.Services
{
    public interface IOrderService
    {
        Task<DonHangDto?> CreateOrderAsync(CreateDonHangRequest request);
        Task<DonHangDto?> GetOrderByIdAsync(Guid maDH);
        Task<IEnumerable<DonHangDto>> GetOrdersByUserIdAsync(Guid maTK);
        Task<PagedDonHangResult?> GetAllPagedAsync(int page, int pageSize);
        Task<bool> UpdateOrderStatusAsync(Guid maDH, string newStatus);
        Task<bool> SyncSalesCountAsync();
    }
}
