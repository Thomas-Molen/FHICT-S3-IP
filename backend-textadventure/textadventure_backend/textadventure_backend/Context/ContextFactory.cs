using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace textadventure_backend.Context
{
    public class ContextFactory : IDesignTimeDbContextFactory<TextadventureDBContext>, IContextFactory
    {
        private readonly string connectionString;
        public ContextFactory(string _connectionString)
        {
            connectionString = _connectionString;

            var options = new DbContextOptionsBuilder<TextadventureDBContext>();
            options.UseSqlServer(connectionString);

            var context = new TextadventureDBContext(options.Options);

            context.Database.EnsureCreated();
        }

        public TextadventureDBContext CreateDbContext(string[] args = null)
        {
            var options = new DbContextOptionsBuilder<TextadventureDBContext>();
            options.UseSqlServer(connectionString);

            return new TextadventureDBContext(options.Options);
        }
    }
}
