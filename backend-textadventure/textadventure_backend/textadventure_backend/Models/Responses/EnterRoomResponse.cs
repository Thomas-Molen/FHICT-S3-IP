using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace textadventure_backend_entitymanager.Models.Responses
{
    public class EnterRoomResponse
    {
        public int adventurerId { get; set; }
        public string Direction { get; set; }
    }
}
