using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using textadventure_backend.Enums;
using textadventure_backend.Hubs;

namespace textadventure_backend.Services
{
    public class CommandService
    {
        private readonly SessionManager sessionManager;
        private readonly IHubContext<GameHub> hubContext;
        private readonly RoomService roomService;
        private readonly WeaponService weaponService;
        private readonly AdventurerService adventurerService;
        private readonly EnemyService enemyService;
        private readonly CombatService combatService;

        public CommandService(SessionManager _sessionManager, IHubContext<GameHub> _hubContext, RoomService _roomService, WeaponService _weaponService, AdventurerService _adventurerService, EnemyService _enemyService, CombatService _combatService)
        {
            sessionManager = _sessionManager;
            hubContext = _hubContext;
            roomService = _roomService;
            weaponService = _weaponService;
            adventurerService = _adventurerService;
            enemyService = _enemyService;
            combatService = _combatService;
        }

        private List<string> GetWorldCommands(string Event, bool EventCompleted)
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
                        result.Add("attack");
                        result.Add("observe");
                        break;
                    case "chest":
                        result.Add("open");
                        break;
                    default:
                        break;
                }
            }
            return result;
        }

        private List<string> GetCombatCommands()
        {
            List<string> result = new List<string> { "help", "attack", "run", "test" };
            return result;
        }

        public async Task HandleExploringCommands(string connectionId, string message)
        {
            var session = sessionManager.GetSession(connectionId);
            //list of direction keywords to look out for
            List<string> commands = GetWorldCommands(session.Room.Event, session.Room.EventCompleted);
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
                            if (!await roomService.MoveTo(session.Adventurer.Id, direction))
                            {
                                //if there is no wall in this direction
                                await hubContext.Clients.Client(connectionId)
                                    .SendAsync("ReceiveMessage", $"With full confidence you walk through the door on the {direction} and... hit the wall \n guess there was no door there to begin with.");
                                return;
                            }
                            await hubContext.Clients.Client(connectionId)
                                .SendAsync("ReceiveMessage", $"You decide to go through the door to the {direction}");

                            var adventurer = await adventurerService.GetAdventurer(session.Adventurer.Id);
                            var newRoom = await roomService.GetRoom(Convert.ToInt32(adventurer.RoomId));
                            newRoom.EventCompleted = adventurer.IsRoomCompleted(newRoom.Id);
                            sessionManager.UpdateSessionRoom(connectionId, newRoom);

                            await hubContext.Clients.Client(connectionId)
                                .SendAsync("UpdateEnemy", new
                                {
                                    difficulty = 1,
                                    name = "Enemy",
                                    weapon = "Weapon",
                                    health = 0,
                                });
                            await hubContext.Clients.Client(connectionId)
                                .SendAsync("UpdateRoomsExplored", adventurer.AdventurerMaps.Count);

                            await hubContext.Clients.Client(connectionId)
                                .SendAsync("ReceiveMessage", $"You look around and see {session.Room}");
                            return;
                        default:
                            await hubContext.Clients.Client(connectionId)
                                .SendAsync("ReceiveMessage", $"Could not read direction did you spell the command right? \n Direction read: {message.Replace("go", "")}");
                            return;
                    }
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
                    await GetEnemy(connectionId);
                    await hubContext.Clients.Client(connectionId)
                                .SendAsync("ReceiveMessage", $"You take a moment to observe the weird looking creature in the room...\n It seems to be some kind of a {session.Enemy.Name}");
                    return;
                case "attack":
                case "fight":
                    await GetEnemy(connectionId);
                    session.State = States.Fighting;
                    await hubContext.Clients.Client(connectionId)
                               .SendAsync("ReceiveMessage", $"You decide to attack the {session.Enemy.Name}");
                    await HandleFightingCommands(connectionId, "attack");
                    return;
                case "rest":
                    int healing = (int)(3 * session.Adventurer.Experience / 10);
                    session.Adventurer.Health += healing;
                    await adventurerService.SetHealth(session.Adventurer.Id, session.Adventurer.Health);
                    await hubContext.Clients.Client(connectionId)
                            .SendAsync("ReceiveMessage", $"You decide to rest here and heal your wounds (+{healing} hp)");
                    await hubContext.Clients.Client(connectionId)
                        .SendAsync("UpdateHealth", session.Adventurer.Health);

                    await roomService.CompleteRoom(session.Adventurer.Id);
                    session.Room.EventCompleted = true;
                    return;
                //response when no keywords are found
                default:
                    await hubContext.Clients.Client(connectionId)
                        .SendAsync("ReceiveMessage", $"Given command not found, did you misspell it? \n Command given: {message}, use help to see all available commands");
                    return;
            }
        }

        public async Task HandleFightingCommands(string connectionId, string message)
        {
            var session = sessionManager.GetSession(connectionId);
            //list of direction keywords to look out for
            List<string> commands = GetCombatCommands();
            switch (commands.FirstOrDefault<string>(s => message.Contains(s)))
            {
                case "help":
                    await hubContext.Clients.Client(connectionId)
                        .SendAsync("ReceiveMessage", "Available commands: " + string.Join(", ", commands));
                    return;
                case "test":
                    await hubContext.Clients.Client(connectionId)
                        .SendAsync("ReceiveMessage", "Test message");
                    return;
                case "run":
                    bool success = await combatService.Run(connectionId);
                    if (success)
                    {
                        Random rngDirection = new Random();
                        Array possibleDirections = typeof(Directions).GetEnumValues();
                        var direction = possibleDirections.GetValue(rngDirection.Next(possibleDirections.Length)).ToString().ToLower();

                        await hubContext.Clients.Client(connectionId)
                            .SendAsync("ReceiveMessage", $"You dodged out of the way of the {session.Enemy.Name}'s attack and run in a random direction");


                        if (!await roomService.MoveTo(session.Adventurer.Id, direction))
                        {
                            //if there is no wall in this direction
                            await hubContext.Clients.Client(connectionId)
                                .SendAsync("ReceiveMessage", $" in your panic you decided to run {direction} and hit the wall... ouch");
                            return;
                        }
                        await hubContext.Clients.Client(connectionId)
                            .SendAsync("ReceiveMessage", $" you ran to the {direction} and closed the door behind you just in time");

                        var adventurer = await adventurerService.GetAdventurer(session.Adventurer.Id);
                        var newRoom = await roomService.GetRoom(Convert.ToInt32(adventurer.RoomId));
                        newRoom.EventCompleted = adventurer.IsRoomCompleted(newRoom.Id);
                        sessionManager.UpdateSessionRoom(connectionId, newRoom);

                        await hubContext.Clients.Client(connectionId)
                            .SendAsync("UpdateEnemy", new
                            {
                                difficulty = 1,
                                name = "Enemy",
                                weapon = "Weapon",
                                health = 0,
                            });
                        await hubContext.Clients.Client(connectionId)
                            .SendAsync("UpdateRoomsExplored", adventurer.AdventurerMaps.Count);
                    }
                    else
                    {
                        await hubContext.Clients.Client(connectionId)
                            .SendAsync("ReceiveMessage", $"You tried to run but the {session.Enemy.Name} noticed this and cut you off " +
                            $"\n the {session.Enemy.Name} hit you with his {session.Enemy.Weapon.Name} (-{session.Enemy.Weapon?.Attack ?? 0} hp)");

                        await hubContext.Clients.Client(connectionId)
                            .SendAsync("UpdateHealth", session.Adventurer.Health);
                        await IsPlayerDead(connectionId);
                    }
                    return;
                case "attack":
                    Random rng = new Random();
                    if (rng.Next(10*session.Enemy.Difficulty, 100) < 65)
                    {
                        //player attacks
                        await combatService.PlayerAttack(connectionId);
                        await hubContext.Clients.Client(connectionId)
                        .SendAsync("ReceiveMessage", $"You attack the {session.Enemy.Name} hurting it (-{session.Adventurer.Damage} hp)");
                        await hubContext.Clients.Client(connectionId)
                            .SendAsync("UpdateEnemyHealth", session.Enemy.Health);
                        await IsEnemyDead(connectionId);
                    }
                    else
                    {
                        //enemy attacks
                        await combatService.EnemyAttack(connectionId);
                        await hubContext.Clients.Client(connectionId)
                        .SendAsync("ReceiveMessage", $"The {session.Enemy.Name} saw your attack coming and countered it with his {session.Enemy.Weapon.Name} (-{session.Enemy.Weapon.Attack})");
                        await hubContext.Clients.Client(connectionId)
                            .SendAsync("UpdateHealth", session.Adventurer.Health);
                        await IsPlayerDead(connectionId);
                    }
                    return;
                //response when no keywords are found
                default:
                    await hubContext.Clients.Client(connectionId)
                        .SendAsync("ReceiveMessage", $"Given command not found, did you misspell it? \n Command given: {message}, use help to see all available commands");
                    return;
            }
        }

        private async Task GetEnemy(string connectionId)
        {
            var session = sessionManager.GetSession(connectionId);

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
        }

        private async Task IsPlayerDead(string connectionId)
        {
            var session = sessionManager.GetSession(connectionId);
            if (session.Adventurer.Health < 1)
            {
                await hubContext.Clients.Client(connectionId)
                        .SendAsync("ReceiveMessage", $"During your brutal battle you become fatally wounded and pass out. \n Your adventure ends here {session.Adventurer.Name}.");
                session.State = States.Dead;
            }
        }

        private async Task IsEnemyDead(string connectionId)
        {
            var session = sessionManager.GetSession(connectionId);
            if (session.Enemy.Health < 1)
            {
                Random rng = new Random();
                int experience = rng.Next(1 * (session.Enemy.Difficulty*2), 15);
                session.Adventurer.Experience += experience;
                await adventurerService.SetExperience(session.Adventurer.Id, session.Adventurer.Experience);
                await hubContext.Clients.Client(connectionId)
                        .SendAsync("ReceiveMessage", $"During your fight with the {session.Enemy.Name} you land a fatal blow and come out victorious (+{experience} exp) ");
                await hubContext.Clients.Client(connectionId)
                        .SendAsync("UpdateExperience", session.Adventurer.Experience);
                await roomService.CompleteRoom(session.Adventurer.Id);
                session.Room.EventCompleted = true;
                session.State = States.Exploring;
            }
        }
    }
}
