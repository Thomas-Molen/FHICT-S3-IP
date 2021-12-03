using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using textadventure_backend_entitymanager.Enums;
using textadventure_backend_entitymanager.Models.Entities;

namespace textadventure_backend_entitymanager.Models
{
    public class AdjacentRooms
    {
        public Directions RelativePosition { get; set; }
        public Rooms Room { get; set; } = null;

        public bool HasDoor()
        {
            if (Room == null)
            {
                return false;
            }
            switch (RelativePosition)
            {
                //this might be able to be done easier
                case Directions.North:
                    if (Room.SouthInteraction == DirectionalInteractions.Door.ToString())
                    {
                        return true;
                    }
                    break;
                case Directions.East:
                    if (Room.WestInteraction == DirectionalInteractions.Door.ToString())
                    {
                        return true;
                    }
                    break;
                case Directions.South:
                    if (Room.NorthInteraction == DirectionalInteractions.Door.ToString())
                    {
                        return true;
                    }
                    break;
                case Directions.West:
                    if (Room.EastInteraction == DirectionalInteractions.Door.ToString())
                    {
                        return true;
                    }
                    break;
                default:
                    break;
            }
            return false;
        }
    }
}
