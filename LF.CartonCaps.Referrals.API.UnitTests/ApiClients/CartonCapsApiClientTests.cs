using LF.CartonCaps.Referrals.API.ApiClients.FakeInMemoryDatastores;
using LF.CartonCaps.Referrals.API.Models;
using LF.CartonCaps.Referrals.API.Models.Abstractions;
using LF.CartonCaps.Referrals.API.Models.Exceptions;
using LF.CartonCaps.Referrals.API.Proxies;

namespace LF.CartonCaps.Referrals.API.UnitTests.ApiClients
{
    /*
     * Because CartonCapsApiClient utilizes two static classes acting as in-memory datastores,
     * These tests are testing the functionality of CartonCapsApiClient and both in-memory datastores.
     */
    public class CartonCapsApiClientTests
    {
        private ICartonCapsApiClient client;

        private static readonly string userIdWithNullReferrals = "1";
        private static readonly string userIdWithEmptyReferrals = "2";
        private static readonly string userIdWithReferrals = "3";
        private static readonly string userIdThatDoesNotExist = "12345";
        private static readonly string referralId = "123";
        private static readonly string newReferralId = "asdf";

        public CartonCapsApiClientTests() {
            this.client = new CartonCapsApiClient();
        }

        #region USER_REFERRALS_TESTS

        [Fact]
        public void GetReferrals_ShouldReturnUserReferrals()
        {
            // Arrange
            var expected = new List<Referral>()
            {
                new Referral()
                {
                    ReferralId = "123",
                    FirstName = "Alexis",
                    LastName = "Saari",
                    ReferralStatus = ReferralStatus.Sent
                }
            };

            // Act
            var actual = this.client.GetReferrals(userIdWithReferrals);

            // Assert
            Assert.NotNull(actual);
            Assert.NotEmpty(actual);
            Assert.Equal(expected.FirstOrDefault()?.ReferralId, actual.FirstOrDefault()?.ReferralId);
            Assert.Equal(expected.FirstOrDefault()?.FirstName, actual.FirstOrDefault()?.FirstName);
            Assert.Equal(expected.FirstOrDefault()?.LastName, actual.FirstOrDefault()?.LastName);
            Assert.Equal(expected.FirstOrDefault()?.ReferralStatus, actual.FirstOrDefault()?.ReferralStatus);
        }

        [Fact]
        public void GetReferrals_ShouldReturnNullUserReferrals()
        {
            // Act
            var actual = this.client.GetReferrals(userIdWithNullReferrals);

            // Assert
            Assert.Null(actual);
        }

        [Fact]
        public void GetReferrals_ShouldReturnEmptyUserReferrals()
        {
            // Act
            var response = this.client.GetReferrals(userIdWithEmptyReferrals);

            // Assert
            Assert.NotNull(response);
            Assert.Empty(response);
        }

        [Fact]
        public void GetReferrals_ShouldThrowUserDoesNotExist()
        {
            // Arrange
            var datastoreResult = UsersDatastore.GetUser(userIdThatDoesNotExist);
            Assert.Null(datastoreResult);

            // Act
            try
            {
                var actual = this.client.GetReferrals(userIdThatDoesNotExist);
            }
            catch (UserDoesNotExistException ex)
            {
                // Assert
                Assert.NotNull(ex);
                Assert.Equal(userIdThatDoesNotExist, ex.UserId);
                return;
            }

            // If we get here, it means we didn't throw an error, so mark this test as failed.
            Assert.True(false);
        }

        [Fact]
        public void AddReferralToUser_ShouldAddReferralToNullReferralList()
        {
            // Arrange
            var newReferral = new Referral()
            {
                ReferralId = newReferralId,
                FirstName = "New",
                LastName = "Referral",
                ReferralStatus = ReferralStatus.Sent
            };
            var getReferralsResponse = this.client.GetReferrals(userIdWithNullReferrals);
            Assert.Null(getReferralsResponse);

            // Act
            var response = this.client.AddReferralToUser(userIdWithNullReferrals, newReferral);

            // Assert
            Assert.True(response);

            getReferralsResponse = this.client.GetReferrals(userIdWithNullReferrals);
            Assert.NotNull(getReferralsResponse);
            Assert.Single(getReferralsResponse);
        }

        [Fact]
        public void AddReferralToUser_ShouldAddReferralToExistingReferralList()
        {
            // Arrange
            var newReferral = new Referral()
            {
                ReferralId = newReferralId,
                FirstName = "New",
                LastName = "Referral",
                ReferralStatus = ReferralStatus.Sent
            };
            var getReferralsResponse = this.client.GetReferrals(userIdWithReferrals);
            Assert.NotNull(getReferralsResponse);
            var expectedReferralCount = getReferralsResponse.Count + 1;

            // Act
            var response = this.client.AddReferralToUser(userIdWithReferrals, newReferral);

            // Assert
            Assert.True(response);

            getReferralsResponse = this.client.GetReferrals(userIdWithReferrals);
            Assert.NotNull(getReferralsResponse);
            Assert.Equal(expectedReferralCount, getReferralsResponse.Count);
        }

        [Fact]
        public void AddReferralToUser_ShouldThrowUserDoesNotExist()
        {
            // Arrange
            var newReferral = new Referral()
            {
                ReferralId = newReferralId,
                FirstName = "New",
                LastName = "Referral",
                ReferralStatus = ReferralStatus.Sent
            };

            // Act
            try
            {
                var actual = this.client.AddReferralToUser(userIdThatDoesNotExist, newReferral);
            }
            catch (UserDoesNotExistException ex)
            {
                // Assert
                Assert.NotNull(ex);
                Assert.Equal(userIdThatDoesNotExist, ex.UserId);
                return;
            }

            // If we get here, it means we didn't throw an error, so mark this test as failed.
            Assert.True(false);
        }

        [Fact]
        public void UpdateReferralStatusForUser_ShouldUpdateReferralStatus()
        {
            // Arrange
            var expected = ReferralStatus.Pending;

            var getReferralsResponse = this.client.GetReferrals(userIdWithReferrals);
            Assert.NotNull(getReferralsResponse);
            var expectedReferralCount = getReferralsResponse.Count;

            // Act
            var response = this.client.UpdateReferralStatusForUser(userIdWithReferrals, referralId, expected);
            
            // Assert
            Assert.True(response);
            getReferralsResponse = this.client.GetReferrals(userIdWithReferrals);
            Assert.NotNull(getReferralsResponse);
            Assert.Equal(expected, getReferralsResponse.First().ReferralStatus);
            Assert.Equal(expectedReferralCount, getReferralsResponse.Count);
        }

        [Fact]
        public void UpdateReferralStatusForUser_ThrowsReferralDoesNotExistOnUser()
        {
            // Arrange
            var referralIdNotOnUser = Guid.NewGuid().ToString();
            
            // Act
            try
            {
                this.client.UpdateReferralStatusForUser(userIdWithReferrals, referralIdNotOnUser, ReferralStatus.Pending);
            }
            catch (ReferralDoesNotExistOnUserException ex)
            {
                // Assert
                Assert.NotNull(ex);
                Assert.Equal(userIdWithReferrals, ex.UserId);
                Assert.Equal(referralIdNotOnUser, ex.ReferralId);
                return;
            }

            // If we get here, it means we didn't throw an error, so mark this test as failed.
            Assert.True(false);
        }

        [Fact]
        public void UpdateReferralStatusForUser_ShouldThrowUserDoesNotExist()
        {
            // Act
            try
            {
                var actual = this.client.UpdateReferralStatusForUser(userIdThatDoesNotExist, referralId, ReferralStatus.Pending);
            }
            catch (UserDoesNotExistException ex)
            {
                // Assert
                Assert.NotNull(ex);
                Assert.Equal(userIdThatDoesNotExist, ex.UserId);
                return;
            }

            // If we get here, it means we didn't throw an error, so mark this test as failed.
            Assert.True(false);
        }

        #endregion USER_REFERRALS_TESTS

        #region ACTIVE_REFERALS_TESTS

        [Fact]
        public void AddAndGetActiveReferral_ShouldReturnActiveReferral()
        {
            // Arrange
            var expected = new ActiveReferral()
            {
                OriginatingReferralUserId = userIdWithReferrals,
                ReferralStatus = ReferralStatus.Sent
            };

            // Act
            var addSuccess = this.client.AddActiveReferral(referralId, expected.OriginatingReferralUserId);
            Assert.True(addSuccess);

            var actual = this.client.GetActiveReferral(referralId);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(expected.OriginatingReferralUserId, actual.OriginatingReferralUserId);
            Assert.Equal(expected.ReferralStatus, actual.ReferralStatus);
        }

        [Fact]
        public void CannotGetNonActiveReferral()
        {
            // Arrange
            var nonExistantReferralId = Guid.NewGuid().ToString();

            // Act
            var actual = this.client.GetActiveReferral(nonExistantReferralId);

            // Assert
            Assert.Null(actual);
        }

        [Fact]
        public void AddAndUpdateActiveReferral_ShouldUpdate()
        {
            // Arrange
            var originatingReferralUserId = Guid.NewGuid().ToString();
            var newReferralId = Guid.NewGuid().ToString();

            // Act - Add
            var addSuccess = this.client.AddActiveReferral(newReferralId, originatingReferralUserId);
            Assert.True(addSuccess);
            var addedActual = this.client.GetActiveReferral(newReferralId);

            // Assert
            Assert.NotNull(addedActual);
            Assert.Equal(originatingReferralUserId, addedActual.OriginatingReferralUserId);
            Assert.Equal(ReferralStatus.Sent, addedActual.ReferralStatus);

            // Act - Update
            var updateSuccess = this.client.UpdateActiveReferral(newReferralId, ReferralStatus.Pending);
            Assert.True(updateSuccess);
            var updatedActual = this.client.GetActiveReferral(newReferralId);

            // Assert
            Assert.NotNull(updatedActual);
            Assert.Equal(originatingReferralUserId, updatedActual.OriginatingReferralUserId);
            Assert.Equal(ReferralStatus.Pending, updatedActual.ReferralStatus);
        }

        [Fact]
        public void CannotUpdateNonActiveReferral()
        {
            // Arrange
            var expected = new ActiveReferral()
            {
                OriginatingReferralUserId = Guid.NewGuid().ToString(),
                ReferralStatus = ReferralStatus.Sent
            };
            var nonExistantReferralId = Guid.NewGuid().ToString();

            // Act
            var addSuccess = this.client.UpdateActiveReferral(nonExistantReferralId, ReferralStatus.Pending);
            Assert.False(addSuccess);
        }

        [Fact]
        public void AddAndRemoveActiveReferral()
        {
            // Arrange
            var originatingReferralUserId = Guid.NewGuid().ToString();
            var newReferralId = Guid.NewGuid().ToString();

            // Act - Add
            var addSuccess = this.client.AddActiveReferral(newReferralId, originatingReferralUserId);
            Assert.True(addSuccess);
            var addedActual = this.client.GetActiveReferral(newReferralId);

            // Assert
            Assert.NotNull(addedActual);
            Assert.Equal(originatingReferralUserId, addedActual.OriginatingReferralUserId);
            Assert.Equal(ReferralStatus.Sent, addedActual.ReferralStatus);

            // Act - Remove
            var removeSuccess = this.client.RemoveActiveReferral(newReferralId);
            Assert.True(removeSuccess);
            var removedActual = this.client.GetActiveReferral(newReferralId);

            // Assert
            Assert.Null(removedActual);
        }

        [Fact]
        public void CannotRemoveNonActiveReferral()
        {
            // Arrange
            var nonExistantReferralId = Guid.NewGuid().ToString();

            // Act
            var removeSuccess = this.client.RemoveActiveReferral(nonExistantReferralId);
            Assert.False(removeSuccess);
        }

        #endregion ACTIVE_REFERALS_TESTS
    }
}
