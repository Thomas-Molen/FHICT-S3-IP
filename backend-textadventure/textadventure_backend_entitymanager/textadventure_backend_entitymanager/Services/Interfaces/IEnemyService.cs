using textadventure_backend_entitymanager.Models.Entities;

namespace textadventure_backend_entitymanager.Services.Interfaces
{
    public interface IEnemyService
    {
        Enemy GenerateEnemy(int experience);
    }
}