using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using textadventure_backend.Models.Entities;
using textadventure_backend.Services.ConnectionServices;

namespace textadventure_backend.tests.MockedServices
{
    class MockedEnemyConnectionService : IEnemyConnectionService
    {
        public MockedEnemyConnectionService()
        {
        }

        public Task<Enemy> CreateEnemy(int experience, int roomId)
        {
            return Task.Run(() => new Enemy
            {
                Difficulty = 1,
                Health = 20,
                Name = "testingEnemy",
                RoomId = 1,
                Weapon = new Weapons
                {
                    Name = "testingWeapon",
                    AdventurerId = 1,
                    Attack = 10,
                    Durability = 100,
                    Equiped = false,
                    Id = 1,
                }
            });
        }
    }
}
