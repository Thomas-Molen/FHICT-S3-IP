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
        private readonly string _botUser;
        public GameHub()
        {
            _botUser = "MyChat Bot";
        }

        public async Task JoinGame(StartGameConnection startGameConnection)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, startGameConnection.Room);

            await Clients.Group(startGameConnection.Room).SendAsync("ReceiveMessage", _botUser, $"{startGameConnection.User} has started game in room {startGameConnection.Room}");
        }
    }
}
