using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace textadventure_backend_entitymanager.Models
{
    public class Interactions : DefaultModel
    {
        public string Type { get; set; }
        public int? NPCId { get; set; }

        public virtual NPCs NPC { get; set; }
        public virtual ICollection<Rooms> RoomNorth { get; set; }
        public virtual ICollection<Rooms> RoomEast { get; set; }
        public virtual ICollection<Rooms> RoomSouth { get; set; }
        public virtual ICollection<Rooms> RoomWest { get; set; }

        public Interactions()
        {
            RoomNorth = new HashSet<Rooms>();
            RoomEast = new HashSet<Rooms>();
            RoomSouth = new HashSet<Rooms>();
            RoomWest = new HashSet<Rooms>();
        }

        public Interactions(string type)
        {
            RoomNorth = new HashSet<Rooms>();
            RoomEast = new HashSet<Rooms>();
            RoomSouth = new HashSet<Rooms>();
            RoomWest = new HashSet<Rooms>();

            Type = type;
        }
    }
}
