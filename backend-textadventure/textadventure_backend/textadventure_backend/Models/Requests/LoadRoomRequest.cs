using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using textadventure_backend.Models.Entities;

namespace textadventure_backend.Models.Requests
{
    public class LoadRoomRequest
    {
        public string Message { get; set; }
        public string Event { get; set; } = "empty";
        public bool EventCompleted { get; set; } = false;
        public string NorthInteraction { get; set; } = "Wall";
        public string EastInteraction { get; set; } = "Wall";
        public string SouthInteraction { get; set; } = "Wall";
        public string WestInteraction { get; set; } = "Wall";
    }
}
