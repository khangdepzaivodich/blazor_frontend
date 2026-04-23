namespace blazor_frontend.Services
{
    public class AuthState
    {
        public event Action? AuthStateChanged;

        public string? Token { get; private set; }
        public Guid? UserId { get; private set; }
        public string? HoTen { get; private set; }

        public bool IsAuthenticated => !string.IsNullOrEmpty(Token);

        public void SetUser(string token, Guid userId, string hoTen)
        {
            Token = token;
            UserId = userId;
            HoTen = hoTen;
            AuthStateChanged?.Invoke();
        }

        public void Clear()
        {
            Token = null;
            UserId = null;
            HoTen = null;
            AuthStateChanged?.Invoke();
        }
    }
}