using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using textadventure_backend.Enums;

namespace textadventure_backend.Models.Entities
{
    public class Enemy
    {
        public string Name { get; set; }
        public Weapons Weapon { get; set; }
        public int Health { get; set; }
        public int Difficulty { get; set; }
        public int? RoomId { get; set; }
    }
}
