using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace textadventure_backend.Models.Requests
{
    public class JoinGameRequest
    {
        public int adventurerId { get; set; }
        public int userId { get; set; }
    }
}
