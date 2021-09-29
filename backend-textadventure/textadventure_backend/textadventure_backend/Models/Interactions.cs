using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace textadventure_backend.Models
{
    public class Interactions : DefaultModel
    {
        public string Type { get; set; }
        public int NPCId { get; set; }

        public virtual NPCs NPC { get; set; }
        public virtual ICollection<Rooms> RoomNorth { get; set; }
        public virtual ICollection<Rooms> RoomEast { get; set; }
        public virtual ICollection<Rooms> RoomSouth { get; set; }
        public virtual ICollection<Rooms> RoomWest { get; set; }

        public Interactions()
        {

        }

        public Interactions(string type, NPCs npc)
        {
            Type = type;
            NPC = npc;
            NPCId = npc.Id;
        }
    }
}
