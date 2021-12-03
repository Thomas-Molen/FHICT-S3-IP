using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using textadventure_backend.Enums;

namespace textadventure_backend.Models.Entities
{
    public class Rooms : DefaultModel
    {
        public int DungeonId { get; set; }
        public int PositionX { get; set; }
        public int PositionY { get; set; }
        public string Event { get; set; }
        public string NorthInteraction { get; set; } = "Wall";
        public string EastInteraction { get; set; } = "Wall";
        public string SouthInteraction { get; set; } = "Wall";
        public string WestInteraction { get; set; } = "Wall";
        public bool EventCompleted { get; set; } = false;

        public virtual Dungeons Dungeon { get; set; }
        public virtual ICollection<Adventurers> Adventurers { get; set; }
        public virtual ICollection<AdventurerMaps> AdventurerMaps { get; set; }

        public Rooms()
        {
            Adventurers = new HashSet<Adventurers>();
            AdventurerMaps = new HashSet<AdventurerMaps>();
        }
    }
}
