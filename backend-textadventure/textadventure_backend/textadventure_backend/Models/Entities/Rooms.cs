using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace textadventure_backend.Models.Entities
{
    public class Rooms : DefaultModel
    {
        public int DungeonId { get; set; }
        public int PositionX { get; set; }
        public int PositionY { get; set; }
        public string Event { get; set; }
        public int NorthInteractionId { get; set; }
        public int EastInteractionId { get; set; }
        public int SouthInteractionId { get; set; }
        public int WestInteractionId { get; set; }

        public virtual Dungeons Dungeon { get; set; }
        public virtual Interactions NorthInteraction { get; set; }
        public virtual Interactions EastInteraction { get; set; }
        public virtual Interactions SouthInteraction { get; set; }
        public virtual Interactions WestInteraction { get; set; }

        public virtual ICollection<Adventurers> Adventurers { get; set; }
        public virtual ICollection<AdventurerMaps> AdventurerMaps { get; set; }

        public Rooms()
        {
            Adventurers = new HashSet<Adventurers>();
            AdventurerMaps = new HashSet<AdventurerMaps>();
        }
    }
}
