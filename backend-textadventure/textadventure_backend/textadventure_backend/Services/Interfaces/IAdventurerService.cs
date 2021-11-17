using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using textadventure_backend.Models.Entities;

namespace textadventure_backend.Services.Interfaces
{
    public interface IAdventurerService
    {
        Task<Adventurers> GetAdventurer(int adventurerId);
    }
}
