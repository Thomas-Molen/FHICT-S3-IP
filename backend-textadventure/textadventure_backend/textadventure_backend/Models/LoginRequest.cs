using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace textadventure_backend.Models
{
    public class LoginRequest
    {
        [Required]
        public string email { get; set; }
        [Required]
        public string passsword { get; set; }
    }
}
