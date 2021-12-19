using System.Collections.Generic;
using System.Threading.Tasks;
using textadventure_backend.Models.Entities;

namespace textadventure_backend.Services.Interfaces
{
    public interface IWeaponConnectionService
    {
        Task<Weapons> CreateWeapon(int adventurerId);
        Task<List<Weapons>> GetWeapons(int adventurerId);
        Task SetWeapon(int adventurerId, int weaponId);
    }
}