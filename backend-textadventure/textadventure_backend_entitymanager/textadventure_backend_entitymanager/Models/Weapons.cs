using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace textadventure_backend_entitymanager.Models
{
    public class Weapons : DefaultModel
    {
        public string Name { get; set; }
        public int Attack { get; set; }
        public int Durability { get; set; } = 100;
        public int AdventurerId { get; set; }
        public bool Equiped { get; set; } = false;

        public virtual Adventurers Adventurer { get; set; }
        public virtual ICollection<NPCs> NPCs { get; set; }

        Weapons()
        {
            NPCs = new HashSet<NPCs>();
        }
    }
}
