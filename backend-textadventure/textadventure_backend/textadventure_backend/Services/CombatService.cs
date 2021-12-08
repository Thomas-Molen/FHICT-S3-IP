using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace textadventure_backend.Services
{
    public class CombatService
    {
        private readonly SessionManager sessionManager;
        private readonly RoomConnectionService roomService;
        private readonly WeaponConnectionService weaponService;
        private readonly AdventurerConnectionService adventurerService;
        private readonly EnemyConnectionService enemyService;

        public CombatService(SessionManager _sessionManager, RoomConnectionService _roomService, WeaponConnectionService _weaponService, AdventurerConnectionService _adventurerService, EnemyConnectionService _enemyService)
        {
            sessionManager = _sessionManager;
            roomService = _roomService;
            weaponService = _weaponService;
            adventurerService = _adventurerService;
            enemyService = _enemyService;
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
