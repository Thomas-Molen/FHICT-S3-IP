using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using textadventure_backend_entitymanager.Models;
using textadventure_backend_entitymanager.Models.Entities;

namespace textadventure_backend_entitymanager.Services.Interfaces
{
    public interface IRoomService
    {
        Task<Rooms> GenerateRoom(int adventurerId, string direction = null, bool isSpawn = false);
    }
}
