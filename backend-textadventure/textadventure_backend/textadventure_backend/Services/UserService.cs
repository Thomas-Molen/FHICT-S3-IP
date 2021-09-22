using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using textadventure_backend.Context;
using textadventure_backend.Models;
using textadventure_backend.Services.Interfaces;

namespace textadventure_backend.Services
{
    public class UserService : CRUDService<users>, IUserService
    {
        private readonly IContextFactory contextFactory;

        public UserService(IContextFactory _contextFactory) : base(_contextFactory)
        {
            contextFactory = _contextFactory;
        }

        public async Task<IEnumerable<users>> GetUsers()
        {
            return await Get();
        }
    }
}
