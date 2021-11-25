using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using textadventure_backend.Models.Entities;

namespace textadventure_backend.Models
{
    public class Session
    {
        public Adventurers Adventurer { get; set; }
        public string ConnectionId { get; set; }
        public string Group { get; set; }
        
        public SessionRoom Room { get; set; }
        public List<Weapons> Weapons { get; set; }
    }
}
