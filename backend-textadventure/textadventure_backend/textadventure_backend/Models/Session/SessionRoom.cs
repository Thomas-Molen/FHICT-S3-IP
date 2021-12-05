using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using textadventure_backend.Enums;

namespace textadventure_backend.Models.Session
{
    public class SessionRoom
    {
        public int Id { get; set; }
        public string Event { get; set; } = "empty";
        public bool EventCompleted { get; set; } = false;
        public string NorthInteraction { get; set; } = "Wall";
        public string EastInteraction { get; set; } = "Wall";
        public string SouthInteraction { get; set; } = "Wall";
        public string WestInteraction { get; set; } = "Wall";

        public override string ToString()
        {
            if (EventCompleted)
            {
                return (Enum.Parse(typeof(Events), Event)) switch
                {
                    Events.Chest => "an already opened chest, seems like you have already been here",
                    Events.Enemy => "a slain enemy, brings back memories of your past victory",
                    Events.Empty => "an empty room that you seem to remember having been to before. It is unsettling",
                    _ => "actually nothing hmmm, maybe a bug maybe a feature who knows!",
                };
            }
            else
            {
                return (Enum.Parse(typeof(Events), Event)) switch
                {
                    Events.Chest => "a treasure chest! There might be some good loot in there",
                    Events.Enemy => "a monster wielding some kind of weapon",
                    Events.Empty => "nothing... how strange",
                    _ => "actually nothing hmmm, maybe a bug maybe a feature who knows!",
                };
            }
        }
    }
}
