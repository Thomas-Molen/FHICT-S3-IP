using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using textadventure_backend.Services.Interfaces;

namespace textadventure_backend.Services
{
    public class CombatService : ICombatService
    {
        private readonly ISessionManager sessionManager;
        private readonly IAdventurerConnectionService adventurerService;

        public CombatService(ISessionManager _sessionManager, IAdventurerConnectionService _adventurerService)
        {
            sessionManager = _sessionManager;
            adventurerService = _adventurerService;
        }

        public async Task<bool> Run(string connectionId)
        {
            Random rng = new Random();
            var session = sessionManager.GetSession(connectionId);

            if (rng.Next(1, 8) > 5)
            {
                session.State = Enums.States.Exploring;
                return true;
            }
            session.Adventurer.Health -= session.Enemy.Weapon?.Attack ?? 0;

            await adventurerService.SetHealth(session.Adventurer.Id, session.Adventurer.Health);
            return false;
        }

        public async Task EnemyAttack(string connectionId)
        {
            var session = sessionManager.GetSession(connectionId);
            session.Adventurer.Health -= session.Enemy.Weapon?.Attack ?? 0;

            await adventurerService.SetHealth(session.Adventurer.Id, session.Adventurer.Health);
        }

        public Task PlayerAttack(string connectionId)
        {
            var session = sessionManager.GetSession(connectionId);
            session.Enemy.Health -= session.Adventurer.Damage;
            return Task.CompletedTask;
        }
    }
}
