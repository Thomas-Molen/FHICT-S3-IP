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

namespace textadventure_backend_entitymanager.tests
{
    public class AdventurerServiceTests
    {
        IDbContextFactory<TextadventureDBContext> contextFactory;
        TextadventureDBContext context;

        AdventurerService sut;
        //Users user;
        public AdventurerServiceTests()
        {
            contextFactory = new TestDbContextFactory();
            context = contextFactory.CreateDbContext();

            sut = new AdventurerService(contextFactory);

            //user = new Users("email", "username", "password");
            //context.Add(user);
            //context.SaveChanges();
        }

        [Fact]
        public async Task CreatingAdventurerWithNoValidUserIdThrowsArgumentException()
        {
            
        }

        [Fact]
        public async Task CanCreateAnAdventurerFromUser()
        {

        }

        [Fact]
        public async Task CreatingAnAdventurerWithoutDungeonWillGenerateDungeon()
        {

        }

        [Fact]
        public async Task CanGetAnAdventurer()
        {

        }

        [Fact]
        public async Task CanGetAllAdventurersFromAUser ()
        {

        }

        [Fact]
        public async Task CanGetAListOfAdventurersOrderedByExperience()
        {

        }

        [Fact]
        public async Task CanDeleteAdventurers()
        {

        }

    }
}
