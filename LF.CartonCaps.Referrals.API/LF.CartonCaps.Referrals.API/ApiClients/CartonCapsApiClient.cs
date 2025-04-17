using LF.CartonCaps.Referrals.API.ApiClients.FakeInMemoryDatastores;
using LF.CartonCaps.Referrals.API.Models;
using LF.CartonCaps.Referrals.API.Models.Abstractions;
using LF.CartonCaps.Referrals.API.Models.Exceptions;

namespace LF.CartonCaps.Referrals.API.Proxies
{
    /* 
     * Ideally, REST Apis don't directly handle connections to a datastore and 
     * instead rely on calling another Api that is responsible for datastore CRUD operations.
     * 
     * However, for this example, we are going to keep it simple and use two fake in-memory datastores.
     * 
     * One datastore will be used to store users and their referrals.
     * The second datastore will be used to store active referrals, i.e. Status = Sent or Pending, but not Compelete.
     */
    public class CartonCapsApiClient : ICartonCapsApiClient
    {
        #region USER_REFERRALS

        public IList<Referral>? GetReferrals(string userId)
        {
            return GetUser(userId).Referrals;
        }

        public bool AddReferralToUser(string userId, Referral referral)
        {
            var user = GetUser(userId);
            if (user.Referrals == null)
            {
                user.Referrals = new List<Referral>();
            }

            user.Referrals.Add(referral);
            return UsersDatastore.UpdateUser(user);
        }

        public bool UpdateReferralStatusForUser(string userId, string referralId, ReferralStatus referralStatus)
        {
            var user = GetUser(userId);
            if (user != null)
            {
                var referral = GetReferral(user, referralId);
                if (referral == null)
                {
                    throw new ReferralDoesNotExistOnUserException($"Referral {referralId} does not exist on User {userId}", userId, referralId);
                }

                referral.ReferralStatus = referralStatus;
                return UsersDatastore.UpdateUser(user);
            }
            return false;
        }

        private User GetUser(string userId)
        {
            var user = UsersDatastore.GetUser(userId);
            if (user == null)
            {
                throw new UserDoesNotExistException($"User Not Found. UserId = {userId}.", userId);
            }
            return user;
        }

        private Referral? GetReferral(User user, string referralId)
        {
            return user.Referrals?.FirstOrDefault(x => x.ReferralId == referralId);
        }

        #endregion USER_REFERRALS

        #region ACTIVE_REFERALS 

        public ActiveReferral? GetActiveReferral(string referralId)
        {
            return ActiveReferralsDatastore.GetActiveReferral(referralId);
        }

        public bool AddActiveReferral(string referralId, string originatingReferralUserId)
        {
            var newReferral = new ActiveReferral()
            {
                ReferralStatus = ReferralStatus.Sent,
                OriginatingReferralUserId = originatingReferralUserId,
            };

            return ActiveReferralsDatastore.AddActiveReferral(referralId, newReferral);
        }

        public bool UpdateActiveReferral(string referralId, ReferralStatus referralStatus)
        {
            return ActiveReferralsDatastore.UpdateActiveReferral(referralId, referralStatus);
        }

        public bool RemoveActiveReferral(string referralId)
        {
            return ActiveReferralsDatastore.RemoveActiveReferral(referralId);
        }

        #endregion ACTIVE_REFERALS
    }
}
