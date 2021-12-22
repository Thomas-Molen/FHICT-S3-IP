using System;
using textadventure_backend.Services;
using Xunit;

namespace textadventure_backend.tests
{
    public class SessionManagerTests
    {
        private readonly ISessionManager _sut;
        public SessionManagerTests()
        {
            _sut = new SessionManager();
        }

        [Fact]
        public void Test1()
        {

        }
    }
}
