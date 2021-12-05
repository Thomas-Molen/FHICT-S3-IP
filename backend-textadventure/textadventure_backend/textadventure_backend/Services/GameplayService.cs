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
        private readonly CommandService commandService;

        public GameplayService(SessionManager _sessionManager, IHubContext<GameHub> _hubContext, RoomService _roomService, WeaponService _weaponService, AdventurerService _adventurerService, EnemyService _enemyService, CommandService _commandService)
        {
            sessionManager = _sessionManager;
            hubContext = _hubContext;
            roomService = _roomService;
            weaponService = _weaponService;
            adventurerService = _adventurerService;
            commandService = _commandService;
        }

        public async Task AddPlayer(string connectionId, int adventurerId)
        {
            //add player to the sessionManager for use later
            var adventurer = await adventurerService.GetAdventurer(adventurerId);
            string group = adventurer.DungeonId.ToString();

            sessionManager.AddSession(connectionId, new SessionAdventurer {
                Id = adventurerId,
                Name = adventurer.Name, 
                Damage = adventurer.Weapons.FirstOrDefault(w => w.Equiped)?.Attack ?? 0, 
                Health = adventurer.Health,
                Experience = adventurer.Experience}, 
                group);

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

            if (session.Adventurer.Health > 0)
            {
                await hubContext.Clients.Client(connectionId)
                    .SendAsync("ReceiveMessage", $"You look around and see {session.Room}");
            }


            //send the player's stats and weapons
            int damage = adventurer.Weapons.FirstOrDefault(w => w.Equiped)?.Attack ?? 0;
            await hubContext.Clients.Client(connectionId)
                    .SendAsync(
                        "UpdateAdventurer", 
                        new { id = adventurerId, 
                            experience = session.Adventurer.Experience, 
                            health = session.Adventurer.Health, 
                            name = session.Adventurer.Name, 
                            damage = session.Adventurer.Damage, 
                            roomsCleared = adventurer.AdventurerMaps.ToList().Count}
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
            if (session.Adventurer.Health < 1)
            {
                session.State = Enums.States.Dead;
            }

            //list of commands keywords to look out for
            switch (session.State)
            {
                case Enums.States.Exploring:
                    await commandService.HandleExploringCommands(connectionId, message);
                    break;
                case Enums.States.Fighting:
                    await commandService.HandleFightingCommands(connectionId, message);
                    break;
                case Enums.States.Dead:
                    await hubContext.Clients.Client(connectionId)
                    .SendAsync("ReceiveMessage", $"You are unable to move due to your fatal wounds...");
                    break;
                default:
                    await hubContext.Clients.Client(connectionId)
                    .SendAsync("ReceiveMessage", $"Something wen horribly wrong.. please contact the support \n error code: 001");
                    break;
            }
        }

        public async Task EquipWeapon(string connectionId, int weaponId)
        {
            var session = sessionManager.GetSession(connectionId);
            if (session.State == Enums.States.Dead)
            {
                return;
            }

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
