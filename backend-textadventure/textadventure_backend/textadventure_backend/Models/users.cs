using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace textadventure_backend.Models
{
    public class Users : DefaultModel
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public bool Admin { get; set; }

        [JsonIgnore]
        public string Password { get; set; }

        [JsonIgnore]
        public List<RefreshTokens> RefreshTokens { get; set; }


        public Users()
        {

        }

        public Users(string _email, string _username, string _password)
        {
            Email = _email;
            Username = _username;
            Password = _password;
        }
    }
}
