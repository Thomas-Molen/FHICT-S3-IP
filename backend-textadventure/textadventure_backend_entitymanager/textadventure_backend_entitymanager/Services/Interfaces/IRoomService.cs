using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using textadventure_backend_entitymanager.Models;
using textadventure_backend_entitymanager.Models.Entities;
using textadventure_backend_entitymanager.Models.Responses;

namespace textadventure_backend_entitymanager.Services.Interfaces
{
    public interface IRoomService
    {
        Task<EnterRoomResponse> MoveToRoom(int adventurerId, string direction);
        Task<LoadRoomResponse> LoadRoom(int adventurerId);
        Task<EnterRoomResponse> CreateSpawn(int adventurerId);
        Task CompleteRoom(int adventurerId);
    }
}
