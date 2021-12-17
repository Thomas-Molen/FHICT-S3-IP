using System.Threading.Tasks;
using textadventure_backend_entitymanager.Models.Requests;
using textadventure_backend_entitymanager.Models.Responses;

namespace textadventure_backend_entitymanager.Services.Interfaces
{
    public interface IUserService
    {
        Task DeactivateToken(string refreshToken);
        Task<VerificationResponse> Login(LoginRequest request);
        Task<VerificationResponse> Register(RegisterRequest request);
        Task<VerificationResponse> RenewToken(string refreshToken);
    }
}