using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using textadventure_backend.Helpers;
using textadventure_backend.Models.Entities;
using textadventure_backend.Models.Requests;
using textadventure_backend.Services.Interfaces;
using textadventure_backend_entitymanager.Models.Responses;

namespace textadventure_backend.Services
{
    public class GameService : IGameService
    {
        private readonly HttpClient httpClient;
        private readonly AppSettings appSettings;
        public GameService(HttpClient _httpClient, IOptions<AppSettings> _appSettings)
        {
            httpClient = _httpClient;
            appSettings = _appSettings.Value;
        }

        public async Task<EnterRoomRequest> EnterRoom(Adventurers adventurer, string direction)
        {
            EnterRoomRequest result = new EnterRoomRequest{ Message = "Something went wrong" };

            using (var requestMessage = new HttpRequestMessage(HttpMethod.Post, appSettings.EnityManagerURL + "Room/enter/" + appSettings.GameAccessToken))
            {
                var requestBody = JsonConvert.SerializeObject(new EnterRoomResponse { adventurerId = adventurer.Id, Direction = direction });
                requestMessage.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                var response = await httpClient.SendAsync(requestMessage);
                if (!response.IsSuccessStatusCode)
                {
                    throw new ArgumentException(response.ReasonPhrase);
                }
                result = JsonConvert.DeserializeObject<EnterRoomRequest>(response.Content.ReadAsStringAsync().Result);
            }
            return result;
        }

        public async Task<LoadRoomRequest> LoadRoom(int adventurerId)
        {
            LoadRoomRequest result = new LoadRoomRequest { Message = "Something went wrong" };

            using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, appSettings.EnityManagerURL + "Room/load/" + adventurerId + "/" + appSettings.GameAccessToken))
            {
                var response = await httpClient.SendAsync(requestMessage);
                if (!response.IsSuccessStatusCode)
                {
                    throw new ArgumentException(response.ReasonPhrase);
                }
                result = JsonConvert.DeserializeObject<LoadRoomRequest>(response.Content.ReadAsStringAsync().Result);
            }
            return result;
        }

        public async Task<EnterRoomRequest> GenerateSpawn(int adventurerId)
        {
            EnterRoomRequest result = new EnterRoomRequest { Message = "Something went wrong" };

            using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, appSettings.EnityManagerURL + "Room/spawn/" + adventurerId + "/" + appSettings.GameAccessToken))
            {
                var response = await httpClient.SendAsync(requestMessage);
                if (!response.IsSuccessStatusCode)
                {
                    throw new ArgumentException(response.ReasonPhrase);
                }
                result = JsonConvert.DeserializeObject<EnterRoomRequest>(response.Content.ReadAsStringAsync().Result);
            }
            return result;
        }
    }
}
