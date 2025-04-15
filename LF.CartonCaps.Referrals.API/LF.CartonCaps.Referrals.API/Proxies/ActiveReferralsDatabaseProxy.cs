using LF.CartonCaps.Referrals.API.Models;
using LF.CartonCaps.Referrals.API.Models.Abstractions;

namespace LF.CartonCaps.Referrals.API.Proxies
{
    public static class ActiveReferralsDatabaseProxy
    {
        private static Dictionary<string, ActiveReferral> activeReferees;

        static ActiveReferralsDatabaseProxy()
        {
            activeReferees = new Dictionary<string, ActiveReferral>();
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
            activeReferees.Remove(referralId);
        }
    }
}
