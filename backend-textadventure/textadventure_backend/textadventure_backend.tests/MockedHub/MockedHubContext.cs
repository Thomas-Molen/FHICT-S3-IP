using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Text;
using textadventure_backend.Hubs;

namespace textadventure_backend.tests.MockedHub
{
    class MockedHubContext : IHubContext<GameHub>
    {
        public IHubClients Clients => throw new NotImplementedException();

        public IGroupManager Groups => throw new NotImplementedException();
    }
}
