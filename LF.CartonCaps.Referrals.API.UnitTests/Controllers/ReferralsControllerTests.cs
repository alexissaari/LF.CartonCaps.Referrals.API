using LF.CartonCaps.Referrals.API.Controllers;
using LF.CartonCaps.Referrals.API.Models;
using LF.CartonCaps.Referrals.API.Models.Abstractions;
using LF.CartonCaps.Referrals.API.Models.Exceptions;
using Microsoft.Extensions.Logging;
using Moq;

namespace LF.CartonCaps.Referrals.API.UnitTests.Controllers
{
    public class ReferralsControllerTests
    {
        private Mock<ILogger<ReferralsController>> logger;
        private Mock<IReferralsService> referralsService;
        private ReferralsController referralsController;

        private static readonly string someUserId = "someUserId";
        private static readonly string someReferralId = "someReferralId";

        public ReferralsControllerTests() 
        {
            this.logger = new Mock<ILogger<ReferralsController>>();
            this.referralsService = new Mock<IReferralsService>();
            this.referralsController = new ReferralsController(this.logger.Object, this.referralsService.Object);
        }

        [Fact]
        public void GetReferrals()
        {
            // Arrange
            var expected = new List<Referral>() { new Referral() { RefereeId = "asdf", FirstName = "adf", LastName = "asd" }};
            this.referralsService
                .Setup(x => x.GetReferrals(someUserId))
                .Returns(expected);

            // Act
            var response = this.referralsController.GetReferrals(someUserId);
            var result = response?.Result as Microsoft.AspNetCore.Mvc.OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(expected, result.Value);
        }

        [Fact]
        public void GetReferrals_None()
        {
            // Arrange
            IList<Referral> expected = null;
            this.referralsService
                .Setup(x => x.GetReferrals(someUserId))
                .Returns(expected);

            // Act
            var response = this.referralsController.GetReferrals(someUserId);
            var result = response?.Result as Microsoft.AspNetCore.Mvc.OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Null(result.Value);
        }

        [Fact]
        public void GetReferrals_UserDoesNotExistException()
        {
            // Arrange
            this.referralsService
                .Setup(x => x.GetReferrals(someUserId))
                .Throws(new UserDoesNotExistException($"User Not Found. UserId = {someUserId}.", someUserId));

            // Act
            try
            {
                var response = this.referralsController.GetReferrals(someUserId);
            }
            catch (UserDoesNotExistException ex)
            {
                // Assert
                Assert.Equal(someUserId, ex.UserId);
                return;
            }
        }

        [Fact]
        public void PatchReferral()
        {
            // Arrange
            this.referralsService.Setup(x => x.UpdateReferralStatus(someReferralId, ReferralStatus.Pending));

            // Act
            var response = this.referralsController.PatchReferral(someReferralId, ReferralStatus.Pending);
            var result = response as Microsoft.AspNetCore.Mvc.OkResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public void PatchReferral_ReferralDoesNotExistException()
        {
            // Arrange
            this.referralsService
                .Setup(x => x.UpdateReferralStatus(someReferralId, ReferralStatus.Pending))
                .Throws(new ReferralDoesNotExistException($"Referral Not Found. ReferralId = {someReferralId}.", someReferralId));

            // Act
            try
            {
                var response = this.referralsController.PatchReferral(someReferralId, ReferralStatus.Pending);
            }
            catch (ReferralDoesNotExistException ex)
            {
                // Assert
                Assert.Equal(someReferralId, ex.ReferralId);
                Assert.Equal($"Referral Not Found. ReferralId = {someReferralId}.", ex.Message);
                return;
            }
        }

        [Fact]
        public void InviteFriend()
        {
            // Arrange
            var expected = someReferralId;
            this.referralsService
                .Setup(x => x.InviteFriend(someUserId, "Alexis", "Saari"))
                .Returns(expected);

            // Act
            var response = this.referralsController.InviteFriend(someUserId, "Alexis", "Saari");
            var result = response as Microsoft.AspNetCore.Mvc.OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(expected, result.Value);
        }

        [Fact]
        public void InviteFriend_UserDoesNotExistException()
        {
            // Arrange
            this.referralsService
                .Setup(x => x.InviteFriend(someUserId, "Alexis", "Saari"))
                .Throws(new UserDoesNotExistException($"User Not Found. UserId = {someUserId}.", someUserId));

            // Act
            try
            {
                var response = this.referralsController.InviteFriend(someUserId, "Alexis", "Saari");
            }
            catch (UserDoesNotExistException ex)
            {
                // Assert
                Assert.Equal(someUserId, ex.UserId);
                Assert.Equal($"User Not Found. UserId = {someUserId}.", ex.Message);
                return;
            }
        }
    }
}
