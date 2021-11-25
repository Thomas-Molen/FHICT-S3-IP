using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using textadventure_backend_entitymanager.Models.Entities;
using textadventure_backend_entitymanager.Models.Responses;

namespace textadventure_backend_entitymanager.Services.Interfaces
{
    public interface IWeaponService
    {
        Task<OpenChestResponse> GenerateWeapon(int adventurerId);
        Task<List<Weapons>> GetAllWeapons(int adventurerId);
        Task EquipWeapon(int adventurerId, int weaponId);
    }
}
