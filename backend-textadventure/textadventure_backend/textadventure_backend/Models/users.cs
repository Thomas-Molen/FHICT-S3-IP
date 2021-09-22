using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace textadventure_backend.Models
{
    public class users : DefaultModel
    {
        public string email { get; set; }
        public string username { get; set; }
        public bool admin { get; set; }

        [JsonIgnore]
        public string password { get; set; }

        public users()
        {

        }

        public users(string _email, string _password, string _username)
        {
            email = _email;
            password = _password;
            username = _username;
        }
    }
}
