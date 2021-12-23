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
        private readonly string _groupId;
        public CommandServiceTests()
        {
            //mocking needed dependencies
            var sessionManager = new SessionManager();
            //seed sessions
            _connectionId = "_connectionId";
            _groupId = "_group1";
            var sessionAdventurer = new SessionAdventurer
            {
                Damage = 10,
                Experience = 10,
                Health = 20,
                Id = 1,
                Name = "testingSessionAdventurer"
            };
            sessionManager.AddSession(_connectionId, sessionAdventurer, _groupId);
            sessionManager.UpdateSessionRoom(_connectionId, new Rooms
            {
                Id = 1,
                Event = Events.Chest.ToString(),
                SouthInteraction = DirectionalInteractions.Door.ToString()
            });

            var roomConnectionService = new MockedRoomConnectionService();
            var enemyConnectionService = new MockedEnemyConnectionService();
            var adventurerConnectionService = new MockedAdventurerConnectionService();
            var weaponConnectionService = new MockedWeaponConnectionService();
            var combatService = new CombatService(sessionManager, adventurerConnectionService);

            //mock hub
            Mock<IHubClients> mockClients = new Mock<IHubClients>();
            _mockClientProxy = new Mock<IClientProxy>();
            mockClients.SetReturnsDefault<IClientProxy>(_mockClientProxy.Object);

            var hubContext = new Mock<IHubContext<GameHub>>();
            hubContext.Setup(x => x.Clients).Returns(() => mockClients.Object);

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


        //command input testing
        [Fact]
        public async Task GiveErrorMessageIfNoValidCommandWasFound()
        {
            //Arrange
            //Act
            await _sut.HandleExploringCommands(_connectionId, "Not A Valid Command");
            //Assert
            _mockClientProxy.Verify(
                clientProxy => clientProxy.SendCoreAsync(
                    "ReceiveMessage",
                    It.Is<object[]>(o => o != null && o.Length == 1 && (string)o[0] == "Given command not found, did you misspell it? \n Command given: Not A Valid Command, use help to see all available commands"),
                    default(CancellationToken)),
                Times.Once);
        }

        //help
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

        //clear
        [Fact]
        public async Task ClearCommandSendsClearCall()
        {
            //Arrange
            //Act
            await _sut.HandleExploringCommands(_connectionId, "clear");
            //Assert
            _mockClientProxy.Verify(
                clientProxy => clientProxy.SendCoreAsync(
                    "ClearConsole",
                    It.Is<object[]>(o => o != null),
                    default(CancellationToken)),
                Times.Once);
        }

        //say
        [Fact]
        public async Task SayCommandShowsMessageSent()
        {
            //Arrange
            //Act
            await _sut.HandleExploringCommands(_connectionId, "say testing message");
            //Assert
            _mockClientProxy.Verify(
                clientProxy => clientProxy.SendCoreAsync(
                    "ReceiveMessage",
                    It.Is<object[]>(o => o != null && o.Length == 1 && (string)o[0] == "sent testing message"),
                    default(CancellationToken)),
                Times.Once);
        }

        [Fact]
        public async Task SayCommandSendsMessageToOtherPlayers()
        {
            //Arrange
            //Act
            await _sut.HandleExploringCommands(_connectionId, "say testing message");
            //Assert
            _mockClientProxy.Verify(
                clientProxy => clientProxy.SendCoreAsync(
                    "ReceiveMessage",
                    It.Is<object[]>(o => o != null && o.Length == 1 && (string)o[0] == "testingSessionAdventurer said testing message"),
                    default(CancellationToken)),
                Times.Once);
        }

        //go
        [Fact]
        public async Task goWithWrongDirectionWillGiveErrorMessage()
        {
            //Arrange
            //Act
            await _sut.HandleExploringCommands(_connectionId, "go wrongDirection");
            //Assert
            _mockClientProxy.Verify(
                clientProxy => clientProxy.SendCoreAsync(
                    "ReceiveMessage",
                    It.Is<object[]>(o => o != null && o.Length == 1 && (string)o[0] == "Could not read direction did you spell the command right? \n Direction read:  wrongDirection"),
                    default(CancellationToken)),
                Times.Once);
        }

        [Fact]
        public async Task WalkingIntoANewRoomResetsEnemyViewer()
        {
            //Arrange
            //Act
            await _sut.HandleExploringCommands(_connectionId, "go east");
            //Assert
            var invocations = _mockClientProxy.Invocations;

            _mockClientProxy.Verify(
                clientProxy => clientProxy.SendCoreAsync(
                    "UpdateEnemy",
                    //comparing o[0] returns false for some reason new { difficulty = 1, name = "Enemy", weapon = "Weapon", health = 0 }
                    It.Is<object[]>(o => o != null),
                    default(CancellationToken)),
                Times.Once);
        }

        [Fact]
        public async Task WalkingIntoANewRoomUpdatesRoomCounter()
        {
            //Arrange
            //Act
            await _sut.HandleExploringCommands(_connectionId, "go east");
            //Assert
            var invocations = _mockClientProxy.Invocations;

            _mockClientProxy.Verify(
                clientProxy => clientProxy.SendCoreAsync(
                    "UpdateRoomsExplored",
                    It.Is<object[]>(o => o != null),
                    default(CancellationToken)),
                Times.Once);
        }

        [Fact]
        public async Task WalkingIntoANewRoomShowsWhatIsInTheRoom()
        {
            //Arrange
            //Act
            await _sut.HandleExploringCommands(_connectionId, "go east");
            //Assert
            var invocations = _mockClientProxy.Invocations;

            _mockClientProxy.Verify(
                clientProxy => clientProxy.SendCoreAsync(
                    "ReceiveMessage",
                    It.Is<object[]>(o => o != null && o[0].ToString().Contains("You look around and see ")),
                    default(CancellationToken)),
                Times.Once);
        }

        //look
        [Fact]
        public async Task LookingWillGiveGeneralMessage()
        {
            //Arrange
            //Act
            await _sut.HandleExploringCommands(_connectionId, "look");
            //Assert
            var invocations = _mockClientProxy.Invocations;

            _mockClientProxy.Verify(
                clientProxy => clientProxy.SendCoreAsync(
                    "ReceiveMessage",
                    It.Is<object[]>(o => o != null && o[0].ToString().Contains("You see ")),
                    default(CancellationToken)),
                Times.Once);
        }

        [Fact]
        public async Task LookingInDirectionWillShowInformationOnDirection()
        {
            //Arrange
            //Act
            await _sut.HandleExploringCommands(_connectionId, "look north");
            //Assert
            var invocations = _mockClientProxy.Invocations;

            _mockClientProxy.Verify(
                clientProxy => clientProxy.SendCoreAsync(
                    "ReceiveMessage",
                    It.Is<object[]>(o => o != null && o[0].ToString().Contains("You look to the north and see a ")),
                    default(CancellationToken)),
                Times.Once);
        }
    }
}
