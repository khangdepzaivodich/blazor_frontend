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
            // Nếu đã đăng nhập, tự động gắn Token vào Header
            if (_authState.IsAuthenticated)
            {
                Console.WriteLine($"[DEBUG] AuthHandler({_authState.GetHashCode()}): Gắn Token cho {request.RequestUri}");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _authState.Token);
            }
            else 
            {
                Console.WriteLine($"[DEBUG] AuthHandler({_authState.GetHashCode()}): CHƯA ĐĂNG NHẬP cho {request.RequestUri}");
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
