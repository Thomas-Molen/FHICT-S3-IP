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
        public string NorthInteraction { get; set; } = "Wall";
        public string EastInteraction { get; set; } = "Wall";
        public string SouthInteraction { get; set; } = "Wall";
        public string WestInteraction { get; set; } = "Wall";

        public virtual Dungeons Dungeon { get; set; }
        public virtual ICollection<Adventurers> Adventurers { get; set; }
        public virtual ICollection<AdventurerMaps> AdventurerMaps { get; set; }

        public Rooms()
        {
            Adventurers = new HashSet<Adventurers>();
            AdventurerMaps = new HashSet<AdventurerMaps>();
        }

        public Rooms(Vector2 position, string _event)
        {
            Adventurers = new HashSet<Adventurers>();
            AdventurerMaps = new HashSet<AdventurerMaps>();

            PositionX = (int)position.X;
            PositionY = (int)position.Y;
            Event = _event;
        }

        public Rooms(int positionX, int positionY, string _event)
        {
            PositionX = positionX;
            PositionY = positionY;
            Event = _event;
        }
    }
}
