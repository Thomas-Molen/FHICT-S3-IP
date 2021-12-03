using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using textadventure_backend_entitymanager.Enums;

namespace textadventure_backend_entitymanager.Models.Entities
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
        
        [NotMapped]
        public bool EventCompleted { get; set; }

        public virtual Dungeons Dungeon { get; set; }
        public virtual ICollection<Adventurers> Adventurers { get; set; }
        public virtual ICollection<AdventurerMaps> AdventurerMaps { get; set; }

        public Rooms()
        {
            Adventurers = new HashSet<Adventurers>();
            AdventurerMaps = new HashSet<AdventurerMaps>();
        }

        public Rooms(int dungeonId, Vector2 position, ICollection<AdjacentRooms> adjacentRooms)
        {
            Adventurers = new HashSet<Adventurers>();
            AdventurerMaps = new HashSet<AdventurerMaps>();

            DungeonId = dungeonId;
            PositionX = (int)position.X;
            PositionY = (int)position.Y;

            //set random Event
            Random rng = new Random();
            Array possbileEvents = typeof(Events).GetEnumValues();
            Event = possbileEvents.GetValue(rng.Next(possbileEvents.Length)).ToString();
            SetInteraction(adjacentRooms, rng);
        }

        public Rooms(int dungeonId, Vector2 position, ICollection<AdjacentRooms> adjacentRooms, Events _event)
        {
            Adventurers = new HashSet<Adventurers>();
            AdventurerMaps = new HashSet<AdventurerMaps>();

            DungeonId = dungeonId;
            PositionX = (int)position.X;
            PositionY = (int)position.Y;
            Event = _event.ToString();
            SetInteraction(adjacentRooms, new Random());
        }

        private void SetInteraction(ICollection<AdjacentRooms> adjacentRooms, Random rng)
        {
            //set interactions based on adjacentRooms
            foreach (var entry in adjacentRooms)
            {
                //generate what interaction given roomside should get
                DirectionalInteractions interaction = DirectionalInteractions.Wall;
                if (entry.HasDoor() || (entry.Room == null && rng.Next(1, 11) < 8))
                {
                    interaction = DirectionalInteractions.Door;
                }
                else if (entry.Room != null)
                {
                    interaction = DirectionalInteractions.WeakWall;
                }

                //Set wall's interactions depending on which direction the adjacent room is
                switch (entry.RelativePosition)
                {
                    case Directions.North:
                        NorthInteraction = interaction.ToString();
                        break;
                    case Directions.East:
                        EastInteraction = interaction.ToString();
                        break;
                    case Directions.South:
                        SouthInteraction = interaction.ToString();
                        break;
                    case Directions.West:
                        WestInteraction = interaction.ToString();
                        break;
                    default:
                        break;
                }
            }
        }

        public bool DirectionHasDoor(string direction)
        {
            switch (direction.ToLower())
            {
                case "north":
                    if (NorthInteraction == "Door")
                    {
                        return true;
                    }
                    break;
                case "south":
                    if (SouthInteraction == "Door")
                    {
                        return true;
                    }
                    break;
                case "east":
                    if (EastInteraction == "Door")
                    {
                        return true;
                    }
                    break;
                case "west":
                    if (WestInteraction == "Door")
                    {
                        return true;
                    }
                    break;
            }
            return false;
        }
    }
}
