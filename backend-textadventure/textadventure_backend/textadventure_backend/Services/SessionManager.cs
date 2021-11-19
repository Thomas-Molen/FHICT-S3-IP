using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using textadventure_backend.Helpers;
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
            var sessionToGet = GetSessionFromConnectionId(connectionId);
            return sessionToGet;
        }

        public void RemoveSession(string connectionId)
        {
            var sessionToRemove = GetSessionFromConnectionId(connectionId);
            sessions.Remove(sessionToRemove);
        }

        public async Task<Adventurers> GetUpdatedAdventurer(string connectionId)
        {
            var sessionToUpdate = GetSessionFromConnectionId(connectionId);
            var adventurer = await adventurerService.GetAdventurer(sessionToUpdate.Adventurer.Id);
            sessionToUpdate.Adventurer = adventurer;
            return adventurer;
        }

        private Session GetSessionFromConnectionId(string connectionId)
        {
            var session = sessions.Find(s => s.ConnectionId == connectionId);
            if (session == null)
            {
                throw new ArgumentException("No active adventurer connected to given connectionId");
            }
            return session;
        }
    }
}
