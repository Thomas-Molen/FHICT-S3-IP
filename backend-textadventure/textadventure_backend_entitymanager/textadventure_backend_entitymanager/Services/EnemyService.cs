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
    public class EnemyService
    {
        private readonly IDbContextFactory<TextadventureDBContext> contextFactory;

        public EnemyService(IDbContextFactory<TextadventureDBContext> _contextFactory)
        {
            contextFactory = _contextFactory;
        }

        public async Task<Enemy> GenerateEnemy(int adventurerId)
        {
            using (var db = contextFactory.CreateDbContext())
            {
                var adventurer = await db.Adventurers.OrderBy(d => d.Id).FirstOrDefaultAsync(a => a.Id == adventurerId);
                if (adventurer == null)
                {
                    throw new ArgumentException("No adventurer found with given Id");
                }
                return new Enemy(adventurer.Experience);
            }
        }
    }
}
