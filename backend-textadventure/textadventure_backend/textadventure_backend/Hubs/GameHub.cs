using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using textadventure_backend.Models;
using textadventure_backend.Models.Entities;
using textadventure_backend.Services.Interfaces;

namespace textadventure_backend.Hubs
{
    [Authorize]
    public class GameHub : Hub
    {
        private readonly IAdventurerService adventurerService;

        private string JWTToken;
        private Adventurers adventurer;

        public GameHub(IAdventurerService _adventurerService)
        {
            adventurerService = _adventurerService;
        }

        public async Task JoinGame(JoinGameRequest request)
        {
            string dungeonGroup = request.DungeonId.ToString();
            await UpdateJWT(request.JWTToken);

            try
            {
                adventurer = await adventurerService.GetAdventurer(request.AdventurerId, JWTToken);

                await Groups.AddToGroupAsync(Context.ConnectionId, dungeonGroup);

                await Clients.Caller
                    .SendAsync("ReceiveMessage", "Welcome " + adventurer.Name);

                await Clients.OthersInGroup(dungeonGroup)
                    .SendAsync("ReceiveMessage", adventurer.Name + " Has entered the dungeon");
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }

        public async Task UpdateJWT(string _JWTToken)
        {
            JWTToken = _JWTToken;

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.ReadJwtToken(JWTToken);
            var delay = long.Parse(token.Claims.Where(c => c.Type == "exp").FirstOrDefault().Value) - DateTimeOffset.Now.ToUnixTimeSeconds();

            await Clients.Caller
                    .SendAsync("RenewJWT", (int)delay * 1000);
        }
    }
}
