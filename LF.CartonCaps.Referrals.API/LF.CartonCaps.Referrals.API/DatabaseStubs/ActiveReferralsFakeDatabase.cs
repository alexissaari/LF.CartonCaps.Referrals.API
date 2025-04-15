using System.Collections.Concurrent;
using LF.CartonCaps.Referrals.API.Models;

namespace LF.CartonCaps.Referrals.API.Proxies
{
    public static class ActiveReferralsFakeDatabase
    {
        private static ConcurrentDictionary<string, ActiveReferral> activeReferees;
        
        static ActiveReferralsFakeDatabase()
        {
            activeReferees = new ConcurrentDictionary<string, ActiveReferral>();
        }

        public static ActiveReferral? GetActiveReferral(string referralId)
        {
            activeReferees.TryGetValue(referralId, out ActiveReferral? activeReferral);

            return activeReferral;
        }

        public static void AddActiveReferral(string referralId, string originatingReferralUserId)
        {
            var newReferee = new ActiveReferral()
            {
                ReferralStatus = ReferralStatus.Sent,
                OriginatingReferralUserId = originatingReferralUserId,
            };

            activeReferees.TryAdd(referralId, newReferee);
        }

        public static void UpdateActiveReferral(string referralId, ReferralStatus referralStatus)
        {
            activeReferees.TryGetValue(referralId,out ActiveReferral? activeReferral);
            if (activeReferral != null)
            {
                activeReferees[referralId].ReferralStatus = referralStatus;
            }
        }

        public static void RemoveActiveReferral(string referralId)
        {
            activeReferees.Remove(referralId, out ActiveReferral activeReferral);
        }
    }
}
