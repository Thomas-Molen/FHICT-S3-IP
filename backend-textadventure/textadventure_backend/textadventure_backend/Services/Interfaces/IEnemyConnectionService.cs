using System.Threading.Tasks;
using textadventure_backend.Models.Entities;

namespace textadventure_backend.Services.Interfaces
{
    public interface IEnemyConnectionService
    {
        Task<Enemy> CreateEnemy(int experience, int roomId);
    }
}