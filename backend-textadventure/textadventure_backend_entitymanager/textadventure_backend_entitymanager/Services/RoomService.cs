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

        public async Task<Rooms> CreateSpawnRoom(int adventurerId)
        {
            using (var db = contextFactory.CreateDbContext())
            {
                var dungeon = await db.Dungeons
                    .OrderByDescending(x => x.Id)
                    .Include(d => d.Adventurers)
                    .Include(d => d.Rooms)
                    .FirstOrDefaultAsync(d => d.Id == adventurerId);

                //there must be a better way to do this
                int posX = 0;
                int posY = 0;
                while (posX == 0 || dungeon.Rooms.Any(r => r.PositionX == posX))
                {
                    posX = rng.Next(1, 10 * dungeon.Rooms.Count+1);
                }
                while (posY == 0 || dungeon.Rooms.Any(r => r.PositionY == posY))
                {
                    posY = rng.Next(1, 10 * dungeon.Rooms.Count+1);
                }

                var spawnRoom = new Rooms
                {
                    PositionX = posX,
                    PositionY = posY,
                    Event = Events.Chest.ToString(),
                    DungeonId = dungeon.Id,
                };
                spawnRoom = await CheckAdjacentRooms(spawnRoom);

                await db.AddAsync(spawnRoom);
                await db.SaveChangesAsync();
                await AddRoom(adventurerId, spawnRoom.Id);
                return spawnRoom;
            }
        }

        private async Task<Rooms> CheckAdjacentRooms(Rooms room)
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
                return SetDirectionalInteractions(adjacentRooms, room);
            }
        }

        private Rooms SetDirectionalInteractions(ICollection<AdjacentRooms> adjacentRooms, Rooms room)
        {
            foreach (var entry in adjacentRooms)
            {
                DirectionalInteractions interaction = DirectionalInteractions.Wall;
                if (DoesAdjacentRoomHaveDoor(entry, room) || (entry.Room == null && rng.Next(1, 4) > 1))
                {
                    interaction = DirectionalInteractions.Door;
                }
                else if (entry.Room != null)
                {
                    interaction = DirectionalInteractions.WeakWall;
                }

                //this might be able to be done easier
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

        private async Task AddRoom(int adventurerId, int roomId)
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
