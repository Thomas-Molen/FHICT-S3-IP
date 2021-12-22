using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using textadventure_backend_entitymanager.Context;
using textadventure_backend_entitymanager.Helpers;
using textadventure_backend_entitymanager.Models.Entities;
using textadventure_backend_entitymanager.Models.Requests;
using textadventure_backend_entitymanager.Models.Responses;

namespace textadventure_backend_entitymanager.Services
{
    public interface IUserService
    {
        Task DeactivateToken(string refreshToken);
        Task<VerificationResponse> Login(LoginRequest request);
        Task<VerificationResponse> Register(RegisterRequest request);
        Task<VerificationResponse> RenewToken(string refreshToken);
    }

    public class UserService : IUserService
    {
        private readonly IDbContextFactory<TextadventureDBContext> contextFactory;
        private readonly JWTHelper JWT;

        public UserService(IDbContextFactory<TextadventureDBContext> _contextFactory, JWTHelper JWThelper)
        {
            contextFactory = _contextFactory;
            JWT = JWThelper;
        }

        public async Task<VerificationResponse> Register(RegisterRequest request)
        {
            using (var db = contextFactory.CreateDbContext())
            {
                if (await db.Users.OrderByDescending(x => x.Id).FirstOrDefaultAsync(u => u.Email == request.email) != null)
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
                var token = JWT.GenerateJwtToken(newUser);
                var refreshToken = await JWT.GenerateRefreshToken(newUser);

                return new VerificationResponse(newUser, token, refreshToken.Token);
            }
        }

        public async Task<VerificationResponse> Login(LoginRequest request)
        {
            using (var db = contextFactory.CreateDbContext())
            {
                Users user = await db.Users.OrderByDescending(x => x.Id).Include(u => u.RefreshTokens).FirstOrDefaultAsync(u => u.Email == request.email);

                if (user == null)
                {
                    throw new ArgumentException("Email does not exist");
                }

                if (!BCrypt.Net.BCrypt.Verify(request.password, user.Password))
                {
                    throw new ArgumentException("Combination of email and password does not match");
                }

                // authentication successful so generate jwt token
                var token = JWT.GenerateJwtToken(user);
                var refreshToken = await JWT.GenerateRefreshToken(user);

                return new VerificationResponse(user, token, refreshToken.Token);
            }
        }

        public async Task<VerificationResponse> RenewToken(string refreshToken)
        {
            return await JWT.RenewTokens(refreshToken);
        }

        public async Task DeactivateToken(string refreshToken)
        {
            await JWT.DeactivateRefreshToken(refreshToken);
        }
    }
}
