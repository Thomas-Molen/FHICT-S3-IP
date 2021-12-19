﻿using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using textadventure_backend.Helpers;
using textadventure_backend.Models.Entities;
using textadventure_backend.Services.Interfaces;

namespace textadventure_backend.Services
{
    public class EnemyConnectionService : IEnemyConnectionService
    {
        private readonly HttpClient httpClient;
        private readonly AppSettings appSettings;
        public EnemyConnectionService(HttpClient _httpClient, IOptions<AppSettings> _appSettings)
        {
            httpClient = _httpClient;
            appSettings = _appSettings.Value;
        }

        public async Task<Enemy> CreateEnemy(int experience, int roomId)
        {
            using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"{appSettings.EnityManagerURL}Enemy/generate/{experience}/{appSettings.GameAccessToken}"))
            {
                var response = await httpClient.SendAsync(requestMessage);
                if (!response.IsSuccessStatusCode)
                {
                    throw new ArgumentException(response.ReasonPhrase);
                }

                Enemy enemy = JsonConvert.DeserializeObject<Enemy>(response.Content.ReadAsStringAsync().Result);
                enemy.RoomId = roomId;
                return enemy;
            }
        }
    }
}
