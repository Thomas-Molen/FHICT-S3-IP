using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using textadventure_backend_entitymanager.Context;

namespace textadventure_backend_entitymanager.tests.Helpers
{
    class TestDbContextFactory : IDbContextFactory<TextadventureDBContext>
    {
        private DbContextOptions<TextadventureDBContext> _options;

        public TestDbContextFactory()
        {
            _options = new DbContextOptionsBuilder<TextadventureDBContext>()
                .UseInMemoryDatabase("InMemoryDB")
                .Options;
        }

        public TextadventureDBContext CreateDbContext()
        {
            return new TextadventureDBContext(_options);
        }
    }
}
