using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Linq;
using System.Threading.Tasks;
using textadventure_backend.Models;
using textadventure_backend.Models.Requests;
using textadventure_backend.Models.Responses;
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
                .SendAsync("ReceiveMessage", $"{Currentsession.Adventurer.Name} Has vanished from the dungeon");
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
                    .SendAsync("ReceiveMessage", $"Welcome {currentSession.Adventurer.Name}");

                await Clients.OthersInGroup(currentSession.Group)
                    .SendAsync("ReceiveMessage", $"{currentSession.Adventurer.Name} Has entered the dungeon");

                if (currentSession.Adventurer.RoomId == null)
                {
                    var generateSpawnResult = await gameService.GenerateSpawn(currentSession.Adventurer.Id);
                    var loadResult = await gameService.LoadRoom(currentSession.Adventurer.Id);
                    sessionManager.UpdateRoom(Context.ConnectionId, CreateSessionRoom(loadResult));
                    await Clients.Caller
                        .SendAsync("ReceiveMessage", generateSpawnResult.Message);
                }
                else
                {
                    var loadResult = await gameService.LoadRoom(currentSession.Adventurer.Id);
                    sessionManager.UpdateRoom(Context.ConnectionId, CreateSessionRoom(loadResult));
                    await Clients.Caller
                        .SendAsync("ReceiveMessage", loadResult.Message);
                }

                var weapons = await sessionManager.GetUpdatedWeapons(Context.ConnectionId);
                await Clients.Caller
                    .SendAsync("UpdateInventory", weapons.ToArray());
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
            string[] _commands = gameService.GetCommands(currentSession.Room.Event, currentSession.Room.EventCompleted);
            string _command = _commands.FirstOrDefault<string>(s => message.Contains(s));

            //list of direction keywords to look out for
            string[] _directions = new string[] { "north", "east", "south", "west" };
            string _direction = _directions.FirstOrDefault<string>(s => message.Contains(s));
            switch (_command)
            {
                //basic functionality commands
                case "help":
                    await Clients.Caller
                        .SendAsync("ReceiveMessage", "Available commands: " + string.Join(", ", _commands));
                    return;
                case "test":
                    await Clients.Caller
                        .SendAsync("ReceiveMessage", "Test message");
                    return;
                case "clear":
                    await Clients.Caller
                        .SendAsync("ClearConsole");
                    return;
                case "say":
                    string sayResult = message.Replace("say ", "");
                    await Clients.OthersInGroup(currentSession.Group)
                        .SendAsync("ReceiveMessage", currentSession.Adventurer.Name + " said " + sayResult);
                    await Clients.Caller
                        .SendAsync("ReceiveMessage", "sent " + sayResult);
                    return;
                //game logic
                case "go":
                    var enterRoomResult = new EnterRoomRequest();
                    //force given direction into a properly formatted direction
                    switch (_direction)
                    {
                        case "north":
                            enterRoomResult = await gameService.EnterRoom(await sessionManager.GetUpdatedAdventurer(Context.ConnectionId), _direction);
                            break;
                        case "east":
                            enterRoomResult = await gameService.EnterRoom(await sessionManager.GetUpdatedAdventurer(Context.ConnectionId), _direction);
                            break;
                        case "south":
                            enterRoomResult = await gameService.EnterRoom(await sessionManager.GetUpdatedAdventurer(Context.ConnectionId), _direction);
                            break;
                        case "west":
                            enterRoomResult = await gameService.EnterRoom(await sessionManager.GetUpdatedAdventurer(Context.ConnectionId), _direction);
                            break;
                        default:
                            await Clients.Caller
                                .SendAsync("ReceiveMessage", $"Could not read direction did you spell the command right? \n Direction read: {message.Replace("go", "")}");
                            return;
                    }
                    
                    await Clients.Caller
                        .SendAsync("ReceiveMessage", enterRoomResult.Message);
                    if (enterRoomResult.NewRoom)
                    {
                        var loadResult = await gameService.LoadRoom(currentSession.Adventurer.Id);
                        sessionManager.UpdateRoom(Context.ConnectionId, CreateSessionRoom(loadResult));
                        await Clients.Caller
                            .SendAsync("ReceiveMessage", loadResult.Message);
                    }
                    return;
                //let the player look in a direction
                case "look":
                    switch (_direction)
                    {
                        case "north":
                            await Clients.Caller
                                .SendAsync("ReceiveMessage", $"You look to the {_direction} and see a {currentSession.Room.NorthInteraction}");
                            break;
                        case "east":
                            await Clients.Caller
                                .SendAsync("ReceiveMessage", $"You look to the {_direction} and see a {currentSession.Room.EastInteraction}");
                            break;
                        case "south":
                            await Clients.Caller
                                .SendAsync("ReceiveMessage", $"You look to the {_direction} and see a {currentSession.Room.SouthInteraction}");
                            break;
                        case "west":
                            await Clients.Caller
                                .SendAsync("ReceiveMessage", $"You look to the {_direction} and see a {currentSession.Room.WestInteraction}");
                            break;
                        default:
                            var reloadResult = await gameService.LoadRoom(currentSession.Adventurer.Id);
                            sessionManager.UpdateRoom(Context.ConnectionId, CreateSessionRoom(reloadResult));
                            await Clients.Caller
                                .SendAsync("ReceiveMessage", reloadResult.Message);
                            return;
                    }
                    return;
                //situation specific commands
                case "open":
                    var openResult = await gameService.OpenChest(currentSession.Adventurer.Id);
                    await Clients.Caller
                        .SendAsync("ReceiveMessage", $"{openResult.Message}");

                    var weapons = await sessionManager.GetUpdatedWeapons(Context.ConnectionId);
                    await Clients.Caller
                        .SendAsync("UpdateInventory", weapons.ToArray());

                    sessionManager.CompleteRoom(Context.ConnectionId);
                    return;

                //response when no keywords are found
                default:
                    await Clients.Caller
                        .SendAsync("ReceiveMessage", $"Given command not found, did you misspell it? \n Command given: {message}");
                    break;
            }
        }

        public async Task EquipWeapon(int weaponId)
        {
            var currentSession = sessionManager.GetSession(Context.ConnectionId);

            var weaponToPutAway = currentSession.Adventurer.Weapons.ToList().Find(w => w.Equiped);
            if (weaponToPutAway.Id == weaponId)
            {
                return;
            }

            await gameService.EquipWeapon(currentSession.Adventurer.Id, weaponId);
            var weaponToEquip = currentSession.Adventurer.Weapons.ToList().Find(w => w.Id == weaponId);

            var adventurer = await sessionManager.GetUpdatedAdventurer(Context.ConnectionId);
            var weapons = await sessionManager.GetUpdatedWeapons(Context.ConnectionId);
            await Clients.Caller
                        .SendAsync("UpdateInventory", weapons.ToArray());
            await Clients.Caller
                        .SendAsync("UpdateStats", new GetAdventurerResponse
                        {
                            DungeonId = adventurer.DungeonId,
                            Id = adventurer.Id,
                            Name = adventurer.Name,
                            Damage = weaponToEquip.Attack,
                            Experience = adventurer.Experience,
                            Health = adventurer.Health
                        });

            await Clients.Caller
                    .SendAsync("ReceiveMessage", $"You put away your {weaponToPutAway.Name} and grab your {weaponToEquip.Name}");
        }

        private SessionRoom CreateSessionRoom(LoadRoomRequest loadResult)
        {
            return new SessionRoom
            {
                Event = loadResult.Event,
                EventCompleted = loadResult.EventCompleted,
                NorthInteraction = loadResult.NorthInteraction,
                EastInteraction = loadResult.EastInteraction,
                SouthInteraction = loadResult.SouthInteraction,
                WestInteraction = loadResult.WestInteraction
            };
        }
    }
}
