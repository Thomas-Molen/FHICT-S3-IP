using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace textadventure_backend_entitymanager.Models.Requests
{
    public class EnterRoomRequest
    {
        public int AdventurerId { get; set; }
        public string Direction { get; set; }
    }
}
