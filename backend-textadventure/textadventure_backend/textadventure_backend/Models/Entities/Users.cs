using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace textadventure_backend.Models.Entities
{
    public class Users : DefaultModel
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public bool Admin { get; set; } = false;
        public virtual ICollection<Adventurers> Adventurers { get; set; }

        public Users()
        {
            Adventurers = new HashSet<Adventurers>();
        }
    }
}
