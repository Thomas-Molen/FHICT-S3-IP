using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using textadventure_backend.Models;

namespace textadventure_backend.Services.Interfaces
{
    public interface IAdventurerService
    {
        Task<ICollection<Adventurers>> Create(int userId);
        Task<ICollection<Adventurers>> Get(int userId);
    }
}
