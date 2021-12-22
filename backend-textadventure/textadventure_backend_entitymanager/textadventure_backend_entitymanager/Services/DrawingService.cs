using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using textadventure_backend_entitymanager.Context;
using textadventure_backend_entitymanager.Models.Responses;

namespace textadventure_backend_entitymanager.Services
{
    public interface IDrawingService
    {
        Task<GetDrawingResponse> GetDrawing(int adventurerId);
        Task SaveDrawing(int adventurerId, string drawing);
    }

    public class DrawingService : IDrawingService
    {
        private readonly IDbContextFactory<TextadventureDBContext> contextFactory;

        public DrawingService(IDbContextFactory<TextadventureDBContext> _contextFactory)
        {
            contextFactory = _contextFactory;
        }

        public async Task SaveDrawing(int adventurerId, string drawing)
        {
            using (var db = contextFactory.CreateDbContext())
            {
                var adventurer = await db.Adventurers.OrderByDescending(x => x.Id).FirstOrDefaultAsync(a => a.Id == adventurerId);

                if (adventurer == null)
                {
                    throw new ArgumentException("No adventurer found with given id");
                }

                adventurer.Drawing = drawing;

                db.Update(adventurer);
                await db.SaveChangesAsync();
            }
        }

        public async Task<GetDrawingResponse> GetDrawing(int adventurerId)
        {
            using (var db = contextFactory.CreateDbContext())
            {
                var adventurer = await db.Adventurers.OrderByDescending(x => x.Id).FirstOrDefaultAsync(a => a.Id == adventurerId);

                if (adventurer == null)
                {
                    throw new ArgumentException("No adventurer found with given id");
                }

                return new GetDrawingResponse { drawing = adventurer.Drawing };
            }
        }
    }
}
