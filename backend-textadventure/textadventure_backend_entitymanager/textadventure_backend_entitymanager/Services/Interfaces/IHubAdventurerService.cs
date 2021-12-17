using System.Threading.Tasks;
using textadventure_backend_entitymanager.Models.Entities;

namespace textadventure_backend_entitymanager.Services.Interfaces
{
    public interface IHubAdventurerService
    {
        Task<Adventurers> Get(int adventurerId);
        Task SetExperience(int adventurerId, int experience);
        Task SetHealth(int adventurerId, int health);
    }
}