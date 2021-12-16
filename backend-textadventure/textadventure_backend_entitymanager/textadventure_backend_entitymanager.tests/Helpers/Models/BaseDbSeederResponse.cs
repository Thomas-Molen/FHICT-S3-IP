using System;
using System.Collections.Generic;
using System.Text;
using textadventure_backend_entitymanager.Models.Entities;

namespace textadventure_backend_entitymanager.tests.Helpers.Models
{
    class BaseDbSeederResponse
    {
        public Users user { get; set; }
        public Dungeons dungeon { get; set; }
        public Adventurers adventurer { get; set; }
    }
}
