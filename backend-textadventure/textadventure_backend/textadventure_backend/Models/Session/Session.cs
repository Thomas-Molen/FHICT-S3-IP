using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using textadventure_backend.Models.Entities;

namespace textadventure_backend.Models.Session
{
    public class Session
    {
        public string ConnectionId { get; set; }
        public string Group { get; set; }
        public Enemy Enemy { get; set; } = null;
        public SessionAdventurer Adventurer { get; set; }
        public SessionRoom Room { get; set; }
        public Weapons Weapon { get; set; }
    }
}
