using System.Collections.Generic;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using blazor_frontend.Models.BackendDTOs;

namespace blazor_frontend.Services
{
    public interface IChatService
    {
        Task<IEnumerable<ChatMessageDto>> GetChatHistoryAsync(Guid sessionId);
    }

    public class ChatService : IChatService
    {
        private readonly HttpClient _httpClient;

        public ChatService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ChatAPI");
        }

        public async Task<IEnumerable<ChatMessageDto>> GetChatHistoryAsync(Guid sessionId)
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<ChatMessageDto>>($"api/chat/messages/{sessionId}")
                   ?? new List<ChatMessageDto>();
        }
    }
}