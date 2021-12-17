using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using textadventure_backend_entitymanager.Context;
using textadventure_backend_entitymanager.Models.Entities;
using textadventure_backend_entitymanager.Services.Interfaces;

namespace textadventure_backend_entitymanager.Services
{
    public class HubAdventurerService : IHubAdventurerService
    {
        private readonly IDbContextFactory<TextadventureDBContext> contextFactory;

        public HubAdventurerService(IDbContextFactory<TextadventureDBContext> _contextFactory)
        {
            contextFactory = _contextFactory;
        }

        public async Task<Adventurers> Get(int adventurerId)
        {
            using (var db = contextFactory.CreateDbContext())
            {
                var adventurer = await db.Adventurers
                    .OrderByDescending(x => x.Id)
                    .Include(a => a.AdventurerMaps)
                    .Include(a => a.Weapons
                        .Where(w => w.Durability > 0))
                    .Include(a => a.Room)
                    .FirstOrDefaultAsync(a => a.Id == adventurerId);
                if (adventurer == null)
                {
                    throw new ArgumentException("No adventurer found with given Id");
                }
                return adventurer;
            }
        }

        public async Task SetHealth(int adventurerId, int health)
        {
            using (var db = contextFactory.CreateDbContext())
            {
                var adventurer = await db.Adventurers
                    .OrderByDescending(x => x.Id)
                    .FirstOrDefaultAsync(a => a.Id == adventurerId);

                if (adventurer == null)
                {
                    throw new ArgumentException("No adventurer found with given Id");
                }

                if (health < 0)
                {
                    health = 0;
                }

                adventurer.Health = health;

                db.Update(adventurer);
                await db.SaveChangesAsync();
            }
        }

        public async Task SetExperience(int adventurerId, int experience)
        {
            using (var db = contextFactory.CreateDbContext())
            {
                var adventurer = await db.Adventurers
                    .OrderByDescending(x => x.Id)
                    .FirstOrDefaultAsync(a => a.Id == adventurerId);

                if (adventurer == null)
                {
                    throw new ArgumentException("No adventurer found with given Id");
                }

                if (experience < 0)
                {
                    experience = 0;
                }

                adventurer.Experience = experience;

                db.Update(adventurer);
                await db.SaveChangesAsync();
            }
        }
    }
}
