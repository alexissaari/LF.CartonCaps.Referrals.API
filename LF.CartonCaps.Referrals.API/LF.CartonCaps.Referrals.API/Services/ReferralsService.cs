using LF.CartonCaps.Referrals.API.Models;
using LF.CartonCaps.Referrals.API.Models.Abstractions;
using LF.CartonCaps.Referrals.API.Models.Exceptions;
using LF.CartonCaps.Referrals.API.Proxies;

namespace LF.CartonCaps.Referrals.API.Services
{
    public class ReferralsService : IReferralsService
    {
        public IList<Referral>? GetReferrals(string userId) => UsersDatabaseProxy.GetReferrals(userId);

        public void UpdateReferralStatus(string referralId, ReferralStatus referralStatus)
        {
            var activeReferral = ActiveReferralsDatabaseProxy.GetActiveReferral(referralId);
            if (activeReferral == null)
            {
                throw new ReferralDoesNotExistException($"Referral Not Found. ReferralId = {referralId}.", referralId);
            }

            // Update our internal store of active referees
            if (referralStatus == ReferralStatus.Complete)
            {
                ActiveReferralsDatabaseProxy.RemoveActiveReferral(referralId);
            }
            else
            {
                ActiveReferralsDatabaseProxy.UpdateActiveReferral(referralId, referralStatus);
            }

            // Update the user who referred this person
            UsersDatabaseProxy.UpdateReferralStatus(activeReferral.OriginatingReferralUserId, referralId, referralStatus);
        }

        public string InviteFriend(string userId, string firstName, string lastName)
        {
            var referral = UsersDatabaseProxy.GetReferral(userId, firstName, lastName);

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
            UsersDatabaseProxy.AddReferee(userId, newReferee);

            // Add this referee to our collection of active referees
            var activeReferral = new ActiveReferral()
            {
                ReferralStatus = ReferralStatus.Sent,
                OriginatingReferralUserId = userId
            };
            ActiveReferralsDatabaseProxy.AddActiveReferral(newReferee.RefereeId, userId);

            return newReferee.RefereeId;
        }
    }
}
