using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using textadventure_backend.Context;
using textadventure_backend.Models;
using textadventure_backend.Services.Interfaces;

namespace textadventure_backend.Services
{
    public class UserService : CRUDService<Users>, IUserService
    {
        private readonly IContextFactory contextFactory;

        public UserService(IContextFactory _contextFactory) : base(_contextFactory)
        {
            contextFactory = _contextFactory;
        }

        public async Task<VerificationResponse> Register(RegisterRequest request)
        {
            using (var db = contextFactory.CreateDbContext())
            {
                if (await db.Users.FirstOrDefaultAsync(u => u.email == request.email) != null)
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

                return new VerificationResponse(newUser, "tokenlol");
            }
        }

        public async Task<VerificationResponse> Login(LoginRequest request)
        {
            using (var db = contextFactory.CreateDbContext())
            {
                Users user = await db.Users.FirstOrDefaultAsync(u => u.email == request.email);

                if (user == null)
                {
                    throw new ArgumentException("Email does not exist");
                }

                if (!BCrypt.Net.BCrypt.Verify(request.passsword, user.password))
                {
                    throw new ArgumentException("Combination of email and password does not match");
                }

                return new VerificationResponse(user, "tokenlol");
            }
        }
    }
}
