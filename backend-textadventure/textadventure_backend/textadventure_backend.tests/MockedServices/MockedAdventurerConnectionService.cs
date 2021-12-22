using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using textadventure_backend.Models.Entities;
using textadventure_backend.Services.ConnectionServices;

namespace textadventure_backend.tests.MockedServices
{
    class MockedAdventurerConnectionService : IAdventurerConnectionService
    {
        public readonly Adventurers _adventurer;
        public MockedAdventurerConnectionService()
        {
            _adventurer = new Adventurers
            {
                DungeonId = 1,
                Name = "testingAdventurer",
                UserId = 1,
            };
        }

        public Task<Adventurers> GetAdventurer(int adventurerId)
        {
            return Task.Run(() => _adventurer);
        }

        public Task SetExperience(int adventurerId, int experience)
        {
            return Task.Run(() => _adventurer.Experience = experience);
        }

        public Task SetHealth(int adventurerId, int health)
        {
            return Task.Run(() => _adventurer.Health = health);
        }
    }
}
