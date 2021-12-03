using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace textadventure_backend.Models.Entities
{
    public class NPCs : DefaultModel
    {
        public string Conversation { get; set; }
        public int Risk { get; set; }
        public int WeaponId { get; set; }
        public int ItemId { get; set; }

        public virtual Weapons Weapon { get; set; }
        public virtual Items Item { get; set; }

        public NPCs()
        {
        }
    }
}
