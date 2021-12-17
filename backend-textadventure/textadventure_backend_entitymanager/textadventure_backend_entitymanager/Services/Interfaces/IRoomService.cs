using System.Threading.Tasks;
using textadventure_backend_entitymanager.Models.Entities;

namespace textadventure_backend_entitymanager.Services.Interfaces
{
    public interface IRoomService
    {
        Task CompleteRoom(int adventurerId);
        Task CreateSpawn(int adventurerId);
        Task<Rooms> Find(int roomId);
        Task<bool> MoveToRoom(int adventurerId, string direction);
    }
}