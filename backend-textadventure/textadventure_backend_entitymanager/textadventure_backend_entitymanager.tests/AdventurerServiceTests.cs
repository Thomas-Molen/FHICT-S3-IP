using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using textadventure_backend_entitymanager.Context;
using textadventure_backend_entitymanager.Models.Entities;
using textadventure_backend_entitymanager.Services;
using Xunit;
using Shouldly;
using textadventure_backend_entitymanager.tests.Helpers;
using System.Linq;

namespace textadventure_backend_entitymanager.tests
{
    public class AdventurerServiceTests
    {
        IDbContextFactory<TextadventureDBContext> _contextFactory;
        TextadventureDBContext _context;

        AdventurerService _sut;
        Users _user;
        public AdventurerServiceTests()
        {
            _contextFactory = new TestDbContextFactory();
            _context = _contextFactory.CreateDbContext();

            _sut = new AdventurerService(_contextFactory);

            _user = new Users("testingEmail", "testingUsername", "testingPassword");
            _context.Add(_user);
            _context.SaveChanges();
        }

        [Fact]
        public async Task CreatingAdventurerWithNoValidUserIdThrowsArgumentException()
        {
            //Arrange
            //Act
            //Assert
            await _sut.Create("FailedAdventurer", _user.Id + 1).ShouldThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task CanCreateAnAdventurerFromUser()
        {
            //Arrange
            Users userWithAdventurer;
            //Act
            await _sut.Create("SucessfullyAddedAdventurer", _user.Id);
            userWithAdventurer = await _context.Users.Include(a => a.Adventurers).FirstOrDefaultAsync(a => a.Id == _user.Id);
            //Assert
            userWithAdventurer.Adventurers.Count.ShouldBe(1);
        }

        [Fact]
        public async Task CreatingAnAdventurerWithoutNameWillGiveDefaultName()
        {
            //Arrange
            //Act
            var newAdventurer = await _sut.Create("", _user.Id);
            //Assert
            newAdventurer.Name.ShouldBeEquivalentTo("Adventurer");
        }

        [Fact]
        public async Task CreatingAnAdventurerWithoutDungeonWillGenerateDungeon()
        {
            //Arrange
            var contextFactory = new TestDbContextFactory();
            var context = contextFactory.CreateDbContext();
            var sut = new AdventurerService(contextFactory);

            var user = new Users("tempEmail", "tempUsername", "tempPassword");
            context.Add(user);
            context.SaveChanges();
            int originalDungeonCount = context.Dungeons.ToList().Count;
            //Act
            await sut.Create("AdventurerWithNewDungeon", user.Id);
            int newDungeonCount = context.Dungeons.ToList().Count;
            //Assert
            newDungeonCount.ShouldBeGreaterThan<int>(originalDungeonCount);
        }

        [Fact]
        public async Task CanGetAnAdventurer()
        {
            //Arrange
            var newAdventurer = await _sut.Create("SucessfullyAddedAdventurer", _user.Id);
            //Act
            var adventurerToGet = await _sut.Get(newAdventurer.Id, _user.Id);
            //Assert
            adventurerToGet.Name.ShouldBeEquivalentTo(newAdventurer.Name);
        }

        [Fact]
        public async Task CanNotGetOtherUsersAdventurer()
        {
            //Arrange
            var contextFactory = new TestDbContextFactory();
            var context = contextFactory.CreateDbContext();
            var sut = new AdventurerService(contextFactory);

            var user = new Users("tempEmail", "tempUsername", "tempPassword");
            var user2 = new Users("tempEmail2", "tempUsername2", "tempPassword2");
            var dungeon = new Dungeons();
            context.Add(user);
            context.Add(user2);
            context.Add(dungeon);
            context.SaveChanges();

            var adventurer = new Adventurers
            {
                Name = "AdventurerThatCanNotBeAccessed",
                UserId = user.Id,
                DungeonId = dungeon.Id
            };
            context.Add(adventurer);
            context.SaveChanges();
            //Act
            //Assert
            await sut.Get(adventurer.Id, user2.Id).ShouldThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task CanGetAllAdventurersFromAUser()
        {
            //Arrange
            await _sut.Create("", _user.Id);
            await _sut.Create("", _user.Id);
            //Act
            var adventurers = await _sut.GetAllFromUser(_user.Id);
            //Assert
            adventurers.Count.ShouldBe(2);
        }

        [Fact]
        public async Task CanGetAListOfAdventurersOrderedByLevel()
        {
            //Arrange
            var contextFactory = new TestDbContextFactory();
            var context = contextFactory.CreateDbContext();
            var sut = new AdventurerService(contextFactory);

            var user = new Users("tempEmail", "tempUsername", "tempPassword");
            context.Add(user);
            var dungeon = new Dungeons();
            context.Add(dungeon);

            context.SaveChanges();

            var HighExperienceAdventurer = new Adventurers
            {
                Name = "HighExperienceAdventurer",
                UserId = user.Id,
                DungeonId = dungeon.Id,
                Experience = 100
            };
            var MediumExperienceAdventurer = new Adventurers
            {
                Name = "MediumExperienceAdventurer",
                UserId = user.Id,
                DungeonId = dungeon.Id,
                Experience = 50
            };
            var LowExperienceAdventurer = new Adventurers
            {
                Name = "LowExperienceAdventurer",
                UserId = user.Id,
                DungeonId = dungeon.Id,
                Experience = 10
            };
            context.Add(HighExperienceAdventurer);
            context.Add(MediumExperienceAdventurer);
            context.Add(LowExperienceAdventurer);
            context.SaveChanges();

            //Act
            var OrderedUsers = await sut.GetLeaderboard();
            //Assert
            OrderedUsers.ElementAt(0).Level.ShouldBeGreaterThan<int>(OrderedUsers.ElementAt(1).Level);
            OrderedUsers.ElementAt(1).Level.ShouldBeGreaterThan<int>(OrderedUsers.ElementAt(2).Level);
        }

        [Fact]
        public async Task CanDeleteAdventurer()
        {
            //Arrange
            var newAdventurer = await _sut.Create("SucessfullyAddedAdventurer", _user.Id);
            int originalAmountOfAdventurers = _context.Adventurers.ToList().Count;
            //Act
            await _sut.Delete(_user.Id, newAdventurer.Id);
            int newAmountOfAdventurers = _context.Adventurers.ToList().Count;
            //Assert
            (originalAmountOfAdventurers - newAmountOfAdventurers).ShouldBe(1);
        }

        [Fact]
        public async Task CanNotDeleteAdventurerFromOtherPerson()
        {
            //Arrange
            var user = new Users("tempEmail", "tempUsername", "tempPassword");
            _context.Add(user);
            _context.SaveChanges();

            await _sut.Create("", user.Id);
            var newAdventurer = await _sut.Create("AdventurerThatCanNotBeDeletedByOtherUser", _user.Id);
            //Act
            //Assert
            await _sut.Delete(user.Id, newAdventurer.Id).ShouldThrowAsync<ArgumentException>();
        }

    }
}
