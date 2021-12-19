using System.Threading.Tasks;

namespace textadventure_backend.Services.Interfaces
{
    public interface ICommandService
    {
        Task HandleExploringCommands(string connectionId, string message);
        Task HandleFightingCommands(string connectionId, string message);
    }
}