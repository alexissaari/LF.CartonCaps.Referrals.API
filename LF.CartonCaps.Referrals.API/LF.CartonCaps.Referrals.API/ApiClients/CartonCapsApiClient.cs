using System.Collections.Concurrent;
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
        public IList<Referral>? GetReferrals(string userId)
        {
            return UsersDatastore.GetUser(userId)?.Referrals;
        }

        #region USER_REFERRALS

        public Referral? GetReferral(string userId, string firstName, string lastName)
        {
            var user = UsersDatastore.GetUser(userId);
            return user?.Referrals?.FirstOrDefault(x => x.FirstName == firstName && x.LastName == lastName);
        }

        public void AddReferee(string userId, Referral referral)
        {
            var user = UsersDatastore.GetUser(userId);

            if (user != null)
            {
                user.Referrals?.Add(referral);
                UsersDatastore.UpdateUser(userId, user);
            }
        }

        public void UpdateReferralStatus(string userId, string referralId, ReferralStatus referralStatus)
        {
            var user = UsersDatastore.GetUser(userId);
            if (user != null)
            {
                var referral = user?.Referrals?.FirstOrDefault(x => x.RefereeId == referralId);
                if (referral != null)
                {
                    referral.ReferralStatus = referralStatus;
                }
                UsersDatastore.UpdateUser(userId, user);
            }
        }

        #endregion USER_REFERRALS

        #region ACTIVE_REFERALS 

        public ActiveReferral? GetActiveReferral(string referralId)
        {
            return ActiveRefereesDatastore.GetActiveReferral(referralId);
        }

        public bool AddActiveReferral(string referralId, string originatingReferralUserId)
        {
            var newReferee = new ActiveReferral()
            {
                ReferralStatus = ReferralStatus.Sent,
                OriginatingReferralUserId = originatingReferralUserId,
            };

            return ActiveRefereesDatastore.AddActiveReferral(referralId, newReferee);
        }

        public bool UpdateActiveReferral(string referralId, ReferralStatus referralStatus)
        {
            return ActiveRefereesDatastore.UpdateActiveReferral(referralId, referralStatus);
        }

        public bool RemoveActiveReferral(string referralId)
        {
            return ActiveRefereesDatastore.RemoveActiveReferral(referralId);
        }

        #endregion ACTIVE_REFERALS
    }
}
