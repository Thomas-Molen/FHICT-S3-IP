using Shouldly;
using System;
using textadventure_backend_entitymanager.Models.Entities;
using textadventure_backend_entitymanager.Services;
using Xunit;

namespace textadventure_backend_entitymanager.tests
{
    public class EnemyServiceTests
    {
        private readonly EnemyService sut;

        public EnemyServiceTests()
        {
            sut = new EnemyService();
        }

        [Fact]
        public void GenerateEnemyReturnsEnemyObject()
        {
            //Arrange
            int experience = 0;
            //Act
            var enemy = sut.GenerateEnemy(experience);
            //Assert
            enemy.ShouldBeOfType<Enemy>();
        }

        [Fact]
        public void GenerateEnemyReturnsAMinimalHealthOf3()
        {
            //Arrange
            int experience = 0;
            //Act
            var enemy = sut.GenerateEnemy(experience);
            //Assert
            enemy.Health.ShouldBeGreaterThanOrEqualTo(3);
        }
    }
}
