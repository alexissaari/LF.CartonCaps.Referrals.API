using LF.CartonCaps.Referrals.API.Models.Abstractions;
using LF.CartonCaps.Referrals.API.Services;
using Moq;

namespace LF.CartonCaps.Referrals.API.UnitTests.Services
{
    public class ReferralsServiceTests
    {
        private ReferralsService service;
        private readonly Mock<ICartonCapsApiClient> usersDatabaseClientMock;

        private static readonly string someUserId = "someUserId";
        private static readonly string someReferralId = "someReferralId";

        public ReferralsServiceTests()
        {
            this.usersDatabaseClientMock = new Mock<ICartonCapsApiClient>();
            this.service = new ReferralsService(this.usersDatabaseClientMock.Object);
        }

        [Fact]
        public void GetReferrals()
        {
            // Arrange
            

            // Act
            var response = this.service.GetReferrals(someUserId);

            // Assert
            Assert.NotNull(response);
        }
    }
}
