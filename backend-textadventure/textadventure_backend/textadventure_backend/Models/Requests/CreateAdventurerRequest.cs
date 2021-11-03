using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace textadventure_backend.Models.Requests
{
    public class CreateAdventurerRequest
    {
        public string Name { get; set; }
    }
}
