using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using textadventure_backend.Models;

namespace textadventure_backend.Hubs
{
    [Authorize]
    public class GameHub : Hub
    {
        public async Task JoinGame(int adventurerId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, adventurerId.ToString());
            await Clients.Group(adventurerId.ToString())
                .SendAsync("ReceiveMessage", "Connected to game as Id: " + adventurerId);
        }
    }
}
