using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;
using textadventure_backend.Models.Requests;
using textadventure_backend.Services;

namespace textadventure_backend.Hubs
{
    [Authorize]
    public class GameHub : Hub
    {
        private readonly IGameplayService gameplayService;

        public GameHub(IGameplayService _gameplayService)
        {
            gameplayService = _gameplayService;
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await gameplayService.RemovePlayer(Context.ConnectionId);
            
            await base.OnDisconnectedAsync(exception);
        }

        [HubMethodName("Join")]
        public async Task JoinGame(JoinGameRequest request)
        {
            try
            {
                await gameplayService.AddPlayer(Context.ConnectionId, request.adventurerId, request.userId);
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
