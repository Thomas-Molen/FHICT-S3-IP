using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace textadventure_backend.Models
{
    public class Adventurers : DefaultModel
    {
        public int Experience { get; set; } = 0;
        public int Health { get; set; } = 20;
        public int UserId { get; set; }
        public int DungeonId { get; set; }
        public int RoomId { get; set; }

        public virtual Users User { get; set; }
        public virtual Dungeons Dungeon { get; set; }
        public virtual Rooms Room { get; set; }

        public virtual ICollection<AdventurerMaps> AdventurerMaps { get; set; }
        public virtual ICollection<Weapons> Weapons { get; set; }
        public virtual ICollection<Items> Items { get; set; }

        public Adventurers()
        {

        }

        public Adventurers(Users user, Dungeons dungeon)
        {
            User = user;
            UserId = user.Id;
            Dungeon = dungeon;
            DungeonId = dungeon.Id;
        }
    }
}
