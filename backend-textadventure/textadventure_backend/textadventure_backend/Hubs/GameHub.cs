using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using textadventure_backend.Models;

namespace textadventure_backend.Hubs
{
    public class GameHub : Hub
    {
        public GameHub()
        {

        }

        public async Task JoinGame(StartGameConnection startGameConnection)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "room");

            await Clients.Group("room").SendAsync("ReceiveMessage", $"{startGameConnection.User} has started game");
        }
    }
}
