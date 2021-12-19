using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using Shouldly;
using System.Collections.Generic;
using System.Threading.Tasks;
using textadventure_backend_entitymanager.Context;
using textadventure_backend_entitymanager.Controllers;
using textadventure_backend_entitymanager.Helpers;
using textadventure_backend_entitymanager.Models.Entities;
using textadventure_backend_entitymanager.Models.Requests;
using textadventure_backend_entitymanager.Services;
using textadventure_backend_entitymanager.tests.Helpers;
using textadventure_backend_entitymanager.tests.Helpers.Models;
using Xunit;

namespace textadventure_backend_entitymanager.tests
{
    public class RoomControllerTests
    {
        IDbContextFactory<TextadventureDBContext> _contextFactory;
        TextadventureDBContext _context;

        RoomController _sut;
        string _accessToken;
        BaseDbSeederResponse _db;
        public RoomControllerTests()
        {
            _contextFactory = new TestDbContextFactory();
            _context = _contextFactory.CreateDbContext();
            var seeder = new TestDbSeeder(_contextFactory);

            RoomService roomService = new RoomService(_contextFactory);
            _accessToken = "AccessToken";
            var appSettings = Options.Create(new AppSettings { GameAccessToken = _accessToken });
            AccessTokenHelper accessTokenHelper = new AccessTokenHelper(appSettings);

            _sut = new RoomController(roomService, accessTokenHelper);

            _db = seeder.Seed();
        }

        [Fact]
        public async Task CanGetARoom()
        {
            //Arrange
            var room = new Rooms
            {
                DungeonId = _db.dungeon.Id,
                Event = "chest",
                PositionX = 1,
                PositionY = 1
            };
            _context.Add(room);
            _context.SaveChanges();
            //Act
            var result = await _sut.GetRoom(room.Id, _accessToken);
            var roomResult = (ObjectResult)result;
            var retreivedRoom = (Rooms)roomResult.Value;
            //Assert
            retreivedRoom.ShouldBeEquivalentTo(room);
        }

        [Fact]
        public async Task CanNotGetRoomWithoutAccessToken()
        {
            //Arrange
            //Act
            var result = await _sut.GetRoom(1, "Wrong AccessToken");
            var weaponsResult = (ObjectResult)result;
            //Assert
            weaponsResult.ShouldBeOfType<UnauthorizedObjectResult>();
        }

        [Fact]
        public async Task GetRoomWithNoValidRoomWillThrowException()
        {
            //Arrange
            //Act
            var result = await _sut.GetRoom(0, _accessToken);
            var weapon = (ObjectResult)result;
            //Assert
            weapon.ShouldBeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task CanCreateASpawnRoom()
        {
            //Arrange
            var adventurer = await _context.Adventurers.FirstOrDefaultAsync(a => a.Id == _db.adventurer.Id);
            //Act
            await _sut.CreateSpawn(_accessToken, _db.adventurer.Id);
            //Assert
            adventurer.RoomId.ShouldBe(null);
            _context.Entry(adventurer).Reload();
            adventurer.RoomId.ShouldNotBe(null);
        }

        [Fact]
        public async Task CanNotCreateSpawnRoomWithoutAccessToken()
        {
            //Arrange
            //Act
            var result = await _sut.CreateSpawn("Wrong AccessToken", 1);
            var weaponsResult = (ObjectResult)result;
            //Assert
            weaponsResult.ShouldBeOfType<UnauthorizedObjectResult>();
        }

        [Fact]
        public async Task CreateSpawnRoomWithNoValidAdventurerWillThrowException()
        {
            //Arrange
            //Act
            var result = await _sut.CreateSpawn(_accessToken, 0);
            var weapon = (ObjectResult)result;
            //Assert
            weapon.ShouldBeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task CanCompleteRoom()
        {
            //Arrange
            var room = new Rooms
            {
                DungeonId = _db.dungeon.Id,
                Event = "chest",
                PositionX = 1,
                PositionY = 1
            };
            _context.Add(room);
            _context.SaveChanges();

            var mapEntry = new AdventurerMaps
            {
                AdventurerId = _db.adventurer.Id,
                RoomId = room.Id
            };
            _db.adventurer.RoomId = room.Id;
            _context.Update(_db.adventurer);
            _context.Add(mapEntry);
            _context.SaveChanges();
            //Act
            await _sut.CompleteRoom(_accessToken, _db.adventurer.Id);
            //Assert
            mapEntry.EventCompleted.ShouldBeFalse();
            _context.Entry(mapEntry).Reload();
            mapEntry.EventCompleted.ShouldBeTrue();
        }

        [Fact]
        public async Task CanNotCompleteRoomWithoutAccessToken()
        {
            //Arrange
            //Act
            var result = await _sut.CompleteRoom("Wrong AccessToken", 1);
            var weaponsResult = (ObjectResult)result;
            //Assert
            weaponsResult.ShouldBeOfType<UnauthorizedObjectResult>();
        }

        [Fact]
        public async Task CompleteRoomWithNoValidAdventurerWillThrowException()
        {
            //Arrange
            //Act
            var result = await _sut.CompleteRoom(_accessToken, 0);
            var weapon = (ObjectResult)result;
            //Assert
            weapon.ShouldBeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task CompleteRoomWithNoValidRoomToAdventurerConnectionWillThrowException()
        {
            //Arrange
            var room = new Rooms
            {
                DungeonId = _db.dungeon.Id,
                Event = "chest",
                PositionX = 1,
                PositionY = 1
            };
            _context.Add(room);
            _context.SaveChanges();

            _db.adventurer.RoomId = room.Id;
            _context.Update(_db.adventurer);
            _context.SaveChanges();
            //Act
            var result = await _sut.CompleteRoom(_accessToken, _db.adventurer.Id);
            var weapon = (ObjectResult)result;
            //Assert
            weapon.ShouldBeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task CanMoveToNewRoom()
        {
            //Arrange
            var room = new Rooms
            {
                DungeonId = _db.dungeon.Id,
                Event = "chest",
                PositionX = 1,
                PositionY = 1,
                SouthInteraction = "Door"
            };
            _context.Add(room);
            _context.SaveChanges();

            var mapEntry = new AdventurerMaps
            {
                AdventurerId = _db.adventurer.Id,
                RoomId = room.Id
            };
            _db.adventurer.RoomId = room.Id;
            _context.Update(_db.adventurer);
            _context.Add(mapEntry);
            _context.SaveChanges();

            var enterRoomRequest = new EnterRoomRequest
            {
                AdventurerId = _db.adventurer.Id,
                Direction = "south"
            };
            //Act
            await _sut.EnterRoom(_accessToken, enterRoomRequest);
            var adventurer = await _context.Adventurers.FirstOrDefaultAsync(a => a.Id == _db.adventurer.Id);
            //Assert
            _context.Entry(adventurer).Reload();
            adventurer.RoomId.ShouldNotBe(room.Id);
        }

        [Fact]
        public async Task MovingToANewRoomReturnsTrue()
        {
            //Arrange
            var room = new Rooms
            {
                DungeonId = _db.dungeon.Id,
                Event = "chest",
                PositionX = 1,
                PositionY = 1,
                SouthInteraction = "Door"
            };
            _context.Add(room);
            _context.SaveChanges();

            var mapEntry = new AdventurerMaps
            {
                AdventurerId = _db.adventurer.Id,
                RoomId = room.Id
            };
            _db.adventurer.RoomId = room.Id;
            _context.Update(_db.adventurer);
            _context.Add(mapEntry);
            _context.SaveChanges();

            var enterRoomRequest = new EnterRoomRequest
            {
                AdventurerId = _db.adventurer.Id,
                Direction = "south"
            };
            //Act
            var result = await _sut.EnterRoom(_accessToken, enterRoomRequest);
            var roomResult = (ObjectResult)result;
            var enterRoomResult = (bool)roomResult.Value;
            //Assert
            enterRoomResult.ShouldBeTrue();
        }

        [Fact]
        public async Task NotMovingToANewRoomReturnsFalse()
        {
            //Arrange
            var room = new Rooms
            {
                DungeonId = _db.dungeon.Id,
                Event = "chest",
                PositionX = 1,
                PositionY = 1,
                SouthInteraction = "Wall"
            };
            _context.Add(room);
            _context.SaveChanges();

            var mapEntry = new AdventurerMaps
            {
                AdventurerId = _db.adventurer.Id,
                RoomId = room.Id
            };
            _db.adventurer.RoomId = room.Id;
            _context.Update(_db.adventurer);
            _context.Add(mapEntry);
            _context.SaveChanges();

            var enterRoomRequest = new EnterRoomRequest
            {
                AdventurerId = _db.adventurer.Id,
                Direction = "south"
            };
            //Act
            var result = await _sut.EnterRoom(_accessToken, enterRoomRequest);
            var roomResult = (ObjectResult)result;
            var enterRoomResult = (bool)roomResult.Value;
            //Assert
            enterRoomResult.ShouldBeFalse();
        }

        [Fact]
        public async Task CanNotMoveToRoomWithoutAccessToken()
        {
            //Arrange
            var enterRoomRequest = new EnterRoomRequest
            {
                AdventurerId = _db.adventurer.Id,
                Direction = "south"
            };
            //Act
            var result = await _sut.EnterRoom("Wrong AccessToken", enterRoomRequest);
            var weaponsResult = (ObjectResult)result;
            //Assert
            weaponsResult.ShouldBeOfType<UnauthorizedObjectResult>();
        }

        [Fact]
        public async Task MoveToRoomWithNoValidAdventurerWillThrowException()
        {
            //Arrange
            var enterRoomRequest = new EnterRoomRequest
            {
                AdventurerId = 0,
                Direction = "south"
            };
            //Act
            var result = await _sut.EnterRoom(_accessToken, enterRoomRequest);
            var weapon = (ObjectResult)result;
            //Assert
            weapon.ShouldBeOfType<BadRequestObjectResult>();
        }
    }
}
