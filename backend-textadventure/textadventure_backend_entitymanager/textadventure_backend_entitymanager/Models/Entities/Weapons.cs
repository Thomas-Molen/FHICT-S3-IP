using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace textadventure_backend_entitymanager.Models.Entities
{
    public class Weapons : DefaultModel
    {
        public string Name { get; set; }
        public int Attack { get; set; }
        public int Durability { get; set; } = 100;
        public int AdventurerId { get; set; } = 0;
        public bool Equiped { get; set; } = false;

        public virtual Adventurers Adventurer { get; set; }
        //public virtual ICollection<NPCs> NPCs { get; set; }

        Weapons()
        {
            //NPCs = new HashSet<NPCs>();
        }

        public Weapons(int exp)
        {
            Random rng = new Random();
            
            string[] prefixs = new string[] { "Weak", "Damaged", "Reinforced", "Magical", "Wicked", "Warped", "Devine", "Fiery", "Gleaming", "Massive", "Small", "Frail", "Dirty", "Shiny" };
            string[] weapons = new string[] { "Dagger", "Sword", "Katana", "Spear", "Rod", "Scythe", "Fork", "Katar", "Rod" };
            string prefix = prefixs.GetValue(rng.Next(prefixs.Length)).ToString();
            string weapon = weapons.GetValue(rng.Next(weapons.Length)).ToString();

            Name = $"{prefix} {weapon}";
            Attack = rng.Next(1, 10) * (int)(Math.Ceiling((double)(exp / 10)) + 1);
            Durability = rng.Next(10, 101);
            Equiped = false;
        }
    }
}
