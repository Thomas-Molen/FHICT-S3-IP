using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace textadventure_backend.Context
{
    [Obsolete("Using default idbcontext now", true)]
    public interface IContextFactory
    {
        public TextadventureDBContext CreateDbContext(string[] args = null);
    }
}
