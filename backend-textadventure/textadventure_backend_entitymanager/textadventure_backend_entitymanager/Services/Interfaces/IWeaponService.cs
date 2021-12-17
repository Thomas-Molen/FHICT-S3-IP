using System.Collections.Generic;
using System.Threading.Tasks;
using textadventure_backend_entitymanager.Models.Entities;

namespace textadventure_backend_entitymanager.Services.Interfaces
{
    public interface IWeaponService
    {
        Task EquipWeapon(int adventurerId, int weaponId);
        Task<Weapons> GenerateWeapon(int adventurerId);
        Task<List<Weapons>> GetAllWeapons(int adventurerId);
    }
}