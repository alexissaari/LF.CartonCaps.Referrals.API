using LF.CartonCaps.Referrals.API.Models;
using System.Collections.Concurrent;

namespace LF.CartonCaps.Referrals.API.ApiClients.FakeInMemoryDatastores
{
    /* 
     * This datastore is to keep track of active referrals, 
     * i.e. Status = Sent or Pending, but not yet Complete.
     * 
     * When a user refers a friend by sharing their deep link, 
     * we will add the referral to this datastore with Status = Sent.
     * 
     * When that referee clicks their deeplink to open the app store,
     * the deeplink 3rd party service will notify our app.
     * From there, our app can call this service to update the referral status to Pending.
     * 
     * When a referee completes the user registration process,
     * app will call this service to update the referral status to Complete. 
     */
    public static class ActiveRefereesDatastore
    {
        private static ConcurrentDictionary<string, ActiveReferral> activeReferees;

        static ActiveRefereesDatastore()
        {
            activeReferees = new ConcurrentDictionary<string, ActiveReferral>();
        }

        public static ActiveReferral? GetActiveReferral(string referralId)
        {
            var referralExists = activeReferees.TryGetValue(referralId, out ActiveReferral? activeReferral);
            return referralExists ? activeReferral : null;
        }

        public static bool AddActiveReferral(string referralId, ActiveReferral newReferee)
        {
            return activeReferees.TryAdd(referralId, newReferee);
        }

        public static bool UpdateActiveReferral(string referralId, ReferralStatus referralStatus)
        {
            if (activeReferees.TryGetValue(referralId, out ActiveReferral? existingReferral))
            {
                var newReferral = new ActiveReferral()
                {
                    ReferralStatus = referralStatus,
                    OriginatingReferralUserId = existingReferral.OriginatingReferralUserId,
                };
                return activeReferees.TryUpdate(referralId, newReferral, existingReferral);
            }

            return false;
        }

        public static bool RemoveActiveReferral(string referralId)
        {
            return activeReferees.Remove(referralId, out ActiveReferral activeReferral);
        }
    }
}
