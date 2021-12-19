using System.Threading.Tasks;

namespace textadventure_backend.Services.Interfaces
{
    public interface IGameplayService
    {
        Task AddPlayer(string connectionId, int adventurerId, int userId);
        Task EquipWeapon(string connectionId, int weaponId);
        Task ExecuteCommand(string message, string connectionId);
        Task RemovePlayer(string connectionId);
    }
}