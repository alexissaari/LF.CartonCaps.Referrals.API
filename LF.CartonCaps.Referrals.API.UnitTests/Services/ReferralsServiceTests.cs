using LF.CartonCaps.Referrals.API.Models;
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
        public void GetReferrals_SHouldReturnUserReferrals()
        {
            // Arrange
            var expected = new List<Referral>
            {
                new Referral
                {
                    RefereeId = someReferralId,
                    FirstName = "Jane",
                    LastName = "Doe",
                    ReferralStatus = ReferralStatus.Sent
                }
            };
            this.usersDatabaseClientMock.Setup(x => x.GetReferrals(someUserId))
                .Returns(expected);

            // Act
            var actual = this.service.GetReferrals(someUserId);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetReferrals_ShouldNotReturnUserReferrals()
        {
            // Arrange
            IList<Referral> expected = null;
            this.usersDatabaseClientMock.Setup(x => x.GetReferrals(someUserId))
                .Returns(expected);

            // Act
            var actual = this.service.GetReferrals(someUserId);

            // Assert
            Assert.Null(actual);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetReferrals_Empty()
        {
            // Arrange
            var expected = new List<Referral>();
            this.usersDatabaseClientMock.Setup(x => x.GetReferrals(someUserId))
                .Returns(expected);

            // Act
            var actual = this.service.GetReferrals(someUserId);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(expected, actual);
        }
    }
}
