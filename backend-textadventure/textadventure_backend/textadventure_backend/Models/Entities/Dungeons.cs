using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace textadventure_backend.Models.Entities
{
    public class Dungeons : DefaultModel
    {
        public virtual ICollection<Rooms> Rooms { get; set; }
        public virtual ICollection<Adventurers> Adventurers { get; set; }

        public Dungeons()
        {
            Rooms = new HashSet<Rooms>();
            Adventurers = new HashSet<Adventurers>();
        }
    }
}
