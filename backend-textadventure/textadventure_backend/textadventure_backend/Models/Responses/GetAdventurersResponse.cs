using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace textadventure_backend.Models.Responses
{
    public class GetAdventurersResponse
    {
        public string Name { get; set; }
        public int Level { get; set; }
        public int Health { get; set; }
        public int Id { get; set; }
        public int Damage { get; set; }
    }
}
