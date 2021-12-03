using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using textadventure_backend.Hubs;
using textadventure_backend.Models.Entities;
using textadventure_backend.Models.Requests;
using textadventure_backend.Models.Session;

namespace textadventure_backend.Services
{
    public class GameplayService
    {
        private readonly SessionManager sessionManager;
        private readonly IHubContext<GameHub> hubContext;
        private readonly RoomService roomService;
        private readonly WeaponService weaponService;
        private readonly AdventurerService adventurerService;
        private readonly EnemyService enemyService;

        public GameplayService(SessionManager _sessionManager, IHubContext<GameHub> _hubContext, RoomService _roomService, WeaponService _weaponService, AdventurerService _adventurerService, EnemyService _enemyService)
        {
            sessionManager = _sessionManager;
            hubContext = _hubContext;
            roomService = _roomService;
            weaponService = _weaponService;
            adventurerService = _adventurerService;
            enemyService = _enemyService;
        }

        private string[] GetWorldCommands(string Event, bool EventCompleted)
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

        private string[] GetCombatCommands()
        {
            List<string> result = new List<string> { "attack", "run", "test" };
            return result.ToArray();
        }

        public async Task AddPlayer(string connectionId, int adventurerId)
        {
            //add player to the sessionManager for use later
            var adventurer = await adventurerService.GetAdventurer(adventurerId);
            string group = adventurer.DungeonId.ToString();

            sessionManager.AddSession(connectionId, new SessionAdventurer {Id = adventurerId, Name = adventurer.Name }, group);
            await hubContext.Groups.AddToGroupAsync(connectionId, group);

            await hubContext.Clients.Client(connectionId)
                    .SendAsync("ReceiveMessage", $"Welcome {adventurer.Name}");

            await hubContext.Clients.GroupExcept(group, connectionId)
                .SendAsync("ReceiveMessage", $"{adventurer.Name} Has entered the dungeon");

            //load either the player's first spawn or the room they were in
            Rooms room;
            if (adventurer.RoomId == null)
            {
                await roomService.CreateSpawn(adventurerId);
                adventurer = await adventurerService.GetAdventurer(adventurerId);
                room = adventurer.Room;

                await hubContext.Clients.Client(connectionId)
                    .SendAsync("ReceiveMessage", $"You wake up in a dark room and slightly moist room, \n In your hand you see a dog tag saying {adventurer.Name}\nYou stand up and see some kind of chest infront of you");
            }
            else
            {
                room = adventurer.Room;
                room.EventCompleted = adventurer.IsRoomCompleted(Convert.ToInt32(adventurer.RoomId));
            }

            sessionManager.UpdateSessionRoom(connectionId, room);
            var session = sessionManager.GetSession(connectionId);

            await hubContext.Clients.Client(connectionId)
                    .SendAsync("ReceiveMessage", $"You look around and see {session.Room}");


            //send the player's stats and weapons
            int damage = adventurer.Weapons.FirstOrDefault(w => w.Equiped).Attack ?? 0;
            await hubContext.Clients.Client(connectionId)
                    .SendAsync(
                        "UpdateAdventurer", 
                        new { id = adventurerId, 
                            experience = adventurer.Experience, 
                            health = adventurer.Health, 
                            name = adventurer.Name, 
                            damage = damage, 
                            roomsCleared = adventurer.AdventurerMaps.Where(am => am.EventCompleted).ToList().Count}
                    );

            await hubContext.Clients.Client(connectionId)
                    .SendAsync("UpdateWeapons", adventurer.Weapons.ToArray());
        }

        public async Task RemovePlayer(string connectionId)
        {
            var session = sessionManager.GetSession(connectionId);
            sessionManager.RemoveSession(connectionId);
            await hubContext.Groups.RemoveFromGroupAsync(connectionId, session.Group);

            await hubContext.Clients.GroupExcept(session.Group, connectionId)
                .SendAsync("ReceiveMessage", $"{session.Adventurer.Name} Has vanished from the dungeon");
        }

        public async Task ExecuteCommand(string message, string connectionId)
        {
            var session = sessionManager.GetSession(connectionId);

            //list of commands keywords to look out for
            string[] commands = GetWorldCommands(session.Room.Event, session.Room.EventCompleted);

            //list of direction keywords to look out for
            string direction = new string[] { "north", "east", "south", "west" }.FirstOrDefault<string>(s => message.Contains(s));
            switch (commands.FirstOrDefault<string>(s => message.Contains(s)))
            {
                //basic functionality commands
                case "help":
                    await hubContext.Clients.Client(connectionId)
                        .SendAsync("ReceiveMessage", "Available commands: " + string.Join(", ", commands));
                    return;
                case "test":
                    await hubContext.Clients.Client(connectionId)
                        .SendAsync("ReceiveMessage", "Test message");
                    return;
                case "clear":
                    await hubContext.Clients.Client(connectionId)
                        .SendAsync("ClearConsole");
                    return;
                case "say":
                    string sayResult = message.Replace("say ", "");
                    await hubContext.Clients.GroupExcept(session.Group, connectionId)
                        .SendAsync("ReceiveMessage", session.Adventurer.Name + " said " + sayResult);
                    await hubContext.Clients.Client(connectionId)
                        .SendAsync("ReceiveMessage", "sent " + sayResult);
                    return;
                //game logic
                
                //try to move the player in a direction
                case "go":
                    //force given direction into a properly formatted direction
                    switch (direction)
                    {
                        case "north":
                        case "east":
                        case "south":
                        case "west":
                            if(!await roomService.MoveTo(session.Adventurer.Id, direction))
                            {
                                await hubContext.Clients.Client(connectionId)
                                    .SendAsync("ReceiveMessage", $"With full confidence you walk through the door on the {direction} and... hit the wall \n guess there was no door there to begin with.");
                                break;
                            }
                            await hubContext.Clients.Client(connectionId)
                                .SendAsync("ReceiveMessage", $"You decide to go through the door to the {direction}");

                            var adventurer = await adventurerService.GetAdventurer(session.Adventurer.Id);
                            var newRoom = await roomService.GetRoom(Convert.ToInt32(adventurer.RoomId));
                            newRoom.EventCompleted = adventurer.IsRoomCompleted(newRoom.Id);
                            sessionManager.UpdateSessionRoom(connectionId, newRoom);

                            await hubContext.Clients.Client(connectionId)
                                .SendAsync("ReceiveMessage", $"You look around and see {session.Room}");
                            break;
                        default:
                            await hubContext.Clients.Client(connectionId)
                                .SendAsync("ReceiveMessage", $"Could not read direction did you spell the command right? \n Direction read: {message.Replace("go", "")}");
                            return;
                    }
                    return;
                //let the player look in a direction
                case "look":
                    switch (direction)
                    {
                        case "north":
                            await hubContext.Clients.Client(connectionId)
                                .SendAsync("ReceiveMessage", $"You look to the {direction} and see a {session.Room.NorthInteraction}");
                            break;
                        case "east":
                            await hubContext.Clients.Client(connectionId)
                                .SendAsync("ReceiveMessage", $"You look to the {direction} and see a {session.Room.EastInteraction}");
                            break;
                        case "south":
                            await hubContext.Clients.Client(connectionId)
                                .SendAsync("ReceiveMessage", $"You look to the {direction} and see a {session.Room.SouthInteraction}");
                            break;
                        case "west":
                            await hubContext.Clients.Client(connectionId)
                                .SendAsync("ReceiveMessage", $"You look to the {direction} and see a {session.Room.WestInteraction}");
                            break;
                        default:
                            await hubContext.Clients.Client(connectionId)
                                .SendAsync("ReceiveMessage", $"You see {session.Room}");
                            return;
                    }
                    return;

                //situation specific commands
                case "open":
                    var weapon = await weaponService.CreateWeapon(session.Adventurer.Id);
                    await hubContext.Clients.Client(connectionId)
                        .SendAsync("UpdateWeapons", await weaponService.GetWeapons(session.Adventurer.Id));

                    await roomService.CompleteRoom(session.Adventurer.Id);
                    session.Room.EventCompleted = true;

                    await hubContext.Clients.Client(connectionId)
                                .SendAsync("ReceiveMessage", $"You Open the chest and find a {weapon.Name}");
                    return;
                case "observe":
                    if (session.Enemy == null || session.Enemy.RoomId != session.Room.Id)
                    {
                        var enemy = await enemyService.CreateEnemy(session.Adventurer.Id, session.Room.Id);
                        session.Enemy = enemy;
                    }

                    await hubContext.Clients.Client(connectionId)
                            .SendAsync("UpdateEnemy", new
                            {
                                difficulty = session.Enemy.Difficulty,
                                name = session.Enemy.Name,
                                weapon = session.Enemy.Weapon.Name,
                                health = session.Enemy.Health,
                            });

                    await hubContext.Clients.Client(connectionId)
                                .SendAsync("ReceiveMessage", $"You take a moment to observe the weird looking creature in the room...\n It seems to be some kind of a {session.Enemy.Name}");
                    return;

                //response when no keywords are found
                default:
                    await hubContext.Clients.Client(connectionId)
                        .SendAsync("ReceiveMessage", $"Given command not found, did you misspell it? \n Command given: {message}");
                    return;
            }
        }

        public async Task EquipWeapon(string connectionId, int weaponId)
        {
            var session = sessionManager.GetSession(connectionId);

            await weaponService.SetWeapon(session.Adventurer.Id, weaponId);

            var weapons = await weaponService.GetWeapons(session.Adventurer.Id);
            var weaponBeingEquiped = weapons.FirstOrDefault(w => w.Id == weaponId);

            await hubContext.Clients.Client(connectionId)
                .SendAsync("UpdateWeapons", weapons);

            await hubContext.Clients.Client(connectionId)
                        .SendAsync("UpdateAttack", weaponBeingEquiped.Attack);

            await hubContext.Clients.Client(connectionId)
                    .SendAsync("ReceiveMessage", $"You put away your weapon and grab your {weaponBeingEquiped.Name}");
        }
    }
}
