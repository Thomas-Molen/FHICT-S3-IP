using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using textadventure_backend_entitymanager.Context;
using textadventure_backend_entitymanager.Models.Entities;
using textadventure_backend_entitymanager.tests.Helpers.Models;

namespace textadventure_backend_entitymanager.tests.Helpers
{
    class TestDbSeeder
    {
        private readonly IDbContextFactory<TextadventureDBContext> contextFactory;

        public TestDbSeeder(IDbContextFactory<TextadventureDBContext> _contextFactory)
        {
            contextFactory = _contextFactory;
        }

        public BaseDbSeederResponse Seed()
        {
            using (var db = contextFactory.CreateDbContext())
            {
                var user = new Users("testingEmail", "testingUsername", "testingPassword");
                var dungeon = new Dungeons();
                var adventurer = new Adventurers
                {
                    Name = "AdventurerThatCanNotBeAccessed",
                    UserId = user.Id,
                    DungeonId = dungeon.Id
                };
                db.Add(user);
                db.Add(dungeon);
                db.Add(adventurer);
                
                db.SaveChanges();
                return new BaseDbSeederResponse
                {
                    adventurer = adventurer,
                    dungeon = dungeon,
                    user = user
                };
            }
        }
    }
}
