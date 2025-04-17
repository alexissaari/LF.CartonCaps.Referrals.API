using LF.CartonCaps.Referrals.API.ApiClients.FakeInMemoryDatastores;
using LF.CartonCaps.Referrals.API.Models;
using LF.CartonCaps.Referrals.API.Models.Abstractions;
using LF.CartonCaps.Referrals.API.Models.Exceptions;
using LF.CartonCaps.Referrals.API.Proxies;

namespace LF.CartonCaps.Referrals.API.Services
{
    /*
     * Services in this style of architecture are used to manage business logic.
     */
    public class ReferralsService : IReferralsService
    {
        private readonly ICartonCapsApiClient usersDatabaseClient;

        public ReferralsService(ICartonCapsApiClient usersDatabaseClient)
        {
            this.usersDatabaseClient = usersDatabaseClient;
        }

        public IList<Referral>? GetReferrals(string userId)
        {
            return this.usersDatabaseClient.GetReferrals(userId);
        }

        public bool UpdateReferralStatus(string referralId, ReferralStatus referralStatus)
        {
            var activeReferral = this.usersDatabaseClient.GetActiveReferral(referralId);
            if (activeReferral == null)
            {
                throw new ActiveReferralDoesNotExistException($"ActiveReferral not found. ReferralId = {referralId}.", referralId);
            }

            bool updateOrRemoveActiveReferralIsSuccessful;
            bool updatUserReferralIsSuccessful;

            // Update our internal store of active referees
            if (referralStatus == ReferralStatus.Complete)
            {
                updateOrRemoveActiveReferralIsSuccessful = this.usersDatabaseClient.RemoveActiveReferral(referralId);
            }
            else
            {
                updateOrRemoveActiveReferralIsSuccessful = this.usersDatabaseClient.UpdateActiveReferral(referralId, referralStatus);
            }

            // Update the user who referred this person
            updatUserReferralIsSuccessful = this.usersDatabaseClient.UpdateReferralStatusForUser(activeReferral.OriginatingReferralUserId, referralId, referralStatus);

            // If we have an issue updating the referral, but it's not due to the user or the referral not existing,
            // let's fail gracefully and not throw an exception.
            return updateOrRemoveActiveReferralIsSuccessful && updatUserReferralIsSuccessful;
        }

        public string InviteFriend(string userId, string firstName, string lastName)
        {
            var referrals = this.usersDatabaseClient.GetReferrals(userId);
            var referral = referrals?.FirstOrDefault(x => x.FirstName == firstName && x.LastName == lastName);

            // We have already referred this friend
            if (referral != null)
            {
                return referral.RefereeId;
            }

            // We have not referred this friend
            var refereeId = Guid.NewGuid().ToString();
            var newReferee = new Referral()
            {
                RefereeId = refereeId,
                FirstName = firstName,
                LastName = lastName,
                ReferralStatus = ReferralStatus.Sent
            };

            // Add this friend to our user's list of referees
            this.usersDatabaseClient.AddRefereeToUser(userId, newReferee);

            // Add this referee to our collection of active referees
            var activeReferral = new ActiveReferral()
            {
                ReferralStatus = ReferralStatus.Sent,
                OriginatingReferralUserId = userId
            };
            this.usersDatabaseClient.AddActiveReferral(newReferee.RefereeId, userId);

            return newReferee.RefereeId;
        }
    }
}
