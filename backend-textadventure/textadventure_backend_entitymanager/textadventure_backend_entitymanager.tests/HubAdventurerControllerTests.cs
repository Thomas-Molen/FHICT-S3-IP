using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using Shouldly;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using textadventure_backend_entitymanager.Context;
using textadventure_backend_entitymanager.Controllers;
using textadventure_backend_entitymanager.Helpers;
using textadventure_backend_entitymanager.Models.Entities;
using textadventure_backend_entitymanager.Models.Requests;
using textadventure_backend_entitymanager.Models.Responses;
using textadventure_backend_entitymanager.Services;
using textadventure_backend_entitymanager.tests.Helpers;
using textadventure_backend_entitymanager.tests.Helpers.Models;
using Xunit;

namespace textadventure_backend_entitymanager.tests
{
    public class HubAdventurerControllerTests
    {
        IDbContextFactory<TextadventureDBContext> _contextFactory;
        TextadventureDBContext _context;

        HubAdventurerController _sut;
        string _accessToken;
        BaseDbSeederResponse _db;
        public HubAdventurerControllerTests()
        {
            _contextFactory = new TestDbContextFactory();
            _context = _contextFactory.CreateDbContext();
            var seeder = new TestDbSeeder(_contextFactory);

            HubAdventurerService adventurerService = new HubAdventurerService(_contextFactory);
            _accessToken = "AccessToken";
            var appSettings = Options.Create(new AppSettings { GameAccessToken = _accessToken });
            AccessTokenHelper accessTokenHelper = new AccessTokenHelper(appSettings);

            _sut = new HubAdventurerController(adventurerService, accessTokenHelper);
            _db = seeder.Seed();
        }

        [Fact]
        public async Task CanGetAnAdventurer()
        {
            //Arrange

            //Act
            var result = await _sut.GetAdventurer(_db.adventurer.Id, _accessToken);
            var adventurer = (ObjectResult)result;
            //Assert
            adventurer.Value.ShouldBeOfType<Adventurers>();
        }

        [Fact]
        public async Task GetAnAdventurerWithNoAdventurerWillThrowException()
        {
            //Arrange

            //Act
            var result = await _sut.GetAdventurer(0, _accessToken);
            var adventurer = (ObjectResult)result;
            //Assert
            adventurer.ShouldBeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task CanNotGetAnAdventurerWithNoAccessToken()
        {
            //Arrange

            //Act
            var result = await _sut.GetAdventurer(_db.adventurer.Id, "Wrong AccessToken");
            var adventurer = (ObjectResult)result;
            //Assert
            adventurer.ShouldBeOfType<UnauthorizedObjectResult>();
        }

        [Fact]
        public async Task CanSetHealthOfAdventurer()
        {
            //Arrange
            var adventurer = await _context.Adventurers.FirstOrDefaultAsync(a => a.Id == _db.adventurer.Id);
            int originalHealth = adventurer.Health;
            int healthChange = 1;
            //Act
            var result = await _sut.SetAdventurerHealth(_db.adventurer.Id, originalHealth+healthChange, _accessToken);

            var context = _contextFactory.CreateDbContext();
            var newAdventurer = context.Adventurers.FirstOrDefault(a => a.Id == _db.adventurer.Id);
            int newHealth = newAdventurer.Health;
            //Assert
            (newHealth - originalHealth).ShouldBe(healthChange);
        }

        [Fact]
        public async Task SettingAdventurerHealthToNegativeSetsHealthTo0()
        {
            //Arrange
            //Act
            var result = await _sut.SetAdventurerHealth(_db.adventurer.Id, -10, _accessToken);

            var context = _contextFactory.CreateDbContext();
            var newAdventurer = context.Adventurers.FirstOrDefault(a => a.Id == _db.adventurer.Id);
            int newHealth = newAdventurer.Health;
            //Assert
            newHealth.ShouldBe(0);
        }

        [Fact]
        public async Task SetAdventurerHealthWithNoAdventurerWillThrowException()
        {
            //Arrange

            //Act
            var result = await _sut.SetAdventurerHealth(0, 0, _accessToken);
            var adventurer = (ObjectResult)result;
            //Assert
            adventurer.ShouldBeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task CanNotSetAdventurerHealthWithNoAccessToken()
        {
            //Arrange

            //Act
            var result = await _sut.SetAdventurerHealth(_db.adventurer.Id, 0, "Wrong AccessToken");
            var adventurer = (ObjectResult)result;
            //Assert
            adventurer.ShouldBeOfType<UnauthorizedObjectResult>();
        }

        [Fact]
        public async Task CanSetExperienceOfAdventurer()
        {
            //Arrange
            var adventurer = await _context.Adventurers.FirstOrDefaultAsync(a => a.Id == _db.adventurer.Id);
            int originalExperience= adventurer.Experience;
            int ExperienceChange = 1;
            //Act
            var result = await _sut.SetAdventurerExperience(_db.adventurer.Id, originalExperience + ExperienceChange, _accessToken);

            var context = _contextFactory.CreateDbContext();
            var newAdventurer = context.Adventurers.FirstOrDefault(a => a.Id == _db.adventurer.Id);
            int newExperience = newAdventurer.Experience;
            //Assert
            (newExperience - originalExperience).ShouldBe(ExperienceChange);
        }

        [Fact]
        public async Task SettingAdventurerExperienceToNegativeSetsHealthTo0()
        {
            //Arrange
            //Act
            var result = await _sut.SetAdventurerExperience(_db.adventurer.Id, -10, _accessToken);

            var context = _contextFactory.CreateDbContext();
            var newAdventurer = context.Adventurers.FirstOrDefault(a => a.Id == _db.adventurer.Id);
            int newExperience = newAdventurer.Experience;
            //Assert
            newExperience.ShouldBe(0);
        }

        [Fact]
        public async Task SetAdventurerExperienceWithNoAdventurerWillThrowException()
        {
            //Arrange

            //Act
            var result = await _sut.SetAdventurerExperience(0, 0, _accessToken);
            var adventurer = (ObjectResult)result;
            //Assert
            adventurer.ShouldBeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task CanNotSetAdventurerExperienceWithNoAccessToken()
        {
            //Arrange

            //Act
            var result = await _sut.SetAdventurerExperience(_db.adventurer.Id, 0, "Wrong AccessToken");
            var adventurer = (ObjectResult)result;
            //Assert
            adventurer.ShouldBeOfType<UnauthorizedObjectResult>();
        }
    }
}
