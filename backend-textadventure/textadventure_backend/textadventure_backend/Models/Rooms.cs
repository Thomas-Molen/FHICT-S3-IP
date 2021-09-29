using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace textadventure_backend.Models
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

        }

        public Rooms(Dungeons dungeon, Vector2 position, string _event, Interactions northInteraction, Interactions eastInteraction, Interactions southInteraction, Interactions westInteraction)
        {
            Dungeon = dungeon;
            DungeonId = dungeon.Id;
            PositionX = (int)position.X;
            PositionY = (int)position.Y;
            Event = _event;

            NorthInteraction = northInteraction;
            NorthInteractionId = northInteraction.Id;

            EastInteraction = eastInteraction;
            EastInteractionId = eastInteraction.Id;

            SouthInteraction = southInteraction;
            SouthInteractionId = southInteraction.Id;

            WestInteraction = westInteraction;
            WestInteractionId = westInteraction.Id;
        }

        public Rooms(Dungeons dungeon, int positionX, int positionY, string _event, Interactions northInteraction, Interactions eastInteraction, Interactions southInteraction, Interactions westInteraction)
        {
            Dungeon = dungeon;
            DungeonId = dungeon.Id;
            PositionX = positionX;
            PositionY = positionY;
            Event = _event;

            NorthInteraction = northInteraction;
            NorthInteractionId = northInteraction.Id;

            EastInteraction = eastInteraction;
            EastInteractionId = eastInteraction.Id;

            SouthInteraction = southInteraction;
            SouthInteractionId = southInteraction.Id;

            WestInteraction = westInteraction;
            WestInteractionId = westInteraction.Id;
        }
    }
}
