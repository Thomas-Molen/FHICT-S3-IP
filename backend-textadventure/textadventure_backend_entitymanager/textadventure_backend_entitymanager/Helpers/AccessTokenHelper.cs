using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace textadventure_backend_entitymanager.Helpers
{
    public class AccessTokenHelper
    {
        private readonly AppSettings appSettings;

        public AccessTokenHelper(IOptions<AppSettings> _appSettings)
        {
            appSettings = _appSettings.Value;
        }

        public bool IsTokenValid(string accessToken)
        {
            if (appSettings.GameAccessToken == accessToken)
            {
                return true;
            }
            return false;
        }
    }
}
