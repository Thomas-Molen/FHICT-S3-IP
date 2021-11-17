using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace textadventure_backend.Models
{
    public class JoinGameRequest
    {
        public int AdventurerId { get; set; }
        public int DungeonId { get; set; }
    }
}
