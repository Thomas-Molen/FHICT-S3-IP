using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using textadventure_backend.Enums;
using textadventure_backend.Models.Entities;
using textadventure_backend.Services.ConnectionServices;

namespace textadventure_backend.tests.MockedServices
{
    class MockedRoomConnectionService : IRoomConnectionService
    {
        public MockedRoomConnectionService()
        {
        }

        public Task CompleteRoom(int adventurerId)
        {
            return Task.Run(() => { });
        }

        public Task CreateSpawn(int adventurerId)
        {
            return Task.Run(() => { });
        }

        public Task<Rooms> GetRoom(int roomId)
        {
            return Task.Run(() => new Rooms
            {
                Id = 1,
                Event = Events.Chest.ToString()
            });
        }

        public Task<bool> MoveTo(int adventurerId, string direction)
        {
            return Task.Run(() => true);
        }
    }
}
