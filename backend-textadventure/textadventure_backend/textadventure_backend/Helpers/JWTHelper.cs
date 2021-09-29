using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using textadventure_backend.Context;
using textadventure_backend.Models;

namespace textadventure_backend.Helpers
{
    public class JWTHelper
    {
        private readonly IContextFactory contextFactory;
        private readonly AppSettings appSettings;

        public JWTHelper(IContextFactory _contextFactory, IOptions<AppSettings> _appSettings)
        {
            contextFactory = _contextFactory;
            appSettings = _appSettings.Value;
        }

        public async Task<VerificationResponse> RenewTokens(string _refreshToken)
        {
            using (var db = contextFactory.CreateDbContext())
            {
                Users user = db.Users.SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == _refreshToken));

                if (user == null)
                {
                    throw new ArgumentException("No user found with token");
                }
                var refreshToken = user.RefreshTokens.Single(x => x.Token == _refreshToken);

                if (!refreshToken.Useable)
                {
                    throw new ArgumentException("Token is no longer active");
                }

                // cleanup refresh tokens and give new tokens
                var newRefreshToken = await GenerateRefreshToken(user);

                var activeRefreshTokens = db.Entry(user).Collection(u => u.RefreshTokens).Query().Where(rt => rt.Useable == true).ToList();
                activeRefreshTokens.ForEach(delegate (RefreshTokens refreshToken)
                {
                    refreshToken.RevokedAt = DateTime.UtcNow;
                });

                // generate new jwt
                var jwtToken = GenerateJwtToken(user);

                return new VerificationResponse(user, jwtToken, newRefreshToken.Token);
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
                Expires = DateTime.UtcNow.AddMinutes(5),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<RefreshTokens> GenerateRefreshToken(Users user)
        {
            using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                var randomBytes = new byte[64];
                rngCryptoServiceProvider.GetBytes(randomBytes);

                var refreshToken = new RefreshTokens(user, Convert.ToBase64String(randomBytes), DateTime.UtcNow.AddMinutes(10));

                using (var db = contextFactory.CreateDbContext())
                {
                    var test1 = await db.AddAsync(refreshToken);
                    var test2 = await db.SaveChangesAsync();

                    return refreshToken;
                }
            }
        }
    }
}
