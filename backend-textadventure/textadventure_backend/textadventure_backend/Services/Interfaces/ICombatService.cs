using System.Threading.Tasks;

namespace textadventure_backend.Services.Interfaces
{
    public interface ICombatService
    {
        Task EnemyAttack(string connectionId);
        Task PlayerAttack(string connectionId);
        Task<bool> Run(string connectionId);
    }
}