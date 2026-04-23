using System;

namespace blazor_frontend.Services
{
    public class AuthState
    {
        public event Action? AuthStateChanged;

        // non-nullable auth properties requested
        public string Token { get; private set; } = string.Empty;
        public Guid UserId { get; private set; } = Guid.Empty;
        public string Email { get; private set; } = string.Empty;
        public string Role { get; private set; } = string.Empty;
        public string HoTen { get; private set; } = string.Empty;

        public bool IsAuthenticated => !string.IsNullOrEmpty(Token);

        // Primary setter with full metadata
        public void SetUser(string token, Guid userId, string email, string role, string hoTen)
        {
            Token = token ?? string.Empty;
            UserId = userId;
            Email = email ?? string.Empty;
            Role = role ?? string.Empty;
            HoTen = hoTen ?? string.Empty;
            AuthStateChanged?.Invoke();
        }

        public void Clear()
        {
            Token = string.Empty;
            UserId = Guid.Empty;
            Email = string.Empty;
            Role = string.Empty;
            HoTen = string.Empty;
            AuthStateChanged?.Invoke();
        }
    }
}