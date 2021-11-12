using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace textadventure_backend.Models.Entities
{
    public class Adventurers : DefaultModel
    {
        public int Experience { get; set; }
        public int Health { get; set; }
        public string Name { get; set; }
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
