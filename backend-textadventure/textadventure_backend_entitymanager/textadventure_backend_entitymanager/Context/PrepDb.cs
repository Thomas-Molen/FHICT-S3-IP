using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace textadventure_backend_entitymanager.Context
{
    public static class PrepDb
    {
        public static void ApplyMigrations(IApplicationBuilder applicationBuilder)
        {
            using var serviceScope = applicationBuilder.ApplicationServices.CreateScope();

            var contextFactory = serviceScope.ServiceProvider.GetService<IDbContextFactory<TextadventureDBContext>>();

            using var db = contextFactory.CreateDbContext();

                db.Database.Migrate();

        }
    }
}
