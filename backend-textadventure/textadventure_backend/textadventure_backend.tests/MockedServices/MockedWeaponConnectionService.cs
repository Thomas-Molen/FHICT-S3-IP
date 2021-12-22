using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using textadventure_backend.Models.Entities;
using textadventure_backend.Services.ConnectionServices;

namespace textadventure_backend.tests.MockedServices
{
    class MockedWeaponConnectionService : IWeaponConnectionService
    {
        public MockedWeaponConnectionService()
        {
        }

        public Task<Weapons> CreateWeapon(int adventurerId)
        {
            return Task.Run(() => new Weapons
            {
                AdventurerId = 1,
                Attack = 10,
                Durability = 100,
                Equiped = false,
                Id = 1,
                Name = "mockedTestingWeapon"
            });
        }

        public Task<List<Weapons>> GetWeapons(int adventurerId)
        {
            return Task.Run(() => new List<Weapons>
            {
                new Weapons
                {
                    AdventurerId = 1,
                    Attack = 10,
                    Durability = 100,
                    Equiped = false,
                    Id = 1,
                    Name = "mockedTestingWeapon"
                }
            });
        }

        public Task SetWeapon(int adventurerId, int weaponId)
        {
            return Task.Run(() => { });
        }
    }
}
