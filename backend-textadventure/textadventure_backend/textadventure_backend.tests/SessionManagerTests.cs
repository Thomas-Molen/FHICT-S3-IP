using Shouldly;
using System;
using textadventure_backend.Models.Entities;
using textadventure_backend.Models.Session;
using textadventure_backend.Services;
using Xunit;

namespace textadventure_backend.tests
{
    public class SessionManagerTests
    {
        private readonly ISessionManager _sut;
        private readonly string _connectionId;
        public SessionManagerTests()
        {
            _sut = new SessionManager();

            //seed sessions
            _connectionId = "_connectionId";
            var sessionAdventurer = new SessionAdventurer
            {
                Damage = 10,
                Experience = 10,
                Health = 20,
                Id = 1,
                Name = "testingSessionAdventurer"
            };
            _sut.AddSession(_connectionId, sessionAdventurer, "group1");
        }

        [Fact]
        public void CanAddAndGetUserSessions()
        {
            //Arrange
            string connectionId = "connectionId";
            var sessionAdventurer = new SessionAdventurer
            {
                Damage = 10,
                Experience = 10,
                Health = 20,
                Id = 2,
                Name = "testingSessionAdventurer"
            };
            //Act
            _sut.AddSession(connectionId, sessionAdventurer, "group1");
            //Assert
            _sut.GetSession(connectionId).Adventurer.ShouldBeEquivalentTo(sessionAdventurer);
        }
        [Fact]
        public void CanRemoveSession()
        {
            //Arrange
            string connectionId = "connectionId";
            var sessionAdventurer = new SessionAdventurer
            {
                Damage = 10,
                Experience = 10,
                Health = 20,
                Id = 2,
                Name = "testingSessionAdventurer"
            };
            //Act
            _sut.AddSession(connectionId, sessionAdventurer, "group1");
            var addedSession = _sut.GetSession(connectionId);
            _sut.RemoveSession(connectionId);
            //Assert
            addedSession.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => _sut.GetSession(connectionId));
        }

        [Fact]
        public void CanSetSessionRoom()
        {
            //Arrange
            var room = new Rooms
            {
                Id = 1,
                Event = "testingEvent",
            };

            var sessionRoomThatWillBeConvertedTo = new SessionRoom
            {
                Id = room.Id,
                Event = room.Event,
                EventCompleted = room.EventCompleted,
                NorthInteraction = room.NorthInteraction,
                EastInteraction = room.EastInteraction,
                SouthInteraction = room.SouthInteraction,
                WestInteraction = room.WestInteraction
            };
            //Act
            _sut.UpdateSessionRoom(_connectionId, room);
            var session = _sut.GetSession(_connectionId);
            //Assert
            session.Room.ShouldBeEquivalentTo(sessionRoomThatWillBeConvertedTo);
        }
    }
}
