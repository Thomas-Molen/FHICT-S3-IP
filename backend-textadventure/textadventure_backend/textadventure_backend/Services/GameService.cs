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
using textadventure_backend_entitymanager.Models.Responses;

namespace textadventure_backend.Services
{
    public class GameService
    {
        private readonly HttpClient httpClient;
        private readonly AppSettings appSettings;
        public GameService(HttpClient _httpClient, IOptions<AppSettings> _appSettings)
        {
            httpClient = _httpClient;
            appSettings = _appSettings.Value;
        }

        public string[] GetCommands(string Event, bool EventCompleted)
        {
            List<string> result = new List<string> { "help", "say", "go", "look", "clear", "test" };
            if (!EventCompleted)
            {
                switch (Event.ToLower())
                {
                    case "empty":
                        result.Add("rest");
                        break;
                    case "enemy":
                        result.Add("fight");
                        result.Add("observe");
                        break;
                    case "chest":
                        result.Add("open");
                        break;
                    default:
                        break;
                }
            }
            return result.ToArray();
        }

        public string[] GetCombatCommands()
        {
            List<string> result = new List<string> { "attack", "run", "test" };
            return result.ToArray();
        }

        public async Task<EnterRoomRequest> EnterRoom(Adventurers adventurer, string direction)
        {
            EnterRoomRequest result = new EnterRoomRequest { Message = "Something went wrong" };

            using (var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{appSettings.EnityManagerURL}Room/enter/{appSettings.GameAccessToken}"))
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

            using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"{appSettings.EnityManagerURL}Room/load/{adventurerId}/{appSettings.GameAccessToken}"))
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

            using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"{appSettings.EnityManagerURL}Room/spawn/{adventurerId}/{appSettings.GameAccessToken}"))
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

        public async Task<OpenChestRequest> OpenChest(int adventurerId)
        {
            OpenChestRequest result = new OpenChestRequest { Message = "Something went wrong" };

            using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"{appSettings.EnityManagerURL}Weapon/generate/{adventurerId}/{appSettings.GameAccessToken}"))
            {
                var response = await httpClient.SendAsync(requestMessage);
                if (!response.IsSuccessStatusCode)
                {
                    throw new ArgumentException(response.ReasonPhrase);
                }
                result = JsonConvert.DeserializeObject<OpenChestRequest>(response.Content.ReadAsStringAsync().Result);
            }
            return result;
        }

        public async Task EquipWeapon(int adventurerId, int weaponId)
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
