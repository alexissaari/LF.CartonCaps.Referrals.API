using LF.CartonCaps.Referrals.API.Models.Exceptions;
using LF.CartonCaps.Referrals.API.Proxies;

namespace LF.CartonCaps.Referrals.API.UnitTests.Proxies
{
    public class DatabaseProxyTests
    {
        private DatabaseProxy _proxy;

        public DatabaseProxyTests() 
        {
            _proxy = new DatabaseProxy();
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

        [Fact]
        public void GetReferrals_UserDoesNotExist()
        {
            var caughtException = Assert.Throws<UserDoesNotExistException>(() => _proxy.GetReferrals("1"));
            Assert.Contains("User Not Found. UserId = 1", caughtException.Message);
            Assert.Equal(caughtException.UserId, "1");
        }
    }
}
