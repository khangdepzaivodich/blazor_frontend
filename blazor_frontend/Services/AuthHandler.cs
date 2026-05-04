using System.Net.Http.Headers;
using blazor_frontend.Services;

namespace blazor_frontend.Services
{
    public class AuthHandler : DelegatingHandler
    {
        private readonly AuthState _authState;

        public AuthHandler(AuthState authState)
        {
            _authState = authState;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Nếu đã đăng nhập và CHƯA có header Authorization, tự động gắn Token vào
            if (_authState.IsAuthenticated && request.Headers.Authorization == null)
            {
                Console.WriteLine($"[DEBUG] AuthHandler: Gắn Token cho {request.RequestUri}");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _authState.Token);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
