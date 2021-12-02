using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using textadventure_backend.Models;
using textadventure_backend.Models.Entities;
using textadventure_backend.Models.Session;

namespace textadventure_backend.Services
{
    public class SessionManager
    {
        private List<Session> sessions;

        public SessionManager(AdventurerService _adventurerService, WeaponService _weaponService, RoomService _roomService)
        {
            sessions = new List<Session>();
        }

        public void AddSession(string connectionId, SessionAdventurer adventurer, string group)
        {
            Session sessionToAdd = new Session
            {
                Adventurer = adventurer,
                ConnectionId = connectionId,
                Group = group
            };
            sessions.Add(sessionToAdd);
        }

        public void RemoveSession(string connectionId)
        {
            var sessionToRemove = GetSessionFromConnectionId(connectionId);
            sessions.Remove(sessionToRemove);
        }

        public Session GetSession(string connectionId)
        {
            var sessionToGet = GetSessionFromConnectionId(connectionId);
            return sessionToGet;
        }

        public void UpdateSessionRoom(string connectionId, Rooms room)
        {
            var sessionToUpdate = GetSessionFromConnectionId(connectionId);
            sessionToUpdate.Room = new SessionRoom
            {
                Id = room.Id,
                Event = room.Event,
                EventCompleted = room.EventCompleted,
                NorthInteraction = room.NorthInteraction,
                EastInteraction = room.EastInteraction,
                SouthInteraction = room.SouthInteraction,
                WestInteraction = room.WestInteraction
            };
        }
        public void UpdateSessionRoom(string connectionId, SessionRoom room)
        {
            var sessionToUpdate = GetSessionFromConnectionId(connectionId);
            sessionToUpdate.Room = room;
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
