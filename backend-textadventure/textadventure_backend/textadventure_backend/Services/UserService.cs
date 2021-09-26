using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using textadventure_backend.Context;
using textadventure_backend.Helpers;
using textadventure_backend.Models;
using textadventure_backend.Services.Interfaces;

namespace textadventure_backend.Services
{
    public class UserService : CRUDService<Users>, IUserService
    {
        private readonly IContextFactory contextFactory;
        private readonly AppSettings appSettings;

        public UserService(IContextFactory _contextFactory, IOptions<AppSettings> _appSettings) : base(_contextFactory)
        {
            contextFactory = _contextFactory;
            appSettings = _appSettings.Value;
        }

        public async Task<VerificationResponse> Register(RegisterRequest request)
        {
            using (var db = contextFactory.CreateDbContext())
            {
                if (await db.Users.FirstOrDefaultAsync(u => u.Email == request.email) != null)
                {
                    throw new ArgumentException("This email has already been used");
                }

                Users newUser = new Users(
                    request.email,
                    request.username,
                    BCrypt.Net.BCrypt.HashPassword(request.password)
                );

                // authentication successful so generate jwt token
                var token = GenerateJwtToken(newUser);
                var refreshToken = GenerateRefreshToken("ipadresslol");

                await db.AddAsync(newUser);
                await db.SaveChangesAsync();

                // save refresh token
                newUser.RefreshTokens.Add(refreshToken);
                db.Update(newUser);
                db.SaveChanges();

                return new VerificationResponse(newUser, token, refreshToken.Token);
            }
        }

        public async Task<VerificationResponse> Login(LoginRequest request)
        {
            using (var db = contextFactory.CreateDbContext())
            {
                Users user = await db.Users.FirstOrDefaultAsync(u => u.Email == request.email);

                if (user == null)
                {
                    throw new ArgumentException("Email does not exist");
                }

                if (!BCrypt.Net.BCrypt.Verify(request.passsword, user.Password))
                {
                    throw new ArgumentException("Combination of email and password does not match");
                }

                // authentication successful so generate jwt token
                var token =  GenerateJwtToken(user);
                var refreshToken = GenerateRefreshToken("ipadresslol");

                // save refresh token
                user.RefreshTokens.Add(refreshToken);
                db.Update(user);
                db.SaveChanges();

                return new VerificationResponse(user, token, refreshToken.Token);
            }
        }

        //JWT
        public VerificationResponse RefreshToken(string token)
        {
            using (var db = contextFactory.CreateDbContext())
            {
                Users user = db.Users.SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == token));

                if (user == null)
                {
                    throw new ArgumentException("No user found with token");
                } 
                var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

                if (!refreshToken.IsActive)
                {
                    throw new ArgumentException("Token is no longer active");   
                }

                // replace old refresh token with a new one and save
                var newRefreshToken = GenerateRefreshToken("tmpipadress");
                refreshToken.Revoked = DateTime.UtcNow;
                refreshToken.RevokedByIp = "tmpipadress";
                refreshToken.ReplacedByToken = newRefreshToken.Token;
                user.RefreshTokens.Add(newRefreshToken);
                db.Users.Update(user);
                db.SaveChanges();

                // generate new jwt
                var jwtToken = GenerateJwtToken(user);

                return new VerificationResponse(user, jwtToken, newRefreshToken.Token);
            }
        }

        private string GenerateJwtToken(Users user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private RefreshTokens GenerateRefreshToken(string ipAddress)
        {
            using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                var randomBytes = new byte[64];
                rngCryptoServiceProvider.GetBytes(randomBytes);
                return new RefreshTokens
                {
                    Token = Convert.ToBase64String(randomBytes),
                    Expires = DateTime.UtcNow.AddDays(7),
                    Created = DateTime.UtcNow,
                    CreatedByIp = ipAddress
                };
            }
        }
    }
}
