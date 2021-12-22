using textadventure_backend_entitymanager.Models.Entities;

namespace textadventure_backend_entitymanager.Services
{
    public interface IEnemyService
    {
        Enemy GenerateEnemy(int experience);
    }

    public class EnemyService : IEnemyService
    {

        public EnemyService()
        {

        }

        public Enemy GenerateEnemy(int experience)
        {
            return new Enemy(experience);
        }
    }
}
