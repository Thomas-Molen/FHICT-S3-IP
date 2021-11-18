using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using textadventure_backend.Models;
using textadventure_backend.Models.Entities;
using textadventure_backend.Services.Interfaces;

namespace textadventure_backend.Services
{
    public class SessionManager : ISessionManager
    {
        private readonly IAdventurerService adventurerService;
        private List<Session> sessions;

        public SessionManager(IAdventurerService _adventurerService)
        {
            adventurerService = _adventurerService;
            sessions = new List<Session>();
        }

        public async Task AddSession(string connectionId, int adventurerId)
        {
            var adventurer = await adventurerService.GetAdventurer(adventurerId);
            if (adventurer == null)
            {
                throw new ArgumentException("No adventurer found with given Id");
            }

            Session sessionToAdd = new Session
            {
                Adventurer = adventurer,
                ConnectionId = connectionId,
                Group = adventurer.DungeonId.ToString()
            };
            sessions.Add(sessionToAdd);
        }

        public Session GetSession(string connectionId)
        {
            var sessionToGet = sessions.Find(s => s.ConnectionId == connectionId);
            if (sessionToGet == null)
            {
                throw new ArgumentException("No active adventurer connected to given connectionId");
            }
            return sessionToGet;
        }

        public void RemoveSession(string connectionId)
        {
            var sessionToRemove = sessions.Find(s => s.ConnectionId == connectionId);
            if (sessionToRemove == null)
            {
                throw new ArgumentException("No active adventurer connected to given connectionId");
            }
            sessions.Remove(sessionToRemove);
        }
    }
}
