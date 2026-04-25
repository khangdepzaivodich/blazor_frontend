using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using blazor_frontend.Models;

namespace blazor_frontend.Services
{
    public interface IChatService : IAsyncDisposable
    {
        HubConnectionState ConnectionState { get; }
        event Action<ChatMessageDto>? OnMessageReceived;
        event Action<ChatSessionDto>? OnNewChatAssigned;
        event Action<ChatSessionDto>? OnNewChatWaiting;
        event Action? OnSessionQueued;
        event Action<string, string, string>? OnSessionAssigned;
        event Action<string>? OnSessionClosed;
        event Action<string, string>? OnSessionReopened;
        event Action<string, string>? OnSessionUpgraded;
        event Action<string>? OnStaffNameUpdated;

        Task InitializeAsync();
        Task<IEnumerable<ChatMessageDto>> GetChatHistoryAsync(Guid sessionId);
        Task<ChatSessionDto?> GetChatSessionAsync(Guid sessionId);
        Task<IEnumerable<ChatSessionDto>> GetAllChatSessionsAsync();
        Task<ChatSessionDto?> GetLatestActiveSessionAsync(string userId);
        
        Task RegisterStaffAsync(string staffId, string staffName, string staffAvatar);
        Task<string> CreateNewChatSessionAsync(string userId, string clientType);
        Task JoinChatSessionAsync(string sessionId);
        Task SendMessageAsync(ChatMessageDto message);
        Task MarkAsReadAsync(string sessionId);
        Task UpgradeSessionAsync(string sessionId, string userId, string hoTen, string? avatar = null);
        Task CloseSessionAsync(string sessionId, string staffId);
        Task ReopenSessionAsync(string sessionId, string staffId);
        Task AssignSessionAsync(string sessionId, string staffId);
    }

    public class ChatService : IChatService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthState _authState;
        private HubConnection? _hubConnection;
        private readonly string _hubUrl;

        public event Action<ChatMessageDto>? OnMessageReceived;
        public event Action<ChatSessionDto>? OnNewChatAssigned;
        public event Action<ChatSessionDto>? OnNewChatWaiting;
        public event Action? OnSessionQueued;
        public event Action<string, string, string>? OnSessionAssigned;
        public event Action<string>? OnSessionClosed;
        public event Action<string, string>? OnSessionReopened;
        public event Action<string, string>? OnSessionUpgraded;
        public event Action<string>? OnStaffNameUpdated;

        public HubConnectionState ConnectionState => _hubConnection?.State ?? HubConnectionState.Disconnected;

        public ChatService(IHttpClientFactory httpClientFactory, AuthState authState)
        {
            _httpClient = httpClientFactory.CreateClient("ChatAPI");
            _authState = authState;
            _hubUrl = _httpClient.BaseAddress?.ToString().TrimEnd('/') + "/chat-hub";
        }

        public async Task InitializeAsync()
        {
            if (_hubConnection != null) return;

            _hubConnection = new HubConnectionBuilder()
                .WithUrl(_hubUrl, options => {
                    options.AccessTokenProvider = () => Task.FromResult(_authState.Token)!;
                })
                .WithAutomaticReconnect()
                .Build();

            _hubConnection.On<ChatMessageDto>("ReceiveNewMessage", (message) => OnMessageReceived?.Invoke(message));
            _hubConnection.On<ChatSessionDto>("NewChatAssigned", (session) => OnNewChatAssigned?.Invoke(session));
            _hubConnection.On<ChatSessionDto>("NewChatWaiting", (session) => OnNewChatWaiting?.Invoke(session));
            _hubConnection.On("SessionQueued", () => OnSessionQueued?.Invoke());
            _hubConnection.On<string, string, string>("SessionAssigned", (sessionId, staffId, staffName) => OnSessionAssigned?.Invoke(sessionId, staffId, staffName));
            _hubConnection.On<string>("SessionClosed", (sessionId) => OnSessionClosed?.Invoke(sessionId));
            _hubConnection.On<string, string>("SessionReopened", (sessionId, status) => OnSessionReopened?.Invoke(sessionId, status));
            _hubConnection.On<string, string>("SessionUpgraded", (sessionId, hoTen) => OnSessionUpgraded?.Invoke(sessionId, hoTen));
            _hubConnection.On<string>("StaffNameUpdated", (newName) => OnStaffNameUpdated?.Invoke(newName));

            await _hubConnection.StartAsync();
        }

        public async Task<IEnumerable<ChatMessageDto>> GetChatHistoryAsync(Guid sessionId)
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<ChatMessageDto>>($"api/chat/messages/{sessionId}")
                   ?? new List<ChatMessageDto>();
        }

        public async Task<IEnumerable<ChatSessionDto>> GetAllChatSessionsAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<ChatSessionDto>>("api/chat/chat-sessions")
                   ?? new List<ChatSessionDto>();
        }

        public async Task<ChatSessionDto?> GetChatSessionAsync(Guid sessionId)
        {
            return await _httpClient.GetFromJsonAsync<ChatSessionDto>($"api/chat/chat-sessions/{sessionId}");
        }

        public async Task<ChatSessionDto?> GetLatestActiveSessionAsync(string userId)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<ChatSessionDto>($"api/chat/latest-session/{userId}");
            }
            catch
            {
                return null;
            }
        }

        public async Task RegisterStaffAsync(string staffId, string staffName, string staffAvatar)
        {
            if (_hubConnection == null) await InitializeAsync();
            await _hubConnection!.SendAsync("RegisterStaff", staffId, staffName, staffAvatar);
        }

        public async Task<string> CreateNewChatSessionAsync(string userId, string clientType)
        {
            if (_hubConnection == null) await InitializeAsync();
            return await _hubConnection!.InvokeAsync<string>("CreateNewChatSession", userId, clientType);
        }

        public async Task JoinChatSessionAsync(string sessionId)
        {
            if (_hubConnection == null) await InitializeAsync();
            await _hubConnection!.SendAsync("JoinChatSession", sessionId);
        }

        public async Task SendMessageAsync(ChatMessageDto message)
        {
            if (_hubConnection == null) await InitializeAsync();
            await _hubConnection!.SendAsync("SendMessage", message);
        }

        public async Task MarkAsReadAsync(string sessionId)
        {
            if (_hubConnection == null) await InitializeAsync();
            await _hubConnection!.SendAsync("MarkAsRead", sessionId);
        }

        public async Task UpgradeSessionAsync(string sessionId, string userId, string hoTen, string? avatar = null)
        {
            if (_hubConnection == null) await InitializeAsync();
            await _hubConnection!.SendAsync("UpgradeSession", sessionId, userId, hoTen, avatar);
        }

        public async Task CloseSessionAsync(string sessionId, string staffId)
        {
            if (_hubConnection == null) await InitializeAsync();
            await _hubConnection!.SendAsync("CloseSession", sessionId, staffId);
        }

        public async Task ReopenSessionAsync(string sessionId, string staffId)
        {
            if (_hubConnection == null) await InitializeAsync();
            await _hubConnection!.SendAsync("ReopenSession", sessionId, staffId);
        }

        public async Task AssignSessionAsync(string sessionId, string staffId)
        {
            if (_hubConnection == null) await InitializeAsync();
            await _hubConnection!.SendAsync("AssignSession", sessionId, staffId);
        }

        public async ValueTask DisposeAsync()
        {
            if (_hubConnection != null)
            {
                await _hubConnection.DisposeAsync();
            }
        }
    }
}