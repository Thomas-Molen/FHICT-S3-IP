using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using textadventure_backend_entitymanager.Models.Entities;

namespace textadventure_backend_entitymanager.Models.Responses
{
    public class OpenChestResponse
    {
        public string Message { get; set; }
        public Weapons weapon { get; set; }
    }
}
