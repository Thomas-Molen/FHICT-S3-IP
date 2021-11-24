using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace textadventure_backend.Models.Requests
{
    public class EnterRoomRequest
    {
        public string Message { get; set; }
        public bool NewRoom { get; set; }
    }
}
