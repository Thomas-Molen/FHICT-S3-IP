using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using textadventure_backend.Models;
using textadventure_backend.Models.Entities;

namespace textadventure_backend.Services.Interfaces
{
    public interface ISessionManager
    {
        Task AddSession(string connectionId, int adventurerId);
        Session GetSession(string connectionId);
        void RemoveSession(string connectionId);
        Task<Adventurers> GetUpdatedAdventurer(string connectionId);
        void UpdateRoom(string connectionId, SessionRoom room);
        void CompleteRoom(string connectionId);
        Task<List<Weapons>> GetUpdatedWeapons(string connectionId);
    }
}
