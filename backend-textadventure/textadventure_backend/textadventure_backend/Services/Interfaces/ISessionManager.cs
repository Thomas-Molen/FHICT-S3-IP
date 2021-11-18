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
        public Task AddSession(string connectionId, int adventurerId);
        public Session GetSession(string connectionId);
        public void RemoveSession(string connectionId);
    }
}
