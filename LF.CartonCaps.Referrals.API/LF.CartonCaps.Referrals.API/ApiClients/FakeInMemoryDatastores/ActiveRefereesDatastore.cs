using LF.CartonCaps.Referrals.API.Models;
using System.Collections.Concurrent;

namespace LF.CartonCaps.Referrals.API.ApiClients.FakeInMemoryDatastores
{
    /* 
     * This datastore is to keep track of ActiveReferrals, 
     * i.e. Status = Sent or Pending, but not yet Complete.
     * This allows for easier lookup and allows a referred person's status to be updated 
     * without knowing the userId of who referred them.
     * 
     * When a user refers a friend, the app will utilze a deep link 3rd party service for managing deep links.
     * The app will then call this REST Api to add a referral for the user with Status = Sent.
     * 
     * When the person being referred clicks their deep link, 
     * the deep link service will direct them to the app store
     * and send a response to our app to update this referred person's referral status to Pending.
     * 
     * When a referral completes the user registration process,
     * app will call this service to update the referral status to Complete.
     * Once completed, let's remove this referral from our collection of ActiveReferrals.
     */
    public static class ActiveReferralsDatastore
    {
        private static ConcurrentDictionary<string, ActiveReferral> activeReferrals;

        static ActiveReferralsDatastore()
        {
            activeReferrals = new ConcurrentDictionary<string, ActiveReferral>();
        }

        public static ActiveReferral? GetActiveReferral(string referralId)
        {
            var referralExists = activeReferrals.TryGetValue(referralId, out ActiveReferral? activeReferral);
            return referralExists ? activeReferral : null;
        }

        public static bool AddActiveReferral(string referralId, ActiveReferral newReferral)
        {
            return activeReferrals.TryAdd(referralId, newReferral);
        }

        public static bool UpdateActiveReferral(string referralId, ReferralStatus referralStatus)
        {
            if (activeReferrals.TryGetValue(referralId, out ActiveReferral? existingReferral))
            {
                var newReferral = new ActiveReferral()
                {
                    ReferralStatus = referralStatus,
                    OriginatingReferralUserId = existingReferral.OriginatingReferralUserId,
                };
                return activeReferrals.TryUpdate(referralId, newReferral, existingReferral);
            }

            return false;
        }

        public static bool RemoveActiveReferral(string referralId)
        {
            return activeReferrals.Remove(referralId, out ActiveReferral activeReferral);
        }
    }
}
