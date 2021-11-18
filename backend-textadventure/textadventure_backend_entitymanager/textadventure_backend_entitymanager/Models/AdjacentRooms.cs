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
    }
}
