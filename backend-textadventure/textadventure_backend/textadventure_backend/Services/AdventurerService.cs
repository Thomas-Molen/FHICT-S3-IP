using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using textadventure_backend.Helpers;
using textadventure_backend.Models.Entities;
using textadventure_backend.Services.Interfaces;

namespace textadventure_backend.Services
{
    public class AdventurerService : IAdventurerService
    {
        private readonly HttpClient httpClient;
        private readonly AppSettings appSettings;
        public AdventurerService(HttpClient _httpClient, IOptions<AppSettings> _appSettings)
        {
            httpClient = _httpClient;
            appSettings = _appSettings.Value;
        }

        public async Task<Adventurers> GetAdventurer(int adventurerId)
        {
            using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, appSettings.EnityManagerURL + "Adventurer/get/" + adventurerId + "/" + appSettings.GameAccessToken))
            {
                var response = await httpClient.SendAsync(requestMessage);
                if (!response.IsSuccessStatusCode)
                {
                    throw new ArgumentException(response.ReasonPhrase);
                }
                return await response.Content.ReadAsAsync<Adventurers>();
            }
        }
    }
}
