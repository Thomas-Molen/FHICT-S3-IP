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

                await db.AddAsync(newUser);
                await db.SaveChangesAsync();

                // authentication successful so generate tokens
                var token = GenerateJwtToken(newUser);
                var refreshToken = await GenerateRefreshToken(newUser);

                return new VerificationResponse(newUser, token, refreshToken.Token);
            }
        }

        public async Task<VerificationResponse> Login(LoginRequest request)
        {
            using (var db = contextFactory.CreateDbContext())
            {
                Users user = await db.Users.Include(u => u.RefreshTokens).FirstOrDefaultAsync(u => u.Email == request.email);

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
                var refreshToken = await GenerateRefreshToken(user);

                return new VerificationResponse(user, token, refreshToken.Token);
            }
        }

        //JWT
        /*public async Task<VerificationResponse> RenewTokens(string token)
        {
            using (var db = contextFactory.CreateDbContext())
            {
                Users user = db.Users.SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == token));

                if (user == null)
                {
                    throw new ArgumentException("No user found with token");
                } 
                var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

                if (!refreshToken.Useable)
                {
                    throw new ArgumentException("Token is no longer active");   
                }

                // replace old refresh token with a new one and save
                var newRefreshToken = GenerateRefreshToken(user);

                var activeRefreshTokens = db.Entry(user).Collection(u => u.RefreshTokens).Query().Where(rt => rt.Useable == true).ToList();
                activeRefreshTokens.ForEach(delegate (RefreshTokens refreshToken) 
                {
                    refreshToken.RevokedAt = DateTime.UtcNow;
                });

                await db.AddAsync(newRefreshToken);
                await db.SaveChangesAsync();

                // generate new jwt
                var jwtToken = GenerateJwtToken(user);

                return new VerificationResponse(user, jwtToken, newRefreshToken.Token);
            }
        }*/

        private string GenerateJwtToken(Users user)
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

        private async Task<RefreshTokens> GenerateRefreshToken(Users user)
        {
            using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
            using (var db = contextFactory.CreateDbContext())
            {
                var randomBytes = new byte[64];
                rngCryptoServiceProvider.GetBytes(randomBytes);

                if (user.RefreshTokens != null)
                {
                    user.RefreshTokens.ToList().ForEach(delegate (RefreshTokens refreshToken)
                    {
                        if (refreshToken.Useable)
                        {
                            refreshToken.RevokedAt = DateTime.UtcNow;
                            db.Update(refreshToken);
                        }
                    });
                }
                
                var newRefreshToken = new RefreshTokens(
                    user,
                    Convert.ToBase64String(randomBytes),
                    DateTime.UtcNow.AddMinutes(10)
                );

                await db.AddAsync(newRefreshToken);
                await db.SaveChangesAsync();

                return newRefreshToken;
            }
        }
    }
}
