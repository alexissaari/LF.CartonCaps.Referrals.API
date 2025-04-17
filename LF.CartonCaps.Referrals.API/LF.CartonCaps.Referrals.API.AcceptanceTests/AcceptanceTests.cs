using System.Net;
using LF.CartonCaps.Referrals.API.Models;
using Newtonsoft.Json;

namespace LF.CartonCaps.Referrals.API.AcceptanceTests
{
    public class AcceptanceTests
    {
        [Fact]
        public async void GetAsync()
        {
            // Arrange
            var client = new HttpClient();
            var requestUri = "http://localhost:5244/Referrals/3";
            // Act
            var response = await client.GetAsync(requestUri);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            // Validate the response body
            var contentString = await response.Content.ReadAsStringAsync();
            Assert.NotEmpty(contentString);

            IList<Referral>? referrals = JsonConvert.DeserializeObject<IList<Referral>>(contentString);
            Assert.NotNull(referrals);
            Assert.NotEmpty(referrals);

            foreach (var referral in referrals)
            {
                Assert.NotNull(referral.RefereeId);
                Assert.NotNull(referral.FirstName);
                Assert.NotNull(referral.LastName);
                Assert.Equal(ReferralStatus.Sent, referral.ReferralStatus);
            }
        }
    }
}