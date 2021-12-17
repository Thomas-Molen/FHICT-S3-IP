using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using textadventure_backend_entitymanager.Context;
using textadventure_backend_entitymanager.Models.Entities;
using textadventure_backend_entitymanager.Models.Responses;

namespace textadventure_backend_entitymanager.Services
{
    public interface IAdventurerService
    {
        Task<Adventurers> Create(string name, int userId);
        Task Delete(int userId, int adventurerId);
        Task<GetAdventurerResponse> Get(int adventurerId, int userId);
        Task<ICollection<GetAdventurersResponse>> GetAllFromUser(int userId);
        Task<ICollection<LeaderboardResponse>> GetLeaderboard();
    }

    public class AdventurerService : IAdventurerService
    {
        private readonly IDbContextFactory<TextadventureDBContext> contextFactory;

        public AdventurerService(IDbContextFactory<TextadventureDBContext> _contextFactory)
        {
            contextFactory = _contextFactory;
        }

        public async Task<Adventurers> Create(string name, int userId)
        {
            using (var db = contextFactory.CreateDbContext())
            {
                if (db.Users.Find(userId) == null)
                {
                    throw new ArgumentException("No user found with given userId");
                }

                var dungeon = db.Dungeons.OrderBy(d => d.Id).LastOrDefault();
                if (dungeon == null)
                {
                    dungeon = new Dungeons();
                    await db.Dungeons.AddAsync(dungeon);
                    await db.SaveChangesAsync();
                }

                if (name == "" || name == null)
                {
                    name = "Adventurer";
                }

                var adventurer = new Adventurers
                {
                    Name = name,
                    Dungeon = dungeon,
                    UserId = userId
                };

                await db.AddAsync(adventurer);
                await db.SaveChangesAsync();

                return adventurer;
            }
        }

        public async Task Delete(int userId, int adventurerId)
        {
            using (var db = contextFactory.CreateDbContext())
            {
                var user = await db.Users
                    .OrderByDescending(x => x.Id)
                    .Include(u => u.Adventurers
                    .Where(a => a.Id == adventurerId))
                    .FirstOrDefaultAsync(u => u.Id == userId);
                var adventurer = user.Adventurers.FirstOrDefault();

                if (adventurer == null)
                {
                    throw new ArgumentException("Given adventurer not connected to current user");
                }

                db.Remove(adventurer);
                await db.SaveChangesAsync();
            }
        }

        public async Task<ICollection<GetAdventurersResponse>> GetAllFromUser(int userId)
        {
            using (var db = contextFactory.CreateDbContext())
            {
                var user = await db.Users
                    .OrderByDescending(u => u.Id)
                    .Include(u => u.Adventurers)
                    .ThenInclude(a => a.Weapons
                    .Where(w => w.Equiped))
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (user == null)
                {
                    throw new ArgumentException("No user found with given Id");
                }

                var result = new List<GetAdventurersResponse>();
                foreach (var adventurer in user.Adventurers)
                {
                    int damage = 0;
                    var currentWeapon = adventurer.Weapons.FirstOrDefault();
                    if (adventurer.Weapons.FirstOrDefault() != null)
                    {
                        damage = currentWeapon.Attack;
                    }
                    var adventurerToAdd = new GetAdventurersResponse
                    {
                        Id = adventurer.Id,
                        Level = (int)(adventurer.Experience / 10),
                        Name = adventurer.Name,
                        Health = adventurer.Health,
                        Damage = damage
                    };
                    result.Add(adventurerToAdd);
                }

                return result;

            }
        }

        public async Task<GetAdventurerResponse> Get(int adventurerId, int userId)
        {
            using (var db = contextFactory.CreateDbContext())
            {
                var adventurer = await db.Adventurers
                    .OrderByDescending(x => x.Id)
                    .Include(a => a.Weapons.Where(w => w.Equiped))
                    .FirstOrDefaultAsync(a => a.Id == adventurerId);

                if (adventurer == null || adventurer.UserId != userId)
                {
                    throw new ArgumentException("There is no adventurer with given Id from user");
                }

                int damage = 0;
                var currentWeapon = adventurer.Weapons.FirstOrDefault();
                if (adventurer.Weapons.FirstOrDefault() != null)
                {
                    damage = currentWeapon.Attack;
                }
                var result = new GetAdventurerResponse
                {
                    Id = adventurer.Id,
                    Damage = damage,
                    Experience = adventurer.Experience,
                    Health = adventurer.Health,
                    Name = adventurer.Name,
                    DungeonId = adventurer.DungeonId
                };

                return result;

            }
        }

        public async Task<ICollection<LeaderboardResponse>> GetLeaderboard()
        {
            using (var db = contextFactory.CreateDbContext())
            {
                var adventurers = await db.Adventurers.Include(a => a.User).Include(a => a.AdventurerMaps).Include(a => a.Weapons.Where(w => w.Equiped)).OrderByDescending(a => a.Experience).Take(25).ToListAsync();
                var result = new List<LeaderboardResponse>();
                int position = 1;

                foreach (var adventurer in adventurers)
                {
                    int damage = 0;
                    var currentWeapon = adventurer.Weapons.FirstOrDefault();
                    if (adventurer.Weapons.FirstOrDefault() != null)
                    {
                        damage = currentWeapon.Attack;
                    }
                    var leaderboardEntry = new LeaderboardResponse
                    {
                        Position = position++,
                        User = adventurer.User.Username,
                        Adventurer = adventurer.Name,
                        Level = (int)(adventurer.Experience / 10),
                        Rooms = adventurer.AdventurerMaps.Count,
                        Damage = damage,
                        Health = adventurer.Health
                    };
                    result.Add(leaderboardEntry);
                }
                return result;
            }
        }
    }
}
