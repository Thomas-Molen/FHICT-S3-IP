using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using textadventure_backend.Models.Entities;
using textadventure_backend.Models.Requests;

namespace textadventure_backend.Services.Interfaces
{
    public interface IGameService
    {
        Task<EnterRoomRequest> EnterRoom(Adventurers adventurer, string direction);
        Task<LoadRoomRequest> LoadRoom(int adventurerId);
        Task<EnterRoomRequest> GenerateSpawn(int adventurerId);
        Task<OpenChestRequest> OpenChest(int adventurerId);
        Task EquipWeapon(int adventurerId, int weaponId);
        string[] GetCommands(string Event, bool EventCompleted);
        string[] GetCombatCommands();
    }
}
