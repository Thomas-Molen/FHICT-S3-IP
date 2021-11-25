using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using textadventure_backend_entitymanager.Context;
using textadventure_backend_entitymanager.Enums;
using textadventure_backend_entitymanager.Models;
using textadventure_backend_entitymanager.Models.Entities;
using textadventure_backend_entitymanager.Models.Responses;
using textadventure_backend_entitymanager.Services.Interfaces;

namespace textadventure_backend_entitymanager.Services
{
    public class RoomService : IRoomService
    {
        private readonly IDbContextFactory<TextadventureDBContext> contextFactory;
        private readonly Random rng;

        public RoomService(IDbContextFactory<TextadventureDBContext> _contextFactory)
        {
            contextFactory = _contextFactory;
            rng = new Random();
        }

        public async Task<LoadRoomResponse> LoadRoom(int adventurerId)
        {
            using (var db = contextFactory.CreateDbContext())
            {
                var adventurer = await db.Adventurers
                    .OrderByDescending(x => x.Id)
                    .Include(a => a.Room)
                    .Include(a => a.AdventurerMaps)
                    .FirstOrDefaultAsync(a => a.Id == adventurerId);

                var adventureMap = adventurer.AdventurerMaps.FirstOrDefault(am => am.RoomId == adventurer.RoomId);

                var result = new LoadRoomResponse
                {
                    Message = $"You look around and see {adventurer.Room.EventToString(adventureMap.EventCompleted)}",
                    Event = adventurer.Room.Event,
                    EventCompleted = adventureMap.EventCompleted,
                    NorthInteraction = adventurer.Room.NorthInteraction,
                    EastInteraction = adventurer.Room.EastInteraction,
                    SouthInteraction = adventurer.Room.SouthInteraction,
                    WestInteraction = adventurer.Room.WestInteraction
                };
                return result;
            }
        }

        public async Task<EnterRoomResponse> MoveToRoom(int adventurerId, string direction)
        {
            using (var db = contextFactory.CreateDbContext())
            {
                var adventurer = await db.Adventurers
                    .OrderByDescending(x => x.Id)
                    .Include(a => a.Room)
                    .FirstOrDefaultAsync(a => a.Id == adventurerId);

                if (!adventurer.Room.DirectionHasDoor(direction))
                {
                    return new EnterRoomResponse
                    {
                        Message = $"You walked confidently towards the {direction} wall and... hit the wall ouch!",
                        NewRoom = false
                    };
                }

                //get new room to load
                Vector2 position = MoveInDirection(direction, new Vector2(adventurer.Room.PositionX, adventurer.Room.PositionY));
                var room = await db.Rooms
                    .OrderByDescending(x => x.Id)
                    .Where(r => r.DungeonId == adventurer.DungeonId)
                    .FirstOrDefaultAsync(r => r.PositionX == position.X && r.PositionY == position.Y);

                if (room == null)
                {
                    room = new Rooms(adventurer.DungeonId, position, await GetAdjacentRooms(position));
                    await db.AddAsync(room);
                    await db.SaveChangesAsync();
                }
                await AddRoomToPlayer(adventurer, room);

                return new EnterRoomResponse
                {
                    Message = $"You walked through the door to the {direction} and entered the room",
                    NewRoom = true
                };
            }
        }

        public async Task<EnterRoomResponse> CreateSpawn(int adventurerId)
        {
            using (var db = contextFactory.CreateDbContext())
            {
                var adventurer = await db.Adventurers
                    .OrderByDescending(x => x.Id)
                    .Include(a => a.Dungeon)
                        .ThenInclude(d => d.Rooms)
                    .FirstOrDefaultAsync(a => a.Id == adventurerId);

                //find empty position in dungeon
                Vector2 position = new Vector2(0, 0);
                while (position.X == 0 || adventurer.Dungeon.Rooms.Any(r => r.PositionX == position.X))
                {
                    position.X = rng.Next(1, 10 * adventurer.Dungeon.Rooms.Count + 1);
                }
                while (position.Y == 0 || adventurer.Dungeon.Rooms.Any(r => r.PositionY == position.Y))
                {
                    position.Y = rng.Next(1, 10 * adventurer.Dungeon.Rooms.Count + 1);
                }
                var adjacentRooms = GetAdjacentRooms(position);
                Rooms spawnRoom = new Rooms(adventurer.DungeonId, position, await GetAdjacentRooms(position), Events.Chest);

                await db.AddAsync(spawnRoom);
                await db.SaveChangesAsync();
                await AddRoomToPlayer(adventurer, spawnRoom);

                var result = new EnterRoomResponse
                {
                    Message = $"You wake up in a dark room and slightly moist room, \n In your hand you see a dog tag saying {adventurer.Name}\nYou stand up and see some kind of chest infront of you",

                    NewRoom = true
                };
                
                return result;
            }
        }

        public async Task CompleteRoom(int adventurerId)
        {
            using (var db = contextFactory.CreateDbContext())
            {
                var adventurer = await db.Adventurers
                    .OrderByDescending(x => x.Id)
                    .FirstOrDefaultAsync(a => a.Id == adventurerId);
                var map = await db.AdventurerMaps
                    .OrderByDescending(x => x.Id)
                    .FirstOrDefaultAsync(am => am.AdventurerId == adventurerId && am.RoomId == adventurer.RoomId);
                if (map == null)
                {
                    throw new ArgumentException("Adventurer Seems to not be connected to the room trying to be completed");
                }
                map.EventCompleted = true;
                db.Update(map);
                await db.SaveChangesAsync();
            }
        }

        private Vector2 MoveInDirection(string direction, Vector2 position)
        {
            //there must be an easier way to handle directions than with a switch
            switch (direction)
            {
                case "north":
                    position.Y = position.Y - 1;
                    break;
                case "south":
                    position.Y = position.Y + 1;
                    break;
                case "east":
                    position.X = position.X + 1;
                    break;
                case "west":
                    position.X = position.X - 1;
                    break;
                default:
                    throw new ArgumentException("No proper direction given");
            }
            return position;
        }

        private async Task<ICollection<AdjacentRooms>> GetAdjacentRooms(Vector2 position)
        {
            List<AdjacentRooms> adjacentRooms = new List<AdjacentRooms>();
            using (var db = contextFactory.CreateDbContext())
            {
                double offsetX = 1;
                double offsetY = 1;
                for (int i = 0; i < 4; i++)
                {
                    Rooms roomBeingChecked;
                    if (i < 2)
                    {
                        roomBeingChecked = await db.Rooms
                            .OrderByDescending(x => x.Id)
                            .FirstOrDefaultAsync(r => (r.PositionX == position.X + offsetX) && (r.PositionY == position.Y));
                        offsetX = -1;
                    }
                    else
                    {
                        roomBeingChecked = await db.Rooms
                            .OrderByDescending(x => x.Id)
                            .FirstOrDefaultAsync(r => (r.PositionY == position.Y + offsetY) && (r.PositionX == position.X));
                        offsetY = -1;
                    }
                    
                    AdjacentRooms adjacentRoom = new AdjacentRooms { Room = roomBeingChecked, RelativePosition = (Directions)i };
                    adjacentRooms.Add(adjacentRoom);
                }
                return adjacentRooms;
            }
        }

        private async Task AddRoomToPlayer(Adventurers adventurer, Rooms room)
        {
            using (var db = contextFactory.CreateDbContext())
            {
                adventurer.Room = room;
                db.Update(adventurer);
                await db.SaveChangesAsync();

                //logic to add room to adventurer's map if not already there
                db.Entry(adventurer).Collection(a => a.AdventurerMaps).Query().Where(am => am.RoomId == room.Id).ToList();
                if (adventurer.AdventurerMaps.Count < 1)
                {
                    AdventurerMaps adventurerMap = new AdventurerMaps
                    {
                        RoomId = room.Id
                    };
                    adventurer.AdventurerMaps.Add(adventurerMap);
                    db.Update(adventurer);
                    await db.SaveChangesAsync();
                }
            }
        }
    }
}
