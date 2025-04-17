using LF.CartonCaps.Referrals.API.Models;
using LF.CartonCaps.Referrals.API.Models.Abstractions;
using LF.CartonCaps.Referrals.API.Models.Exceptions;
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
        public void GetReferrals_ShouldReturnUserReferrals()
        {
            // Arrange
            var expected = new List<Referral>
            {
                new Referral
                {
                    ReferralId = someReferralId,
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

        [Fact]
        public void IsReferee()
        {
            // Arrange
            var refereeExistsId = Guid.NewGuid().ToString();
            this.usersDatabaseClientMock.Setup(x => x.GetActiveReferral(refereeExistsId))
                .Returns(new ActiveReferral()
                {
                    OriginatingReferralUserId = "123",
                    ReferralStatus = ReferralStatus.Pending
                });

            // Act
            var isReferral = this.service.IsReferral(refereeExistsId);

            // Assert
            Assert.True(isReferral);
        }

        [Fact]
        public void IsNotReferee()
        {
            // Arrange
            var refereeDoesNotExistsId = Guid.NewGuid().ToString();
            this.usersDatabaseClientMock.Setup(x => x.GetActiveReferral(refereeDoesNotExistsId));

            // Act
            var isReferral = this.service.IsReferral(refereeDoesNotExistsId);

            // Assert
            Assert.False(isReferral);
        }

        [Fact]
        public void UpdateReferralStatusToPending()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var referralId = Guid.NewGuid().ToString();
            var activeReferral = new ActiveReferral()
            {
                OriginatingReferralUserId = userId,
                ReferralStatus = ReferralStatus.Sent
            };
            this.usersDatabaseClientMock.Setup(x => x.GetActiveReferral(referralId))
                .Returns(activeReferral);
            this.usersDatabaseClientMock.Setup(x => x.UpdateActiveReferral(referralId, ReferralStatus.Pending))
                .Returns(true);
            this.usersDatabaseClientMock.Setup(x => x.UpdateReferralStatusForUser(userId, referralId, ReferralStatus.Pending))
                .Returns(true);

            // Act
            var result = this.service.UpdateReferralStatus(referralId, ReferralStatus.Pending);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void UpdateReferralStatusToComplete()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var referralId = Guid.NewGuid().ToString();
            var activeReferral = new ActiveReferral()
            {
                OriginatingReferralUserId = userId,
                ReferralStatus = ReferralStatus.Pending
            };
            this.usersDatabaseClientMock.Setup(x => x.GetActiveReferral(referralId))
                .Returns(activeReferral);
            this.usersDatabaseClientMock.Setup(x => x.RemoveActiveReferral(referralId))
                .Returns(true);
            this.usersDatabaseClientMock.Setup(x => x.UpdateReferralStatusForUser(userId, referralId, ReferralStatus.Complete))
                .Returns(true);

            // Act
            var result = this.service.UpdateReferralStatus(referralId, ReferralStatus.Complete);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void DidNotUpdateReferralStatus()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var referralId = Guid.NewGuid().ToString();
            var activeReferral = new ActiveReferral()
            {
                OriginatingReferralUserId = userId,
                ReferralStatus = ReferralStatus.Pending
            };
            this.usersDatabaseClientMock.Setup(x => x.GetActiveReferral(referralId))
                .Returns(activeReferral);
            this.usersDatabaseClientMock.Setup(x => x.RemoveActiveReferral(referralId))
                .Returns(false);
            this.usersDatabaseClientMock.Setup(x => x.UpdateReferralStatusForUser(userId, referralId, ReferralStatus.Complete))
                .Returns(true);

            // Act
            var result = this.service.UpdateReferralStatus(referralId, ReferralStatus.Complete);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void UpdateReferralStatus_ThrowsUserDoesNotExistException()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var referralId = Guid.NewGuid().ToString();
            var activeReferral = new ActiveReferral()
            {
                OriginatingReferralUserId = userId,
                ReferralStatus = ReferralStatus.Pending
            };
            this.usersDatabaseClientMock.Setup(x => x.GetActiveReferral(referralId))
                .Returns(activeReferral);
            this.usersDatabaseClientMock.Setup(x => x.RemoveActiveReferral(referralId))
                .Returns(false);
            this.usersDatabaseClientMock.Setup(x => x.UpdateReferralStatusForUser(userId, referralId, ReferralStatus.Complete))
                .Throws(new UserDoesNotExistException($"User Not Found. UserId = {userId}.", userId));

            // Act
            try
            {
                this.service.UpdateReferralStatus(referralId, ReferralStatus.Complete);
            }
            catch (UserDoesNotExistException ex)
            {
                // Assert
                Assert.Equal($"User Not Found. UserId = {userId}.", ex.Message);
                Assert.Equal(userId, ex.UserId);
                return;
            }

            // If we get here, it means we didn't throw an error, so mark this test as failed.
            Assert.True(false);
        }

        [Fact]
        public void UpdateReferralStatus_ThrowsReferralDoesNotExistOnUser()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var referralId = Guid.NewGuid().ToString();
            var activeReferral = new ActiveReferral()
            {
                OriginatingReferralUserId = userId,
                ReferralStatus = ReferralStatus.Pending
            };
            this.usersDatabaseClientMock.Setup(x => x.GetActiveReferral(referralId))
                .Returns(activeReferral);
            this.usersDatabaseClientMock.Setup(x => x.RemoveActiveReferral(referralId))
                .Returns(false);
            this.usersDatabaseClientMock.Setup(x => x.UpdateReferralStatusForUser(userId, referralId, ReferralStatus.Complete))
                .Throws(new UserDoesNotExistException($"User Not Found. UserId = {userId}.", userId));

            // Act
            try
            {
                this.service.UpdateReferralStatus(referralId, ReferralStatus.Complete);
            }
            catch (UserDoesNotExistException ex)
            {
                // Assert
                Assert.Equal($"User Not Found. UserId = {userId}.", ex.Message);
                Assert.Equal(userId, ex.UserId);
                return;
            }

            // If we get here, it means we didn't throw an error, so mark this test as failed.
            Assert.True(false);
        }


        [Fact]
        public void UpdateReferralStatus_ThrowsActiveReferralDoesNotExistException()
        {
            // Arrange
            var referralId = Guid.NewGuid().ToString();
            this.usersDatabaseClientMock.Setup(x => x.GetActiveReferral(referralId))
                .Throws(new ActiveReferralDoesNotExistException($"ActiveReferral not found. ReferralId = {referralId}.", referralId));

            // Act
            try
            {
                this.service.UpdateReferralStatus(referralId, ReferralStatus.Complete);
            }
            catch (ActiveReferralDoesNotExistException ex)
            {
                // Assert
                Assert.Equal($"ActiveReferral not found. ReferralId = {referralId}.", ex.Message);
                Assert.Equal(referralId, ex.ReferralId);
                return;
            }

            // If we get here, it means we didn't throw an error, so mark this test as failed.
            Assert.True(false);
        }

        [Fact]
        public void InviteFirstFriend()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var firstName = "Jane";
            var lastName = "Doe";
            this.usersDatabaseClientMock.Setup(x => x.GetReferrals(userId));
            this.usersDatabaseClientMock.Setup(x => x.AddReferralToUser(userId, It.IsAny<Referral>()))
                .Returns(true);
            this.usersDatabaseClientMock.Setup(x => x.AddActiveReferral(It.IsAny<string>(), userId))
                .Returns(true);

            // Act
            var newReferralsId = this.service.InviteFriend(userId, firstName, lastName);

            // Assert
            Assert.NotNull(newReferralsId);
        }

        [Fact]
        public void InviteFriend()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var firstName = "Jane";
            var lastName = "Doe";
            IList<Referral> existingReferrals = new List<Referral>()
            {
                new Referral()
                {
                    ReferralId = Guid.NewGuid().ToString(),
                    FirstName = "Alexis",
                    LastName = "Saari",
                    ReferralStatus = ReferralStatus.Complete
                }
            };
            this.usersDatabaseClientMock.Setup(x => x.GetReferrals(userId))
                .Returns(existingReferrals);
            this.usersDatabaseClientMock.Setup(x => x.AddReferralToUser(userId, It.IsAny<Referral>()))
                .Returns(true);
            this.usersDatabaseClientMock.Setup(x => x.AddActiveReferral(It.IsAny<string>(), userId))
                .Returns(true);

            // Act
            var newReferralsId = this.service.InviteFriend(userId, firstName, lastName);

            // Assert
            Assert.NotNull(newReferralsId);
        }

        [Fact]
        public void InviteFriend_DidNotAddReferralToUser()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var firstName = "Jane";
            var lastName = "Doe";
            this.usersDatabaseClientMock.Setup(x => x.GetReferrals(userId));
            this.usersDatabaseClientMock.Setup(x => x.AddReferralToUser(userId, It.IsAny<Referral>()))
                .Returns(false);
            this.usersDatabaseClientMock.Setup(x => x.AddActiveReferral(It.IsAny<string>(), userId))
                .Returns(true);

            // Act
            var newReferralsId = this.service.InviteFriend(userId, firstName, lastName);

            // Assert
            Assert.Null(newReferralsId);
        }

        [Fact]
        public void InviteFriend_DidNotAddActiveReferral()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var firstName = "Jane";
            var lastName = "Doe";
            this.usersDatabaseClientMock.Setup(x => x.GetReferrals(userId));
            this.usersDatabaseClientMock.Setup(x => x.AddReferralToUser(userId, It.IsAny<Referral>()))
                .Returns(true);
            this.usersDatabaseClientMock.Setup(x => x.AddActiveReferral(It.IsAny<string>(), userId))
                .Returns(false);

            // Act
            var newReferralsId = this.service.InviteFriend(userId, firstName, lastName);

            // Assert
            Assert.Null(newReferralsId);
        }
    }
}
