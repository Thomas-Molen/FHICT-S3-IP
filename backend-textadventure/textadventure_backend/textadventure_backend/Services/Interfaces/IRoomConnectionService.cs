using System.Threading.Tasks;
using textadventure_backend.Models.Entities;

namespace textadventure_backend.Services.Interfaces
{
    public interface IRoomConnectionService
    {
        Task CompleteRoom(int adventurerId);
        Task CreateSpawn(int adventurerId);
        Task<Rooms> GetRoom(int roomId);
        Task<bool> MoveTo(int adventurerId, string direction);
    }
}