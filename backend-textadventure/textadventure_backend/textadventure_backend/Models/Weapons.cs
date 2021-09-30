using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace textadventure_backend.Models
{
    public class Weapons : DefaultModel
    {
        public string Name { get; set; }
        public int Attack { get; set; }
        public int Durability { get; set; } = 100;
        public int AdventurerId { get; set; }

        public virtual Adventurers Adventurer { get; set; }
        public virtual ICollection<NPCs> NPCs { get; set; }

        Weapons()
        {
            NPCs = new HashSet<NPCs>();
        }

        Weapons(string name, int attack)
        {
            NPCs = new HashSet<NPCs>();

            Name = name;
            Attack = attack;
        }
    }
}
