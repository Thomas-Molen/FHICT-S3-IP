using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using textadventure_backend_entitymanager.Context;
using textadventure_backend_entitymanager.Models;
using textadventure_backend_entitymanager.Models.Entities;
using textadventure_backend_entitymanager.Models.Responses;

namespace textadventure_backend_entitymanager.Helpers
{
    public class JWTHelper
    {
        private readonly IDbContextFactory<TextadventureDBContext> contextFactory;
        private readonly AppSettings appSettings;
        private readonly JwtSecurityTokenHandler tokenHandler;

        public JWTHelper(IDbContextFactory<TextadventureDBContext> _contextFactory, IOptions<AppSettings> _appSettings)
        {
            contextFactory = _contextFactory;
            appSettings = _appSettings.Value;
            tokenHandler = new JwtSecurityTokenHandler();
        }

        //JWT
        public async Task<VerificationResponse> RenewTokens(string _refreshToken)
        {
            if (_refreshToken == null)
            {
                throw new ArgumentException("No refreshToken given");
            }

            using (var db = contextFactory.CreateDbContext())
            {
                Users user = db.Users.FirstOrDefault(u => u.RefreshTokens.Any(rt => rt.Token == _refreshToken));

                if (user == null)
                {
                    throw new ArgumentException("No user found with token");
                }
                var refreshToken = db.RefreshTokens.FirstOrDefault(x => x.Token == _refreshToken);

                if (!refreshToken.Useable)
                {
                    throw new ArgumentException("Token is no longer active");
                }

                refreshToken.RevokedAt = DateTime.UtcNow;
                db.Update(user);
                await db.SaveChangesAsync();

                var jwtToken = GenerateJwtToken(user);
                var newRefreshToken = await GenerateRefreshToken(user);

                return new VerificationResponse(user, jwtToken, newRefreshToken.Token);
            }
        }

        public async Task DeactivateRefreshToken(string _refreshToken)
        {
            if (_refreshToken == null)
            {
                throw new ArgumentException("No refreshToken given");
            }

            using (var db = contextFactory.CreateDbContext())
            {
                Users user = db.Users.FirstOrDefault(u => u.RefreshTokens.Any(rt => rt.Token == _refreshToken));

                if (user == null)
                {
                    throw new ArgumentException("No user found with token");
                }
                var refreshToken = db.RefreshTokens.FirstOrDefault(x => x.Token == _refreshToken);

                if (refreshToken.Useable)
                {
                    refreshToken.RevokedAt = DateTime.UtcNow;
                    db.Update(user);
                    await db.SaveChangesAsync();
                }
            }
        }

        public string GenerateJwtToken(Users user)
        {
            // generate token that is valid for x days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<RefreshTokens> GenerateRefreshToken(Users user)
        {
            using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
            using (var db = contextFactory.CreateDbContext())
            {
                var randomBytes = new byte[64];
                rngCryptoServiceProvider.GetBytes(randomBytes);

                var newRefreshToken = new RefreshTokens(
                    Convert.ToBase64String(randomBytes),
                    DateTime.UtcNow.AddDays(7)
                );

                user.RefreshTokens.Add(newRefreshToken);
                db.Update(user);
                await db.SaveChangesAsync();

                return newRefreshToken;
            }
        }

        public int GetUserIdFromJWT(string authHeader)
        {
            var JWTToken = tokenHandler.ReadJwtToken(authHeader.Replace("Bearer ", ""));
            using (var db = contextFactory.CreateDbContext())
            {
                int userId = int.Parse(JWTToken.Claims.First(t => t.Type == "id").Value);
                var user = db.Users.FirstOrDefault(u => u.Id == userId);
                if (user == null)
                {
                    throw new ArgumentException("there is no user connected to this token");
                }
                return user.Id;
            }   
        }
    }
}
