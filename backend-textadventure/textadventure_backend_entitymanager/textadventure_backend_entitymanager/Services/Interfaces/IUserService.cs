using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using textadventure_backend_entitymanager.Models;
using textadventure_backend_entitymanager.Models.Requests;
using textadventure_backend_entitymanager.Models.Responses;

namespace textadventure_backend_entitymanager.Services.Interfaces
{
    public interface IUserService
    {
        Task<VerificationResponse> Register(RegisterRequest request);
        Task<VerificationResponse> Login(LoginRequest request);
        Task<VerificationResponse> RenewToken(string refreshToken);
    }
}
