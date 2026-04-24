using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using blazor_frontend.Models.BackendDTOs;

namespace blazor_frontend.Services
{
    public interface IUserService
    {
        Task<UserPaginatedResult?> GetAllUsersAsync(int page, int pageSize);
        Task<UserDto?> GetUserByIdAsync(Guid id);
        Task<Guid?> CreateUserAsync(CreateUserRequest request);
        Task<bool> UpdateUserAsync(Guid id, UpdateUserByAdminRequest request);
        Task<bool> DeleteUserAsync(Guid id);
        Task<bool> LockUserAsync(Guid id);
        Task<bool> UnlockUserAsync(Guid id);
        Task<UserDto?> GetMeAsync();
        Task<bool> UpdateMeAsync(UpdateMeRequest request);
        Task<string?> UploadAvatarAsync(Stream fileStream, string fileName);
    }
}
