﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Linq;
using System.Threading.Tasks;
using textadventure_backend.Models;
using textadventure_backend.Models.Requests;
using textadventure_backend.Models.Responses;
using textadventure_backend.Models.Session;
using textadventure_backend.Services;

namespace textadventure_backend.Hubs
{
    [Authorize]
    public class GameHub : Hub
    {
        private readonly GameplayService gameplayService;

        public GameHub(GameplayService _gameplayService)
        {
            gameplayService = _gameplayService;
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await gameplayService.RemovePlayer(Context.ConnectionId);
            
            await base.OnDisconnectedAsync(exception);
        }

        [HubMethodName("Join")]
        public async Task JoinGame(int adventurerId)
        {
            try
            {
                await gameplayService.AddPlayer(Context.ConnectionId, adventurerId);
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }

        [HubMethodName("SendCommand")]
        public async Task ReceiveCommand(string message)
        {
            await gameplayService.ExecuteCommand(message, Context.ConnectionId);
        }

        [HubMethodName("EquipWeapon")]
        public async Task EquipWeapon(int weaponId)
        {
            await gameplayService.EquipWeapon(Context.ConnectionId, weaponId);
        }
    }
}
