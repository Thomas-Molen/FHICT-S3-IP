using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace textadventure_backend_entitymanager.Models.Entities
{
    public class AdventurerMaps : DefaultModel
    {
        public bool EventCompleted { get; set; } = false;
        public int AdventurerId { get; set; }
        public int RoomId { get; set; }

        public virtual Adventurers Adventurer { get; set; }
        public virtual Rooms Room { get; set; }

        public AdventurerMaps ()
        {

        }
    }
}
