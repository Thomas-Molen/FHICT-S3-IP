using Microsoft.AspNetCore.SignalR;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using textadventure_backend.Enums;
using textadventure_backend.Hubs;
using textadventure_backend.Models.Entities;
using textadventure_backend.Models.Session;
using textadventure_backend.Services;
using textadventure_backend.tests.MockedServices;
using Xunit;

namespace textadventure_backend.tests
{
    public class CommandServiceTests
    {
        private readonly CommandService _sut;
        private readonly Mock<IClientProxy> _mockClientProxy;

        private readonly string _connectionId;
        public CommandServiceTests()
        {
            //mocking needed dependencies
            var sessionManager = new SessionManager();
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
            sessionManager.AddSession(_connectionId, sessionAdventurer, "group1");
            sessionManager.UpdateSessionRoom(_connectionId, new Rooms
            {
                Id = 1,
                Event = Events.Chest.ToString(),
            });

            var roomConnectionService = new MockedRoomConnectionService();
            var enemyConnectionService = new MockedEnemyConnectionService();
            var adventurerConnectionService = new MockedAdventurerConnectionService();
            var weaponConnectionService = new MockedWeaponConnectionService();
            var combatService = new CombatService(sessionManager, adventurerConnectionService);

            //mock hub
            Mock<IHubClients> mockClients = new Mock<IHubClients>();
            _mockClientProxy = new Mock<IClientProxy>();
            mockClients.Setup(clients => clients.Client(_connectionId)).Returns(_mockClientProxy.Object);

            var hubContext = new Mock<IHubContext<GameHub>>();
            hubContext.Setup(x => x.Clients).Returns(() => mockClients.Object);

            var client = hubContext.Object.Clients.Client(_connectionId);

            

            _sut = new CommandService(sessionManager, hubContext.Object, roomConnectionService, weaponConnectionService, adventurerConnectionService, enemyConnectionService, combatService);
        }

        [Fact]
        public void CanGetAllCommandsFromExploredRoom()
        {
            //Arrange
            List<string> expectedCommands = new List<string> { "help", "say", "go", "look", "clear", "test" };
            //Act
            var commands = _sut.GetWorldCommands(Events.Empty.ToString(), true);
            //Assert
            commands.ShouldBeEquivalentTo(expectedCommands);
        }

        [Fact]
        public void CanGetAllCommandsFromEmptyRoom()
        {
            //Arrange
            List<string> expectedCommands = new List<string> { "help", "say", "go", "look", "clear", "test", "rest" };
            //Act
            var commands = _sut.GetWorldCommands(Events.Empty.ToString(), false);
            //Assert
            commands.ShouldBeEquivalentTo(expectedCommands);
        }

        [Fact]
        public void CanGetAllCommandsFromEnemyRoom()
        {
            //Arrange
            List<string> expectedCommands = new List<string> { "help", "say", "go", "look", "clear", "test", "fight", "attack", "observe" };
            //Act
            var commands = _sut.GetWorldCommands(Events.Enemy.ToString(), false);
            //Assert
            commands.ShouldBeEquivalentTo(expectedCommands);
        }

        [Fact]
        public void CanGetAllCommandsFromChestRoom()
        {
            //Arrange
            List<string> expectedCommands = new List<string> { "help", "say", "go", "look", "clear", "test", "open" };
            //Act
            var commands = _sut.GetWorldCommands(Events.Chest.ToString(), false);
            //Assert
            commands.ShouldBeEquivalentTo(expectedCommands);
        }

        [Fact]
        public async Task HelpCommandReturnsListOfCommands()
        {
            //Arrange
            //Act
            await _sut.HandleExploringCommands(_connectionId, "help");
            //Assert
            _mockClientProxy.Verify(
                clientProxy => clientProxy.SendCoreAsync(
                    "ReceiveMessage",
                    It.Is<object[]>(o => o != null && o.Length == 1 && (string)o[0] == "Available commands: help, say, go, look, clear, test, open"),
                    default(CancellationToken)),
                Times.Once);
        }
    }
}
