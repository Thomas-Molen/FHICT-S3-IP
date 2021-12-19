using textadventure_backend.Models.Entities;
using textadventure_backend.Models.Session;

namespace textadventure_backend.Services.Interfaces
{
    public interface ISessionManager
    {
        void AddSession(string connectionId, SessionAdventurer adventurer, string group);
        Session GetSession(string connectionId);
        void RemoveSession(string connectionId);
        void UpdateSessionRoom(string connectionId, Rooms room);
        void UpdateSessionRoom(string connectionId, SessionRoom room);
    }
}