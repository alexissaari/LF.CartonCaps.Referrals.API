using System.Net;
using LF.CartonCaps.Referrals.API.Models;

namespace LF.CartonCaps.Referrals.API.AcceptanceTests
{
    public class AcceptanceTests
    {
        HttpClient client;
        string baseUri = "http://localhost:5244/Referrals/";

        private static readonly string userIdWithNullReferrals = "1";
        private static readonly string userIdWithEmptyReferrals = "2";
        private static readonly string userIdWithReferrals = "3";
        private static readonly string userIdThatDoesNotExist = "12345";
        private static readonly string referralId = "123";
        private static readonly string newReferralId = "asdf";

        public AcceptanceTests()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(baseUri);
        }

        [Fact]
        public async void GetAddUpdateDeleteReferral()
        {
            // Get existing referrals for a user
            var existingReferrals = await Helper.GetReferralsReadAndDeserialize(client, userIdWithReferrals);
            var initialReferralCount = existingReferrals?.Count;

            // Add a new referral
            var newUserFirstName = Guid.NewGuid().ToString();
            var newUserLastName = Guid.NewGuid().ToString();
            var addRoute = $"InviteFriend/{userIdWithReferrals}/{newUserFirstName}/{newUserLastName}";

            var postResponse = await client.PostAsync(baseUri + addRoute, null);

            Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);
            var newReferralId = await postResponse.Content.ReadAsStringAsync();

            // Make sure the referral we just added is in the list
            var referrals = await Helper.GetReferralsReadAndDeserialize(client, userIdWithReferrals);
            var referralsCount = referrals?.Count;
            Assert.Equal(existingReferrals?.Count + 1, referralsCount);
            var newReferral = referrals?.FirstOrDefault(r => r.RefereeId == newReferralId);
            Assert.Equal(newReferralId, newReferral?.RefereeId);
            Assert.Equal(newUserFirstName, newReferral?.FirstName);
            Assert.Equal(newUserLastName, newReferral?.LastName);
            Assert.Equal(ReferralStatus.Sent, newReferral?.ReferralStatus);

            // Update our added referral to ReferralStatus.Pending
            var patchRoute = $"ReferralStatus/{newReferralId}/{ReferralStatus.Pending}";
            var patchResponse = await client.PatchAsync(baseUri + patchRoute, null);
            Assert.Equal(HttpStatusCode.OK, patchResponse.StatusCode);

            // Verify our update worked
            referrals = await Helper.GetReferralsReadAndDeserialize(client, userIdWithReferrals);
            newReferral = referrals?.FirstOrDefault(r => r.RefereeId == newReferralId);
            Assert.Equal(ReferralStatus.Pending, newReferral?.ReferralStatus);

            // Update our added referral to ReferralStatus.Complete
            patchRoute = $"ReferralStatus/{newReferralId}/{ReferralStatus.Complete}";
            patchResponse = await client.PatchAsync(baseUri + patchRoute, null);
            Assert.Equal(HttpStatusCode.OK, patchResponse.StatusCode);

            // Verify our update worked
            referrals = await Helper.GetReferralsReadAndDeserialize(client, userIdWithReferrals);
            newReferral = referrals?.FirstOrDefault(r => r.RefereeId == newReferralId);
            Assert.Equal(ReferralStatus.Complete, newReferral?.ReferralStatus);
        }

        [Fact]
        public async void GetReferralsForUserThatDoesNotExist()
        {
            // Act
            var response = await client.GetAsync(baseUri + userIdThatDoesNotExist);

            // Assert
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
            var contentString = await response.Content.ReadAsStringAsync();
            Assert.Contains("User Not Found. UserId = 12345.", contentString);
        }

        [Fact]
        public async void PatchReferralForReferralsThatDoesNotExist()
        {
            // Act
            var referralIdThatDoesNotExist = Guid.NewGuid().ToString();
            var patchRoute = $"ReferralStatus/{referralIdThatDoesNotExist}/{ReferralStatus.Pending}";

            // Assert
            var response = await client.PatchAsync(baseUri + patchRoute, null);
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
            var contentString = await response.Content.ReadAsStringAsync();
            Assert.Contains($"ActiveReferral not found. ReferralId = {referralIdThatDoesNotExist}.", contentString);
        }

        [Fact]
        public async void InviteFriendForUserThatDoesNotExist()
        {
            // Act
            var userId = Guid.NewGuid().ToString(); 
            var addRoute = $"InviteFriend/{userId}/Alexis/Saari";
            var response = await client.PostAsync(baseUri + addRoute, null);

            // Assert
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
            var contentString = await response.Content.ReadAsStringAsync();
            Assert.Contains($"User Not Found. UserId = {userId}.", contentString);
        }
    }
}