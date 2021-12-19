using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
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
    public class UserControllerTests
    {
        IDbContextFactory<TextadventureDBContext> _contextFactory;
        TextadventureDBContext _context;

        TestingUserController _sut;
        BaseDbSeederResponse _db;
        public UserControllerTests()
        {
            _contextFactory = new TestDbContextFactory();
            _context = _contextFactory.CreateDbContext();
            var seeder = new TestDbSeeder(_contextFactory);

            var appSettings = Options.Create(new AppSettings { Secret="thisisaverylongsecretbecausecreatingjwttokensneedatleastacertainlengthofsecret" });
            JWTHelper jwtHelper = new JWTHelper(_contextFactory, appSettings);
            UserService userService = new UserService(_contextFactory, jwtHelper);

            _sut = new TestingUserController(userService);

            _db = seeder.Seed();
        }

        [Fact]
        public async Task CanRegisterNewAccount()
        {
            //Arrange
            string email = "someEmail";
            var registerRequest = new RegisterRequest
            {
                email = email,
                password = "somepassword",
                username = "someUsername"
            };
            //Act
            var result = await _sut.Register(registerRequest);
            var userResult = (ObjectResult)result;
            //Assert
            userResult.ShouldBeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task RegisteringAccountHashesPassword()
        {
            //Arrange
            string password = "password";
            string email = "anotherEmail";
            var registerRequest = new RegisterRequest
            {
                email = email,
                password = password,
                username = "someUsername"
            };
            //Act
            var result = await _sut.Register(registerRequest);
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            //Assert
            BCrypt.Net.BCrypt.Verify(password, user.Password).ShouldBeTrue();
        }

        [Fact]
        public async Task CanNotRegisterAccountWithAllreadyUsedEmail()
        {
            //Arrange
            string email = "anotherEmail";
            var registerRequest = new RegisterRequest
            {
                email = email,
                password = "password",
                username = "someUsername"
            };
            await _sut.Register(registerRequest);
            //Act
            var result = await _sut.Register(registerRequest);
            var user = (ObjectResult)result;
            //Assert
            user.ShouldBeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task CanLogin()
        {
            //Arrange
            string email = "email";
            string password = "password";
            var user = new Users
            {
                Email = email,
                Password = BCrypt.Net.BCrypt.HashPassword(password),
                Username = "username",
            };
            _context.Add(user);
            _context.SaveChanges();

            var loginRequest = new LoginRequest
            {
                email = email,
                password = password
            };
            //Act
            var result = await _sut.Login(loginRequest);
            var userResult = (ObjectResult)result;
            //Assert
            userResult.ShouldBeOfType<OkObjectResult>();
        }
        [Fact]
        public async Task CanNotLoginWithNoValidEmail()
        {
            //Arrange
            string email = "email";
            string password = "password";
            var user = new Users
            {
                Email = email,
                Password = BCrypt.Net.BCrypt.HashPassword(password),
                Username = "username",
            };
            _context.Add(user);
            _context.SaveChanges();

            var loginRequest = new LoginRequest
            {
                email = "notthecorrectEmail",
                password = password
            };
            //Act
            var result = await _sut.Login(loginRequest);
            var userResult = (ObjectResult)result;
            //Assert
            userResult.ShouldBeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task CanNotLoginWithWrongPassword()
        {
            //Arrange
            string email = "email";
            string password = "password";
            var user = new Users
            {
                Email = email,
                Password = BCrypt.Net.BCrypt.HashPassword(password),
                Username = "username",
            };
            _context.Add(user);
            _context.SaveChanges();

            var loginRequest = new LoginRequest
            {
                email = email,
                password = "WrongPassword"
            };
            //Act
            var result = await _sut.Login(loginRequest);
            var userResult = (ObjectResult)result;
            //Assert
            userResult.ShouldBeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task CanRenewAuthenticationTokensWithRefreshToken()
        {
            //Arrange
            string token = "refreshToken";
            var refreshToken = new RefreshTokens
            {
                UserId = _db.user.Id,
                ExpiresAt = DateTime.UtcNow.AddHours(2),
                Token = token
            };

            _context.Add(refreshToken);
            _context.SaveChanges();

            //Act
            var result = await _sut.RenewToken(token);
            var userResult = (ObjectResult)result;
            //Assert
            userResult.ShouldBeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task RenewingAuthenticationTokensWillDisableUsedRefreshToken()
        {
            //Arrange
            string token = "refreshToken";
            var refreshToken = new RefreshTokens
            {
                UserId = _db.user.Id,
                ExpiresAt = DateTime.UtcNow.AddHours(2),
                Token = token
            };

            _context.Add(refreshToken);
            _context.SaveChanges();

            //Act
            await _sut.RenewToken(token);
            //Assert
            _context.Entry(refreshToken).Reload();
            refreshToken.RevokedAt.ShouldNotBeNull();
        }

        [Fact]
        public async Task CanNotRenewAuthenticationTokensWithInvalidToken()
        {
            //Arrange
            string token = "refreshToken";

            //Act
            var result = await _sut.RenewToken(token);
            var userResult = (ObjectResult)result;
            //Assert
            userResult.ShouldBeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task CanNotRenewAuthenticationTokensWithNullToken()
        {
            //Arrange
            string token = null;

            //Act
            var result = await _sut.RenewToken(token);
            var userResult = (ObjectResult)result;
            //Assert
            userResult.ShouldBeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task CanNotRenewAuthenticationTokensWithExpiredToken()
        {
            //Arrange
            string token = "refreshToken";
            var refreshToken = new RefreshTokens
            {
                UserId = _db.user.Id,
                ExpiresAt = DateTime.UtcNow.AddHours(-2),
                Token = token
            };

            _context.Add(refreshToken);
            _context.SaveChanges();

            //Act
            var result = await _sut.RenewToken(token);
            var userResult = (ObjectResult)result;
            //Assert
            userResult.ShouldBeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task CanDeactivateRefreshToken()
        {
            //Arrange
            string token = "refreshToken";
            var refreshToken = new RefreshTokens
            {
                UserId = _db.user.Id,
                ExpiresAt = DateTime.UtcNow.AddHours(2),
                Token = token
            };

            _context.Add(refreshToken);
            _context.SaveChanges();

            //Act
            await _sut.DeactivateToken(token);
            //Assert
            _context.Entry(refreshToken).Reload();
            refreshToken.RevokedAt.ShouldNotBeNull();
        }

        [Fact]
        public async Task CanNotDeactivateRefreshTokenThatDoesNotExist()
        {
            //Arrange
            string token = "refreshToken";
            //Act
            var result = await _sut.DeactivateToken(token);
            var userResult = (ObjectResult)result;
            //Assert
            userResult.ShouldBeOfType<BadRequestObjectResult>();
        }
    }
}
