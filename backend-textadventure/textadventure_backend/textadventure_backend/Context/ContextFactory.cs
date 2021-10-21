using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace textadventure_backend.Context
{
    public class ContextFactory : IDesignTimeDbContextFactory<TextadventureDBContext>, IContextFactory
    {
        private readonly string connectionString;
        public ContextFactory()
        {
            string path = Directory.GetCurrentDirectory();

            IConfigurationBuilder builder =
                new ConfigurationBuilder()
                    .SetBasePath(path)
                    .AddJsonFile("appsettings.json");

            IConfigurationRoot config = builder.Build();

            connectionString = config.GetConnectionString("SQL_DB");

        }
        public ContextFactory(string connectionString)
        {
            this.connectionString = connectionString;

            var options = new DbContextOptionsBuilder<TextadventureDBContext>();
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }

        public TextadventureDBContext CreateDbContext(string[] args = null)
        {
            var options = new DbContextOptionsBuilder<TextadventureDBContext>();
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

            return new TextadventureDBContext(options.Options);
        }
    }
}
