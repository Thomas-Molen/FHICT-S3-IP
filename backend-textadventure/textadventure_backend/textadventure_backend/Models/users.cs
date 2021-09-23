using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace textadventure_backend.Models
{
    public class Users : DefaultModel
    {
        public string email { get; set; }
        public string username { get; set; }
        public bool admin { get; set; }

        [JsonIgnore]
        public string password { get; set; }

        public Users()
        {

        }

        public Users(string _email, string _username, string _password)
        {
            email = _email;
            username = _username;
            password = _password;
        }
    }
}
