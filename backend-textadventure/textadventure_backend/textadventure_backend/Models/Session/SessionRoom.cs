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
                switch (Enum.Parse(typeof(Events), Event.ToString()))
                {
                    case Events.Chest:
                        return "an already opened chest, seems like you have already been here";
                    case Events.Enemy:
                        return "a slain enemy, brings back memories of your past victory";
                    case Events.Empty:
                        return "an empty room that you seem to remember having been to before. It is unsettling";
                    default:
                        return "actually nothing hmmm, maybe a bug maybe a feature who knows!";
                }
            }
            else
            {
                switch (Enum.Parse(typeof(Events), Event.ToString()))
                {
                    case Events.Chest:
                        return "a treasure chest! There might be some good loot in there";
                    case Events.Enemy:
                        return "a monster wielding some kind of weapon";
                    case Events.Empty:
                        return "nothing... how strange";
                    default:
                        return "actually nothing hmmm, maybe a bug maybe a feature who knows!";
                }
            }
        }
    }
}
