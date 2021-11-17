using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using textadventure_backend_entitymanager.Models;
using textadventure_backend_entitymanager.Models.Responses;

namespace textadventure_backend_entitymanager.Services.Interfaces
{
    public interface IAdventurerService
    {
        Task Create(string name, int userId);
        Task<ICollection<GetAdventurersResponse>> Get(int userId);
        Task<GetAdventurerResponse> Get(int userId, int adventurerId);
        Task Delete(int userId, int adventurerId);
        Task<ICollection<LeaderboardResponse>> GetLeaderboard();
        Task<Adventurers> Find(int adventurerId);
    }
}
