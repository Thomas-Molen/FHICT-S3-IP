using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using textadventure_backend.Context;
using textadventure_backend.Models;
using textadventure_backend.Models.Responses;
using textadventure_backend.Services.Interfaces;

namespace textadventure_backend.Services
{
    public class AdventurerService : IAdventurerService
    {
        private readonly IContextFactory contextFactory;
        public AdventurerService(IContextFactory _contextFactory)
        {
            contextFactory = _contextFactory;
        }

        public async Task Create(string name, int userId)
        {
            using (var db = contextFactory.CreateDbContext())
            {
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
            }
        }

        public async Task Delete(int userId, int adventurerId)
        {
            using (var db = contextFactory.CreateDbContext())
            {
                var user = await db.Users.Include(u => u.Adventurers.Where(a => a.Id == adventurerId)).FirstOrDefaultAsync(u => u.Id == userId);
                var adventurer = user.Adventurers.FirstOrDefault();

                if (adventurer == null)
                {
                    throw new ArgumentException("Given adventurer not connected to current user");
                }

                db.Remove(adventurer);
                await db.SaveChangesAsync();
            }
        }

        public async Task<ICollection<GetAdventurersResponse>> Get(int userId)
        {
            using (var db = contextFactory.CreateDbContext())
            {
                var user = await db.Users.Include(u => u.Adventurers).ThenInclude(a => a.Weapons.Where(w => w.Equiped)).FirstOrDefaultAsync(u => u.Id == userId);

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
                        Level = (int)(adventurer.Experience / 100),
                        Name = adventurer.Name,
                        Health = adventurer.Health,
                        Damage = damage
                    };
                    result.Add(adventurerToAdd);
                }

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
                        Level = (int)(adventurer.Experience / 100),
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
