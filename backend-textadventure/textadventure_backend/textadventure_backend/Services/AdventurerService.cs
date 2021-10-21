using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using textadventure_backend.Context;
using textadventure_backend.Models;
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

        public async Task<ICollection<Adventurers>> Create(int userId)
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

                var adventurer = new Adventurers
                {
                    Dungeon = dungeon
                };
                var user = db.Users.Include(u => u.Adventurers).FirstOrDefault(u => u.Id == userId);
                user.Adventurers.Add(adventurer);
                db.Update(user);
                await db.SaveChangesAsync();

                return user.Adventurers.ToList();
            }
        }

        public async Task<ICollection<Adventurers>> Get(int userId)
        {
            using (var db = contextFactory.CreateDbContext())
            {
                var user = db.Users.Include(u => u.Adventurers).FirstOrDefault(u => u.Id == userId);
                return user.Adventurers.ToList();
            }
        }
    }
}
