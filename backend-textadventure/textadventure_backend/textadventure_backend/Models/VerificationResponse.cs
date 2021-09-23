using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace textadventure_backend.Models
{
    public class VerificationResponse
    {
        public int id { get; set; }
        public string email { get; set; }
        public bool admin { get; set; }
        public string token { get; set; }

        public VerificationResponse(Users user, string _token)
        {
            id = user.id;
            email = user.email;
            admin = user.admin;
            token = _token;
        }
    }
}
