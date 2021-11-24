using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Linq;
using System.Threading.Tasks;
using textadventure_backend.Models.Requests;
using textadventure_backend.Services.Interfaces;

namespace textadventure_backend.Hubs
{
    [Authorize]
    public class GameHub : Hub
    {
        private readonly IAdventurerService adventurerService;
        private readonly ISessionManager sessionManager;
        private readonly IGameService gameService;

        public GameHub(IAdventurerService _adventurerService, ISessionManager _sessionManager, IGameService _gameService)
        {
            adventurerService = _adventurerService;
            sessionManager = _sessionManager;
            gameService = _gameService;
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

                if (currentSession.Adventurer.RoomId == null)
                {
                    var generateSpawnResult = await gameService.GenerateSpawn(currentSession.Adventurer.Id);
                    await Clients.Caller
                        .SendAsync("ReceiveMessage", generateSpawnResult.Message);
                }
                else
                {
                    var loadResult = await gameService.LoadRoom(currentSession.Adventurer.Id);
                    await Clients.Caller
                        .SendAsync("ReceiveMessage", loadResult.Message);
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }

        public async Task SendCommand(string message)
        {
            var currentSession = sessionManager.GetSession(Context.ConnectionId);

            //list of commands keywords to look out for
            string[] commands = new string[] {"say", "go", "test", "look" };
            string command = commands.FirstOrDefault<string>(s => message.Contains(s));
            switch (command)
            {
                //tell something to all dungeon members
                case "say":
                    string sayResult = message.Replace("say", "");
                    await Clients.OthersInGroup(currentSession.Group)
                        .SendAsync("ReceiveMessage", currentSession.Adventurer.Name + " said " + sayResult);
                    await Clients.Caller
                        .SendAsync("ReceiveMessage", "sent " + sayResult);
                    return;
                //enter a new room
                case "go":
                    string direction = message.Replace("go ", "");

                    var enterRoomResult = await gameService.EnterRoom(await sessionManager.GetUpdatedAdventurer(Context.ConnectionId), direction);
                    await Clients.Caller
                        .SendAsync("ReceiveMessage", enterRoomResult.Message);
                    if (enterRoomResult.NewRoom)
                    {
                    var loadResult = await gameService.LoadRoom(currentSession.Adventurer.Id);
                    await Clients.Caller
                        .SendAsync("ReceiveMessage", loadResult.Message);
                    }
                    return;
                //let the player look in a direction
                case "look":
                    var reloadResult = await gameService.LoadRoom(currentSession.Adventurer.Id);
                    await Clients.Caller
                        .SendAsync("ReceiveMessage", reloadResult.Message);
                    return;
                //debug command
                case "test":
                    await Clients.Caller
                        .SendAsync("ReceiveMessage", "Testing message");
                    return;

                default:
                    await Clients.Caller
                        .SendAsync("ReceiveMessage", "Given command not found, did you misspell it?");
                    break;
            }
        }
    }
}
