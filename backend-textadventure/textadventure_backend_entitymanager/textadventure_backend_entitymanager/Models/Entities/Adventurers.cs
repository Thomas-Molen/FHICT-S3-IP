using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace textadventure_backend_entitymanager.Models.Entities
{
    public class Adventurers : DefaultModel
    {
        public int Experience { get; set; } = 10;
        public int Health { get; set; } = 20;

        public string Name { get; set; } = "Adventurer";
        public int UserId { get; set; }
        public int DungeonId { get; set; }
        public int? RoomId { get; set; }

        public virtual Users User { get; set; }
        public virtual Dungeons Dungeon { get; set; }
        public virtual Rooms Room { get; set; } = null;

        public virtual ICollection<AdventurerMaps> AdventurerMaps { get; set; }
        public virtual ICollection<Weapons> Weapons { get; set; }
        public virtual ICollection<Items> Items { get; set; }

        public Adventurers()
        {
            AdventurerMaps = new HashSet<AdventurerMaps>();
            Weapons = new HashSet<Weapons>();
            Items = new HashSet<Items>();
        }
    }
}
