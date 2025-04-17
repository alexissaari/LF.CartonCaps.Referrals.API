using LF.CartonCaps.Referrals.API.Controllers;
using LF.CartonCaps.Referrals.API.Models;
using LF.CartonCaps.Referrals.API.Models.Abstractions;
using LF.CartonCaps.Referrals.API.Models.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
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
            Mock<IUrlHelper> mockUrlHelper = new Mock<IUrlHelper>();
            mockUrlHelper.Setup(x => x.Action(It.IsAny<UrlActionContext>())).Returns("mocked-url");
            this.referralsController.Url = mockUrlHelper.Object;
        }

        [Fact]
        public void GetReferrals_ShouldReturnUserReferrals()
        {
            // Arrange
            var expected = new List<Referral>() { new Referral() { ReferralId = "asdf", FirstName = "adf", LastName = "asd" }};
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
        public void GetReferrals_ShouldNotReturnUserReferrals()
        {
            // Arrange
            IList<Referral> expected = null;
            this.referralsService
                .Setup(x => x.GetReferrals(someUserId))
                .Returns(expected);

            // Act
            var response = this.referralsController.GetReferrals(someUserId);
            var result = response?.Result as Microsoft.AspNetCore.Mvc.NoContentResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(204, result.StatusCode);
        }

        [Fact]
        public void GetReferrals_ShouldThrowUserDoesNotExistException()
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

            // If we get here, it means we didn't throw an error, so mark this test as failed.
            Assert.True(false);
        }

        [Fact]
        public void IsReferee()
        {
            // Arrange
            this.referralsService.Setup(x => x.IsReferral(It.IsAny<string>()))
                .Returns(true);

            // Act
            var response = this.referralsController.GetIsReferral(someReferralId);
            var result = response as Microsoft.AspNetCore.Mvc.OkResult;

            // Assert
            Assert.NotNull(response);
        }

        [Fact]
        public void PatchReferral_ShouldUpdateReferrals()
        {
            // Arrange
            this.referralsService.Setup(x => x.UpdateReferralStatus(someReferralId, ReferralStatus.Pending))
                .Returns(true);

            // Act
            var response = this.referralsController.PatchReferral(someReferralId, ReferralStatus.Pending);
            var result = response as Microsoft.AspNetCore.Mvc.OkResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public void PatchReferral_ShouldNotUpdateReferrals()
        {
            // Arrange
            this.referralsService.Setup(x => x.UpdateReferralStatus(someReferralId, ReferralStatus.Pending))
                .Returns(false);

            // Act
            var response = this.referralsController.PatchReferral(someReferralId, ReferralStatus.Pending);
            var result = response as Microsoft.AspNetCore.Mvc.BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public void PatchReferral_ShouldThrowActiveReferralDoesNotExistException()
        {
            // Arrange
            this.referralsService
                .Setup(x => x.UpdateReferralStatus(someReferralId, ReferralStatus.Pending))
                .Throws(new ActiveReferralDoesNotExistException($"ActiveReferral not found. ReferralId = {someReferralId}.", someReferralId));

            // Act
            try
            {
                var response = this.referralsController.PatchReferral(someReferralId, ReferralStatus.Pending);
            }
            catch (ActiveReferralDoesNotExistException ex)
            {
                // Assert
                Assert.Equal(someReferralId, ex.ReferralId);
                Assert.Equal($"ActiveReferral not found. ReferralId = {someReferralId}.", ex.Message);
                return;
            }
        }

        [Fact]
        public void InviteFriend_ShouldAddAFriend()
        {
            // Arrange
            var expected = someReferralId;
            this.referralsService
                .Setup(x => x.InviteFriend(someUserId, "Alexis", "Saari"))
                .Returns(expected);

            // Act
            var response = this.referralsController.InviteFriend(someUserId, "Alexis", "Saari");
            var result = response as Microsoft.AspNetCore.Mvc.CreatedResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(201, result.StatusCode);
            Assert.Equal(expected, result.Value);
        }

        [Fact]
        public void InviteFriend_ShouldNotAddAFriend()
        {
            // Arrange
            var expected = someReferralId;
            this.referralsService
                .Setup(x => x.InviteFriend(someUserId, "Alexis", "Saari"));

            // Act
            var response = this.referralsController.InviteFriend(someUserId, "Alexis", "Saari");
            var result = response as Microsoft.AspNetCore.Mvc.BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public void InviteFriend_ShouldThrowUserDoesNotExistException()
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

            // If we get here, it means we didn't throw an error, so mark this test as failed.
            Assert.True(false);
        }
    }
}
