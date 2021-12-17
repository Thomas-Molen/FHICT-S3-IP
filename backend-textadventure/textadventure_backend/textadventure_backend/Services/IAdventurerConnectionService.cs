using System.Threading.Tasks;
using textadventure_backend.Models.Entities;

namespace textadventure_backend.Services
{
    public interface IAdventurerConnectionService
    {
        Task<Adventurers> GetAdventurer(int adventurerId);
        Task SetExperience(int adventurerId, int experience);
        Task SetHealth(int adventurerId, int health);
    }
}