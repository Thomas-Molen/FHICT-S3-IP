using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace textadventure_backend_entitymanager.Models
{
    public class Users : DefaultModel
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public bool Admin { get; set; } = false;
        public virtual ICollection<RefreshTokens> RefreshTokens { get; set; }
        public virtual ICollection<Adventurers> Adventurers { get; set; }

        [JsonIgnore]
        public string Password { get; set; }

        public Users()
        {
            RefreshTokens = new HashSet<RefreshTokens>();
            Adventurers = new HashSet<Adventurers>();
        }

        public Users(string _email, string _username, string _password)
        {
            RefreshTokens = new HashSet<RefreshTokens>();
            Adventurers = new HashSet<Adventurers>();

            Email = _email;
            Username = _username;
            Password = _password;
        }
    }
}
