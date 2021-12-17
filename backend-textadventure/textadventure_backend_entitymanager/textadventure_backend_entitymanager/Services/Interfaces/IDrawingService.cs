using System.Threading.Tasks;
using textadventure_backend_entitymanager.Models.Responses;

namespace textadventure_backend_entitymanager.Services.Interfaces
{
    public interface IDrawingService
    {
        Task<GetDrawingResponse> GetDrawing(int adventurerId);
        Task SaveDrawing(int adventurerId, string drawing);
    }
}
