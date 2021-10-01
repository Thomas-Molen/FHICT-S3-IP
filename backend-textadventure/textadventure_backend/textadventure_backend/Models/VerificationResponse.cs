﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace textadventure_backend.Models
{
    public class VerificationResponse
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public bool Admin { get; set; }
        public string Token { get; set; }

        [JsonIgnore] // refresh token is returned in http only cookie
        public string RefreshToken { get; set; }

        public VerificationResponse(Users user, string _token, string _refreshToken)
        {
            Id = user.Id;
            Email = user.Email;
            Admin = user.Admin;
            Token = _token;
            RefreshToken = _refreshToken;
        }
    }
}