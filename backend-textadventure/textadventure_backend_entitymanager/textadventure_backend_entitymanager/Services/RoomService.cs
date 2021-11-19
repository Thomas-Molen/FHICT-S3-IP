using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<Rooms> GenerateRoom(int adventurerId, string direction = null, bool isSpawn = false)
        {
            using (var db = contextFactory.CreateDbContext())
            {
                var adventurer = await db.Adventurers
                    .OrderByDescending(x => x.Id)
                    .Include(a => a.Room)
                    .FirstOrDefaultAsync(a => a.Id == adventurerId);

                var dungeon = await db.Dungeons
                    .OrderByDescending(x => x.Id)
                    .Include(d => d.Adventurers)
                    .Include(d => d.Rooms)
                    .FirstOrDefaultAsync(d => d.Id == adventurer.DungeonId);

                var newRoom = new Rooms
                {
                    DungeonId = dungeon.Id,
                };

                if (isSpawn)
                {
                    int posX = 0;
                    int posY = 0;
                    while (posX == 0 || dungeon.Rooms.Any(r => r.PositionX == posX))
                    {
                        posX = rng.Next(1, 10 * dungeon.Rooms.Count + 1);
                    }
                    while (posY == 0 || dungeon.Rooms.Any(r => r.PositionY == posY))
                    {
                        posY = rng.Next(1, 10 * dungeon.Rooms.Count + 1);
                    }
                    newRoom.PositionX = posX;
                    newRoom.PositionY = posY;
                    newRoom.Event = Events.Chest.ToString();
                    newRoom = SetDirectionalInteractions(await CheckAdjacentRooms(newRoom), newRoom, true);
                }
                else
                {
                    int posX = adventurer.Room.PositionX;
                    int posY = adventurer.Room.PositionY;

                    if (direction == null)
                    {
                        throw new ArgumentException("No direction given");
                    }

                    switch (direction)
                    {
                        case "north":
                            posY = adventurer.Room.PositionY - 1;
                            break;
                        case "east":
                            posX = adventurer.Room.PositionX + 1;
                            break;
                        case "south":
                            posY = adventurer.Room.PositionY + 1;
                            break;
                        case "west":
                            posX = adventurer.Room.PositionX - 1;
                            break;
                        default:
                            posX = adventurer.Room.PositionX + 1;
                            break;
                    }

                    newRoom.PositionX = posX;
                    newRoom.PositionY = posY;

                    Array possbileEvents = typeof(Events).GetEnumValues();
                    newRoom.Event = possbileEvents.GetValue(rng.Next(possbileEvents.Length)).ToString();
                    newRoom = SetDirectionalInteractions(await CheckAdjacentRooms(newRoom), newRoom);
                }

                await db.AddAsync(newRoom);
                await db.SaveChangesAsync();
                await AddRoomToPlayer(adventurerId, newRoom.Id);
                return newRoom;
            }
        }

        private async Task<ICollection<AdjacentRooms>> CheckAdjacentRooms(Rooms room)
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
                            .FirstOrDefaultAsync(r => (r.PositionX == room.PositionX + offsetX) && (r.PositionY == room.PositionY));
                        offsetX = -1;
                    }
                    else
                    {
                        roomBeingChecked = await db.Rooms
                            .OrderByDescending(x => x.Id)
                            .FirstOrDefaultAsync(r => (r.PositionY == room.PositionY + offsetY) && (r.PositionX == room.PositionX));
                        offsetY = -1;
                    }
                    
                    AdjacentRooms adjacentRoom = new AdjacentRooms { Room = roomBeingChecked, RelativePosition = (Directions)i };
                    adjacentRooms.Add(adjacentRoom);
                }
                return adjacentRooms;
            }
        }

        private Rooms SetDirectionalInteractions(ICollection<AdjacentRooms> adjacentRooms, Rooms room, bool isSpawn = false)
        {
            foreach (var entry in adjacentRooms)
            {
                //generate what interaction given roomside should get
                DirectionalInteractions interaction = DirectionalInteractions.Wall;
                if (isSpawn)
                {
                    if (DoesAdjacentRoomHaveDoor(entry, room) || (entry.Room == null))
                    {
                        interaction = DirectionalInteractions.Door;
                    }
                    else if (entry.Room != null)
                    {
                        interaction = DirectionalInteractions.WeakWall;
                    }
                }
                else
                {
                    
                    if (DoesAdjacentRoomHaveDoor(entry, room) || (entry.Room == null && rng.Next(1, 4) > 1))
                    {
                        interaction = DirectionalInteractions.Door;
                    }
                    else if (entry.Room != null)
                    {
                        interaction = DirectionalInteractions.WeakWall;
                    }
                }

                //Set wall's interactions depending on which direction the adjacent room is
                switch (entry.RelativePosition)
                {
                    case Directions.North:
                        room.NorthInteraction = interaction.ToString();
                        break;
                    case Directions.East:
                        room.EastInteraction = interaction.ToString();
                        break;
                    case Directions.South:
                        room.SouthInteraction = interaction.ToString();
                        break;
                    case Directions.West:
                        room.WestInteraction = interaction.ToString();
                        break;
                    default:
                        break;
                }
            }
            return room;
        }

        private bool DoesAdjacentRoomHaveDoor(AdjacentRooms adjacentRoom, Rooms room)
        {
            if (adjacentRoom.Room == null)
            {
                return false;
            }
            switch (adjacentRoom.RelativePosition)
            {
                //this might be able to be done easier
                case Directions.North:
                    if (adjacentRoom.Room.SouthInteraction == DirectionalInteractions.Door.ToString())
                    {
                        return true;
                    }
                    break;
                case Directions.East:
                    if (adjacentRoom.Room.WestInteraction == DirectionalInteractions.Door.ToString())
                    {
                        return true;
                    }
                    break;
                case Directions.South:
                    if (adjacentRoom.Room.NorthInteraction == DirectionalInteractions.Door.ToString())
                    {
                        return true;
                    }
                    break;
                case Directions.West:
                    if (adjacentRoom.Room.EastInteraction == DirectionalInteractions.Door.ToString())
                    {
                        return true;
                    }
                    break;
                default:
                    break;
            }
            return false;
        }

        private async Task AddRoomToPlayer(int adventurerId, int roomId)
        {
            using (var db = contextFactory.CreateDbContext())
            {
                var adventurer = await db.Adventurers
                    .OrderByDescending(x => x.Id)
                    .FirstOrDefaultAsync(a => a.Id == adventurerId);
                adventurer.RoomId = roomId;
                AdventurerMaps adventurerMap = new AdventurerMaps
                {
                    RoomId = roomId
                };
                adventurer.AdventurerMaps.Add(adventurerMap);

                db.Update(adventurer);
                await db.SaveChangesAsync();
            }
        }
    }
}
