using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using textadventure_backend.Helpers;
using textadventure_backend.Models.Entities;
using textadventure_backend.Models.Requests;

namespace textadventure_backend.Services
{
    public class WeaponConnectionService
    {
        private readonly HttpClient httpClient;
        private readonly AppSettings appSettings;
        public WeaponConnectionService(HttpClient _httpClient, IOptions<AppSettings> _appSettings)
        {
            httpClient = _httpClient;
            appSettings = _appSettings.Value;
        }

        public async Task<List<Weapons>> GetWeapons(int adventurerId)
        {
            using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"{appSettings.EnityManagerURL}Weapon/get/{adventurerId}/{appSettings.GameAccessToken}"))
            {
                var response = await httpClient.SendAsync(requestMessage);
                if (!response.IsSuccessStatusCode)
                {
                    throw new ArgumentException(response.ReasonPhrase);
                }
                return await response.Content.ReadAsAsync<List<Weapons>>();
            }
        }

        public async Task<Weapons> CreateWeapon(int adventurerId)
        {
            using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"{appSettings.EnityManagerURL}Weapon/generate/{adventurerId}/{appSettings.GameAccessToken}"))
            {
                var response = await httpClient.SendAsync(requestMessage);
                if (!response.IsSuccessStatusCode)
                {
                    throw new ArgumentException(response.ReasonPhrase);
                }
                return JsonConvert.DeserializeObject<Weapons>(response.Content.ReadAsStringAsync().Result);
            }
        }

        public async Task SetWeapon(int adventurerId, int weaponId)
        {
            using (var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{appSettings.EnityManagerURL}Weapon/equip/{appSettings.GameAccessToken}"))
            {
                var requestBody = JsonConvert.SerializeObject(new EquipWeaponRequest { AdventurerId = adventurerId, WeaponId = weaponId });
                requestMessage.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                var response = await httpClient.SendAsync(requestMessage);
                if (!response.IsSuccessStatusCode)
                {
                    throw new ArgumentException(response.ReasonPhrase);
                }
            }
        }
    }
}
