using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace textadventure_backend.Models
{
    public class Items : DefaultModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public int AdventurerId { get; set; }

        public virtual Adventurers Adventurer { get; set; }
        public virtual ICollection<NPCs> NPCs { get; set; }

        Items()
        {

        }

        Items(string name, string description, string content, Adventurers adventurer)
        {
            Name = name;
            Description = description;
            Content = content;
            Adventurer = adventurer;
            AdventurerId = adventurer.Id;
        }
    }
}
