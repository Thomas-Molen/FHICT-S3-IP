using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace textadventure_backend.Context
{
    public interface IContextFactory
    {
        public TextadventureDBContext CreateDbContext(string[] args = null);
    }
}
