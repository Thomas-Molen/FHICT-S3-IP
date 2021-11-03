using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using textadventure_backend.Models;

namespace textadventure_backend.Services.Interfaces
{
    public interface IAdventurerService
    {
        Task Create(string name, int userId);
        Task<ICollection<Adventurers>> Get(int userId);
        Task Delete(int userId, int adventurerId);
    }
}
