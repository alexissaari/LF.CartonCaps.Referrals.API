using LF.CartonCaps.Referrals.API.Models.Exceptions;
using LF.CartonCaps.Referrals.API.Proxies;

namespace LF.CartonCaps.Referrals.API.UnitTests.Proxies
{
    public class DatabaseProxyTests
    {
        private UsersDatabaseProxy _proxy;

        public DatabaseProxyTests() 
        {
            _proxy = new UsersDatabaseProxy();
        }

        [Fact]
        public void GetReferrals_UserHasReferrals()
        {
            var result = _proxy.GetReferrals("3333");
            Assert.NotNull(result);
        }

        [Fact]
        public void GetReferrals_UserDoesNotHaveReferrals()
        {
            var result = _proxy.GetReferrals("1111");
            Assert.Null(result);
        }
    }
}
