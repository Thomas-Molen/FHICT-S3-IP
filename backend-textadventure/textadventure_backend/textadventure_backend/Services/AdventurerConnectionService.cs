using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using textadventure_backend.Helpers;
using textadventure_backend.Models.Entities;
using textadventure_backend.Services.Interfaces;

namespace textadventure_backend.Services
{
    public class AdventurerConnectionService : IAdventurerConnectionService
    {
        private readonly HttpClient httpClient;
        private readonly AppSettings appSettings;
        public AdventurerConnectionService(HttpClient _httpClient, IOptions<AppSettings> _appSettings)
        {
            httpClient = _httpClient;
            appSettings = _appSettings.Value;
        }

        public async Task<Adventurers> GetAdventurer(int adventurerId)
        {
            using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"{appSettings.EnityManagerURL}HubAdventurer/get/{adventurerId}/{appSettings.GameAccessToken}"))
            {
                var response = await httpClient.SendAsync(requestMessage);
                if (!response.IsSuccessStatusCode)
                {
                    throw new ArgumentException(response.ReasonPhrase);
                }
                return await response.Content.ReadAsAsync<Adventurers>();
            }
        }

        public async Task SetHealth(int adventurerId, int health)
        {
            using (var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{appSettings.EnityManagerURL}HubAdventurer/set-health/{adventurerId}/{health}/{appSettings.GameAccessToken}"))
            {
                var response = await httpClient.SendAsync(requestMessage);
                if (!response.IsSuccessStatusCode)
                {
                    throw new ArgumentException(response.ReasonPhrase);
                }
            }
        }

        public async Task SetExperience(int adventurerId, int experience)
        {
            using (var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{appSettings.EnityManagerURL}HubAdventurer/set-experience/{adventurerId}/{experience}/{appSettings.GameAccessToken}"))
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
