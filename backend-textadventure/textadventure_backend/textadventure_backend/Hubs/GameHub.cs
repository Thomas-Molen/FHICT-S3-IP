using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;
using textadventure_backend.Services.Interfaces;

namespace textadventure_backend.Hubs
{
    [Authorize]
    public class GameHub : Hub
    {
        private readonly IAdventurerService adventurerService;
        private readonly ISessionManager sessionManager;

        public GameHub(IAdventurerService _adventurerService, ISessionManager _sessionManager)
        {
            adventurerService = _adventurerService;
            sessionManager = _sessionManager;
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var Currentsession = sessionManager.GetSession(Context.ConnectionId);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, Currentsession.Group);
            sessionManager.RemoveSession(Context.ConnectionId);
            await Clients.Group(Currentsession.Group)
                .SendAsync("ReceiveMessage", Currentsession.Adventurer.Name + " Has vanished from the dungeon");
            await base.OnDisconnectedAsync(exception);
        }

        public async Task JoinGame(int adventurerId)
        {
            try
            {
                await sessionManager.AddSession(Context.ConnectionId, adventurerId);
                var currentSession = sessionManager.GetSession(Context.ConnectionId);

                await Groups.AddToGroupAsync(Context.ConnectionId, currentSession.Group);

                await Clients.Caller
                    .SendAsync("ReceiveMessage", "Welcome " + currentSession.Adventurer.Name);

                await Clients.OthersInGroup(currentSession.Group)
                    .SendAsync("ReceiveMessage", currentSession.Adventurer.Name + " Has entered the dungeon");
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }

        public async Task SendCommand(string message)
        {
            var currentSession = sessionManager.GetSession(Context.ConnectionId);
            await Clients.Group(currentSession.Group)
                .SendAsync("ReceiveMessage", currentSession.Adventurer.Name + " Said " + message);
        }
    }
}
