using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace textadventure_backend.Models.Requests
{
    public class EquipWeaponRequest
    {
        public int AdventurerId { get; set; }
        public int WeaponId { get; set; }
    }
}
