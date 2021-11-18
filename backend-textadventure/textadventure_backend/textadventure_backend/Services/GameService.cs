using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using textadventure_backend.Helpers;
using textadventure_backend.Models.Entities;
using textadventure_backend.Services.Interfaces;

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

        public async Task<string> EnterRoom(Adventurers adventurer)
        {
            if (adventurer.Room == null || true)
            {
                using (var requestMessage = new HttpRequestMessage(HttpMethod.Post, appSettings.EnityManagerURL + "Room/create-spawn/" + appSettings.GameAccessToken))
                {
                    requestMessage.Content = JsonContent.Create(new { adventurerId = adventurer.Id});
                    var response = await httpClient.SendAsync(requestMessage);
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new ArgumentException(response.ReasonPhrase);
                    }
                }
            }
            return "Room Generated";
        }
    }
}
