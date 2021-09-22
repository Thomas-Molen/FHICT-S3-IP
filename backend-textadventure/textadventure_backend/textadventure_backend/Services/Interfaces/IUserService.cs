using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using textadventure_backend.Models;

namespace textadventure_backend.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<users>> GetUsers();
    }
}
