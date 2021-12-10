using Microsoft.EntityFrameworkCore;
using System.Linq;
using textadventure_backend_entitymanager.Context;
using textadventure_backend_entitymanager.Models.Entities;
using textadventure_backend_entitymanager.tests.Helpers;
using Xunit;
using Shouldly;

namespace textadventure_backend_entitymanager.tests
{
    public class TestDbContextFactoryTests
    {
        IDbContextFactory<TextadventureDBContext> contextFactory;
        TextadventureDBContext context;
        public TestDbContextFactoryTests()
        {
            contextFactory = new TestDbContextFactory();
            context = contextFactory.CreateDbContext();
        }

        [Fact]
        public void DatabaseCanBeConnectedTo()
        {
            context.Database.CanConnect().ShouldBeTrue();
        }

        [Fact]
        public void CanStoreDataIntoDatabase()
        {
            //Arrange
            Dungeons dungeon = new Dungeons{};
            //Act
            context.Dungeons.Add(dungeon);
            context.SaveChanges();
            //Assert
            context.Dungeons.Count().ShouldBe(1);
        }
    }
}
