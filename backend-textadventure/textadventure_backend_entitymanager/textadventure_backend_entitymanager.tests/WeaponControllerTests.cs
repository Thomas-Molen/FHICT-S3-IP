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
    public class WeaponControllerTests
    {
        IDbContextFactory<TextadventureDBContext> _contextFactory;
        TextadventureDBContext _context;

        WeaponController _sut;
        string _accessToken;
        BaseDbSeederResponse _db;
        public WeaponControllerTests()
        {
            _contextFactory = new TestDbContextFactory();
            _context = _contextFactory.CreateDbContext();
            var seeder = new TestDbSeeder(_contextFactory);

            WeaponService weaponService = new WeaponService(_contextFactory);
            _accessToken = "AccessToken";
            var appSettings = Options.Create(new AppSettings { GameAccessToken = _accessToken });
            AccessTokenHelper accessTokenHelper = new AccessTokenHelper(appSettings);

            _sut = new WeaponController(weaponService, accessTokenHelper);

            _db = seeder.Seed();
        }

        [Fact]
        public async Task CanGenerateAWeapon()
        {
            //Arrange
            //Act
            var result = await _sut.GenerateWeapon(_accessToken, _db.adventurer.Id);
            var weapon = (ObjectResult)result;
            //Assert
            weapon.Value.ShouldBeOfType<Weapons>();
        }

        [Fact]
        public async Task CanNotGenerateAWeaponWithoutAccessToken()
        {
            //Arrange
            //Act
            var result = await _sut.GenerateWeapon("Wrong AccessToken", _db.adventurer.Id);
            var weapon = (ObjectResult)result;
            //Assert
            weapon.ShouldBeOfType<UnauthorizedObjectResult>();
        }

        [Fact]
        public async Task GenerateWeaponWithNoValidAdventurerWillThrowException()
        {
            //Arrange
            //Act
            var result = await _sut.GenerateWeapon(_accessToken, 0);
            var weapon = (ObjectResult)result;
            //Assert
            weapon.ShouldBeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task CanGetAllWeaponsFromAdventurer()
        {
            //Arrange
            var weapon1 = new Weapons(1);
            var weapon2 = new Weapons(2);
            _db.adventurer.Weapons.Add(weapon1);
            _db.adventurer.Weapons.Add(weapon2);
            _context.Update(_db.adventurer);
            _context.SaveChanges();
            //Act
            var result = await _sut.GetAllWeapons(_accessToken, _db.adventurer.Id);
            var weaponsResult = (ObjectResult)result;
            var weapons = (List<Weapons>)weaponsResult.Value;
            //Assert
            weapons.Count.ShouldBe(2);
        }

        [Fact]
        public async Task CanNotGetAllWeaponsFromAdventurerWithoutAccessToken()
        {
            //Arrange
            //Act
            var result = await _sut.GetAllWeapons("Wrong AccessToken", _db.adventurer.Id);
            var weaponsResult = (ObjectResult)result;
            //Assert
            weaponsResult.ShouldBeOfType<UnauthorizedObjectResult>();
        }

        [Fact]
        public async Task GetAllWeaponsWithNoValidAdventurerWillThrowException()
        {
            //Arrange
            //Act
            var result = await _sut.GetAllWeapons(_accessToken, 0);
            var weapon = (ObjectResult)result;
            //Assert
            weapon.ShouldBeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task CanEquipWeaponFromAdventurer()
        {
            //Arrange
            var weapon = new Weapons(1);
            _db.adventurer.Weapons.Add(weapon);
            _context.Update(_db.adventurer);
            _context.SaveChanges();
            var originalWeaponEquipState = weapon.Equiped;
            var equipWeaponRequest = new EquipWeaponRequest { AdventurerId = _db.adventurer.Id, WeaponId = weapon.Id };
            //Act
            await _sut.EquipWeapon(_accessToken, equipWeaponRequest);
            var result = await _sut.GetAllWeapons(_accessToken, _db.adventurer.Id);
            var weaponsResult = (ObjectResult)result;
            var weapons = (List<Weapons>)weaponsResult.Value;
            var newWeaponEquipState = weapons.Find(w => w.Id == weapon.Id).Equiped;
            //Assert
            originalWeaponEquipState.ShouldBeFalse();
            newWeaponEquipState.ShouldBeTrue();
        }

        [Fact]
        public async Task EquipingWeaponWillUnequipOldWeapon()
        {
            //Arrange
            var oldWeapon = new Weapons(1);
            var newWeapon = new Weapons(1);
            _db.adventurer.Weapons.Add(oldWeapon);
            _db.adventurer.Weapons.Add(newWeapon);
            _context.Update(_db.adventurer);
            _context.SaveChanges();
            var firstEquipWeaponRequest = new EquipWeaponRequest { AdventurerId = _db.adventurer.Id, WeaponId = oldWeapon.Id };
            var secondEquipWeaponRequest = new EquipWeaponRequest { AdventurerId = _db.adventurer.Id, WeaponId = newWeapon.Id };

            //Act
            await _sut.EquipWeapon(_accessToken, firstEquipWeaponRequest);
            var equipOldWeaponresult = await _sut.GetAllWeapons(_accessToken, _db.adventurer.Id);
            var equipOldWeaponWeaponsResult = (ObjectResult)equipOldWeaponresult;
            var equipOldWeaponWeapons = (List<Weapons>)equipOldWeaponWeaponsResult.Value;
            var oldWeaponOriginalEquipState = equipOldWeaponWeapons.Find(w => w.Id == oldWeapon.Id).Equiped;
            var newWeaponOriginalEquipState = newWeapon.Equiped;

            await _sut.EquipWeapon(_accessToken, secondEquipWeaponRequest);
            var equipNewWeaponresult = await _sut.GetAllWeapons(_accessToken, _db.adventurer.Id);
            var equipNewWeaponsWeaponsResult = (ObjectResult)equipNewWeaponresult;
            var equipNewWeaponWeapons = (List<Weapons>)equipNewWeaponsWeaponsResult.Value;
            var newWeaponEquipState = equipNewWeaponWeapons.Find(w => w.Id == newWeapon.Id).Equiped;
            var oldWeaponEquipState = equipNewWeaponWeapons.Find(w => w.Id == oldWeapon.Id).Equiped;

            //Assert
            oldWeaponOriginalEquipState.ShouldBeTrue();
            oldWeaponEquipState.ShouldBeFalse();
            newWeaponOriginalEquipState.ShouldBeFalse();
            newWeaponEquipState.ShouldBeTrue();
        }

        [Fact]
        public async Task CanNotEquipWeaponFromAdventurerWithoutAccessToken()
        {
            //Arrange
            var equipWeaponRequest = new EquipWeaponRequest { AdventurerId = _db.adventurer.Id, WeaponId = 0 };
            //Act
            var result = await _sut.EquipWeapon("Wrong AccessToken", equipWeaponRequest);
            var equipResult = (ObjectResult)result;
            //Assert
            equipResult.ShouldBeOfType<UnauthorizedObjectResult>();
        }

        [Fact]
        public async Task EquipWeaponWithNoValidAdventurerWillThrowException()
        {
            //Arrange
            var equipWeaponRequest = new EquipWeaponRequest { AdventurerId = 0, WeaponId = 0 };
            //Act
            var result = await _sut.EquipWeapon(_accessToken, equipWeaponRequest);
            var weapon = (ObjectResult)result;
            //Assert
            weapon.ShouldBeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task EquipWeaponWithNoValidWeaponWillThrowException()
        {
            //Arrange
            var equipWeaponRequest = new EquipWeaponRequest { AdventurerId = _db.adventurer.Id, WeaponId = 0 };
            //Act
            var result = await _sut.EquipWeapon(_accessToken, equipWeaponRequest);
            var weapon = (ObjectResult)result;
            //Assert
            weapon.ShouldBeOfType<BadRequestObjectResult>();
        }
    }
}
