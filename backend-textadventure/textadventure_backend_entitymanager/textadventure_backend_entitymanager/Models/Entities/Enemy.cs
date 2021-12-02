using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using textadventure_backend_entitymanager.Enums;

namespace textadventure_backend_entitymanager.Models.Entities
{
    public class Enemy
    {
        public string Name { get; set; }
        public Weapons Weapon { get; set; }
        public int Health { get; set; } = 1;
        public int Difficulty { get; set; } = 1;
        public int? RoomId { get; set; } = null;

        public Enemy()
        {
        }

        public Enemy(int experience)
        {
            string[] names = new string[] { "Zombie", "Skeleton", "Orc", "Gorrila?", "Dwarf", "Gnome", "Imp", "Ogre", "Troll"};
            Random rng = new Random();
            Array possbileDifficulties = typeof(Difficulty).GetEnumValues();
            var difficulty = (int)possbileDifficulties.GetValue(rng.Next(possbileDifficulties.Length));

            Difficulty = difficulty;
            Health = (rng.Next(3, 10) * difficulty) + (rng.Next(3, 10) * (experience / 10));
            Weapon = new Weapons(experience);
            Name = names.GetValue(rng.Next(names.Length)).ToString();
        }
    }
}
