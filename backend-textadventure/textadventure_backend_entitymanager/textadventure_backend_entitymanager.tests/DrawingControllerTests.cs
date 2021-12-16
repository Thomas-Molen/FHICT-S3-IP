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
using textadventure_backend_entitymanager.Models.Responses;
using textadventure_backend_entitymanager.Services;
using textadventure_backend_entitymanager.tests.Helpers;
using textadventure_backend_entitymanager.tests.Helpers.Models;
using Xunit;

namespace textadventure_backend_entitymanager.tests
{
    public class DrawingControllerTests
    {
        IDbContextFactory<TextadventureDBContext> _contextFactory;
        TextadventureDBContext _context;

        DrawingController _sut;
        BaseDbSeederResponse _db;
        public DrawingControllerTests()
        {
            _contextFactory = new TestDbContextFactory();
            _context = _contextFactory.CreateDbContext();
            var seeder = new TestDbSeeder(_contextFactory);

            DrawingService drawingService = new DrawingService(_contextFactory);

            _sut = new DrawingController(drawingService);
            _db = seeder.Seed();
        }

        [Fact]
        public async Task CanSaveADrawing()
        {
            //Arrange
            var drawing = "A Beautifull Drawing";
            //Act
            await _sut.SaveDrawing(_db.adventurer.Id, new SaveDrawingRequest { Drawing = drawing });
            var adventurer = await _context.Adventurers.FirstOrDefaultAsync(a => a.Id == _db.adventurer.Id);
            //Assert
            adventurer.Drawing.ShouldBe(drawing);
        }

        [Fact]
        public async Task SaveDrawingWithWrongAdventurerThrowsException()
        {
            //Arrange
            var drawing = "A Beautifull Drawing";
            //Act
            var result = await _sut.SaveDrawing(0, new SaveDrawingRequest { Drawing = drawing });
            var drawingResult = (ObjectResult)result;
            //Assert
            drawingResult.ShouldBeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task CanGetDrawing()
        {
            //Arrange
            var drawing = "A Normal Drawing";
            var adventurer = new Adventurers
            {
                Name = "AnAdventurerWithADrawing",
                UserId = _db.user.Id,
                DungeonId = _db.dungeon.Id,
                Drawing = drawing
            };
            _context.Add(adventurer);
            _context.SaveChanges();
            //Act
            var result = await _sut.GetDrawing(adventurer.Id);
            var drawingResult = (ObjectResult)result;
            var gottenDrawing = (GetDrawingResponse)drawingResult.Value;
            //Assert
            gottenDrawing.drawing.ShouldBe(drawing);
        }

        [Fact]
        public async Task GetDrawingWithWrongAdventurerThrowsException()
        {
            //Arrange
            //Act
            var result = await _sut.GetDrawing(0);
            var drawingResult = (ObjectResult)result;
            //Assert
            drawingResult.ShouldBeOfType<BadRequestObjectResult>();
        }
    }
}
