using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace textadventure_backend.Services.Interfaces
{
    public interface IRoomService
    {
        Task CompleteRoom(int adventurerId);
    }
}
