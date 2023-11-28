using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using textadventure_backend.Helpers;
using textadventure_backend.Models.Entities;
using textadventure_backend_entitymanager.Models.Responses;

namespace textadventure_backend.Services.ConnectionServices
{
    public interface IRoomConnectionService
    {
        Task CompleteRoom(int adventurerId);
        Task CreateSpawn(int adventurerId);
        Task<Rooms> GetRoom(int roomId);
        Task<bool> MoveTo(int adventurerId, string direction);
    }

    public class RoomConnectionService : IRoomConnectionService
    {
        private readonly HttpClient httpClient;
        private readonly AppSettings appSettings;
        public RoomConnectionService(HttpClient _httpClient, IOptions<AppSettings> _appSettings)
        {
            httpClient = _httpClient;
            appSettings = _appSettings.Value;
        }

        public async Task<Rooms> GetRoom(int roomId)
        {
            using (var roomRequest = new HttpRequestMessage(HttpMethod.Get, $"{appSettings.EnityManagerURL}Room/get/{roomId}/{appSettings.GameAccessToken}"))
            {
                var roomResponse = await httpClient.SendAsync(roomRequest);
                if (!roomResponse.IsSuccessStatusCode)
                {
                    throw new ArgumentException(roomResponse.ReasonPhrase);
                }
                return await roomResponse.Content.ReadFromJsonAsync<Rooms>();
            }
        }

        public async Task<bool> MoveTo(int adventurerId, string direction)
        {
            using (var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{appSettings.EnityManagerURL}Room/move-to/{appSettings.GameAccessToken}"))
            {
                var requestBody = JsonConvert.SerializeObject(new EnterRoomResponse { adventurerId = adventurerId, Direction = direction });
                requestMessage.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                var response = await httpClient.SendAsync(requestMessage);
                if (!response.IsSuccessStatusCode)
                {
                    throw new ArgumentException(response.ReasonPhrase);
                }
                return JsonConvert.DeserializeObject<bool>(response.Content.ReadAsStringAsync().Result);
            }
        }

        public async Task CompleteRoom(int adventurerId)
        {
            using (var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{appSettings.EnityManagerURL}Room/complete/{adventurerId}/{appSettings.GameAccessToken}"))
            {
                var response = await httpClient.SendAsync(requestMessage);
                if (!response.IsSuccessStatusCode)
                {
                    throw new ArgumentException(response.ReasonPhrase);
                }
            }
        }

        public async Task CreateSpawn(int adventurerId)
        {
            using (var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{appSettings.EnityManagerURL}Room/spawn/{adventurerId}/{appSettings.GameAccessToken}"))
            {
                var response = await httpClient.SendAsync(requestMessage);
                if (!response.IsSuccessStatusCode)
                {
                    throw new ArgumentException(response.ReasonPhrase);
                }
            }
        }
    }
}
